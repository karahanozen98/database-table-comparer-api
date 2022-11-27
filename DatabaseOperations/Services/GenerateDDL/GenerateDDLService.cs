using DatabaseOperations.Common;
using DatabaseOperations.DBContext;

namespace DatabaseOperations.Services.GenerateDDL
{
    public class GenerateDDLService
    {
        private readonly DAContext _context;

        public GenerateDDLService(DAContext context)
        {
            this._context = context;
        }

        public async Task<string> GenerateDDL(string objectName)
        {
            var sql = "SELECT * FROM sys.objects WHERE name = @ObjectName";
            var dbObject = await this._context
                .QueryFirstOrDefaultAsync<SystemObjectDto>(sql, new { ObjectName = objectName });

            if (dbObject == null)
            {
                throw new Exception($"Object names {objectName} not found in current database");
            }

            switch (dbObject.TypeDesc)
            {
                case DatabaseObjectTypes.Table:
                    return await this.GenerateTableDDL(objectName);
                case DatabaseObjectTypes.StoredProcedure:
                case DatabaseObjectTypes.View:
                case DatabaseObjectTypes.TableValuedFunc:
                case DatabaseObjectTypes.InlineTableValuedFunc:
                case DatabaseObjectTypes.SclarFunc:
                    return await this.ReadDefinition(dbObject.Id);
                default:
                    throw new Exception($"Unknown Database Object Type {dbObject.TypeDesc}");
            }
        }

        private async Task<string> GenerateTableDDL(string tableName)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            var param = new { TableName = tableName };
            var tableColumns = (await this._context
                .QueryAsync<TableInfoDto>(sql, param))
                .OrderBy(col => col.OrdinalPosition);
            var length = tableColumns.Count();
            var columnDefinition = string.Empty;

            for (var i = 0; i < length; i++)
            {
                var col = tableColumns.ElementAt(i);
                columnDefinition += $"\t{col.Name}";
                columnDefinition += $" {(col.CharMaxLength != null && col.CharMaxLength > 0 ? $"{col.DataType}({col.CharMaxLength})" : col.DataType)}";
                columnDefinition += col.IsNullable == "NO" ? " NOT NULL" : string.Empty;
                columnDefinition += (i == length - 1 ? string.Empty : ",\n");
            }

            if (columnDefinition == string.Empty)
            {
                throw new Exception("Table columns can not be empty");
            }

            var contraintDefinition = await this.CreateConstraintDefinition(tableName);

            if (contraintDefinition != String.Empty)
            {
                columnDefinition += "\n" + contraintDefinition;
            }

            return $"CREATE TABLE [dbo].[{tableName}] (\n{columnDefinition}\n)";
        }

        private async Task<string> ReadDefinition(string objectId)
        {
            var sql = "SELECT definition FROM sys.sql_modules WHERE object_id = @ObjectId";
            var definition = await this._context
                .QueryFirstOrDefaultAsync<string>(sql, new { ObjectId = objectId });

            return definition;
        }

        private async Task<string> CreateConstraintDefinition(string tableName)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE WHERE TABLE_NAME = @TableName";
            var tableConstraints = await this._context.QueryAsync<TableConstraint>(sql, new { TableName = tableName });
            var columnDefinition = new List<string>();

            foreach (var contraint in tableConstraints)
            {
                var parts = contraint.ConstraintName.Split("_");
                var contraintType = parts[0];

                if (contraintType == "PK")
                {
                    columnDefinition.Add($"PRIMARY KEY ({parts[2]})");
                }
                else if (contraintType == "FK")
                {
                    var refPath = $"[{contraint.Catalog }].[{contraint.TableSchema}].{parts[2]}({parts[3]})";
                    columnDefinition.Add($"FOREIGN KEY ({parts[3]}) REFERENCES {refPath}");
                }
            }

            return string.Join(",\n", columnDefinition.Select(cd => $"\t{cd}"));
        }

    }
}
