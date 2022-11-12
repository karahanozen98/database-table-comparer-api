using DatabaseOperations.Common;
using DatabaseOperations.DBContext;

namespace DatabaseOperations.Services
{
    public class TableCompareService
    {
        public DAContext context { get; }

        public TableCompareService(DAContext context)
        {
            this.context = context;
        }

        public CompareResponse Compare(CompareQuery query)
        {
            var result = new CompareResponse();
            var firstTable = this.TeadTableInfo(query.First);
            var secondTable = this.TeadTableInfo(query.Second);
            result.FirstTableColumns = firstTable.Select(col => col.Name).ToList();
            result.SecondTableColumns = secondTable.Select(col => col.Name).ToList();
            var compareProperties = query.GetType().GetProperties()
                .Where(p => p.GetValue(query)?.ToString() == "True")
                .Select(p => p.Name.Replace("Compare", string.Empty));

            foreach (var column in firstTable)
            {
                var match = secondTable.FirstOrDefault(col => col.Name == column.Name);

                if (match != null)
                {
                    var compareColumn = new CompareColumn(column.Name);

                    foreach (var property in compareProperties)
                    {
                        var value = column.GetType().GetProperty(property)?.GetValue(column);
                        var secondValue = match.GetType().GetProperty(property)?.GetValue(match);

                        if (Object.Equals(value, secondValue))
                        {
                            compareColumn.MatchingProperties.Add(new ColumnProperty(property, this.ToSafeString(value)));
                        }
                        else
                        {
                            compareColumn.UnMatchingProperties.Add(
                                new UnMatchedColumnProperty(property, this.ToSafeString(value), this.ToSafeString(secondValue)));
                        }
                    }

                    result.ColumnsWithSameName.Add(compareColumn);
                }
            }

            return result;
        }

        private string ToSafeString(object value)
        {
            if (value == null)
            {
                return "null";
            }

            return Convert.ToString(value);
        }

        private IEnumerable<TableInfoDto> TeadTableInfo(string tableName)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            var parameters = new { TableName = tableName };

            return this.context.Query<TableInfoDto>(sql, parameters);
        }
    }
}
