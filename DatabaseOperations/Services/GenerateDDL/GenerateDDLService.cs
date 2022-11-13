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
            var sql = "SELECT name, type_desc as typeDesc FROM sys.objects WHERE name = @ObjectName";
            var dbObject = await this._context.QueryFirstOrDefaultAsync<SystemObjectDto>(sql, new { ObjectName = objectName });

            if (dbObject == null)
            {
                throw new Exception($"Object names {objectName} not found in current database");
            }

            switch (dbObject.TypeDesc)
            {
                case DatabaseObjectTypes.Table:
                    return await this.GenerateTableDDL(objectName);
                case DatabaseObjectTypes.StoredProcedure:
                case DatabaseObjectTypes.TableValuedFunc:
                case DatabaseObjectTypes.InlineTableValuedFunc:
                case DatabaseObjectTypes.SclarFunc:
                    return await this.GenerateSPDDL(objectName);
                default:
                    throw new Exception($"Unknown Database Object Type {dbObject.TypeDesc}");
            }
        }

        private async Task<string> GenerateTableDDL(string tableName)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            var tableColumns = await this._context.QueryAsync<TableInfoDto>(sql, new { TableName = tableName });
            var length = tableColumns.Count();
            var columns = string.Empty;

            for (var i = 0; i < length; i++)
            {
                var col = tableColumns.ElementAt(i);
                columns += $"{col.Name} {col.DataType}{(i == length - 1 ? string.Empty : ",")}";
            }

            if (columns == string.Empty)
            {
                throw new Exception("Table columns can not be empty");
            }

            return $"CREATE TABLE {tableName} ({columns})";
        }

        private async Task<string> GenerateSPDDL(string procName)
        {
            var ddl = await this._context.QueryAsync<string>($"SP_HELPTEXT {procName}");
            return String.Join('\n', ddl);
        }
    }
}
