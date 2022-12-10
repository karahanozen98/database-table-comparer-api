using DatabaseOperations.Common;
using DatabaseOperations.DBContext;

namespace DatabaseOperations.Services.TableCompare
{
    public class TableCompareService
    {
        private readonly DAContext _context;

        public TableCompareService(DAContext context)
        {
            this._context = context;
        }

        public async Task<CompareResponse> Compare(CompareQuery query)
        {
            var result = new CompareResponse();
            var firstTableColumns = await this.ReadTableColumnInfo(query.First);
            var secondTableColumns = await this.ReadTableColumnInfo(query.Second);
            result.FirstTableColumns = firstTableColumns.Select(col => col.Name).ToList();
            result.SecondTableColumns = secondTableColumns.Select(col => col.Name).ToList();
            var compareProperties = query.GetType().GetProperties()
                .Where(p => p.GetValue(query)?.ToString() == "True")
                .Select(p => p.Name.Replace("Compare", string.Empty));

            foreach (var column in firstTableColumns)
            {
                var match = secondTableColumns.FirstOrDefault(col => col.Name == column.Name);

                if (match != null)
                {
                    var compareColumn = new CompareColumn(column.Name);

                    foreach (var property in compareProperties)
                    {
                        var firstPropertyValue = column.GetType().GetProperty(property)?.GetValue(column);
                        var secondPropertyValue = match.GetType().GetProperty(property)?.GetValue(match);

                        if (Object.Equals(firstPropertyValue, secondPropertyValue))
                        {
                            compareColumn.MatchingProperties.Add(
                                new ColumnProperty(property, this.ToSafeString(firstPropertyValue)));
                        }
                        else
                        {
                            compareColumn.UnmatchingProperties.Add(
                                new UnMatchedColumnProperty(property,
                                this.ToSafeString(firstPropertyValue),
                                this.ToSafeString(secondPropertyValue)));
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

        private async Task<IEnumerable<TableInfoDto>> ReadTableColumnInfo(string tableName)
        {
            var sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            var parameters = new { TableName = tableName };

            return await this._context.QueryAsync<TableInfoDto>(sql, parameters);
        }
    }
}
