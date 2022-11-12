using Dapper;
using DbTableComparerApi.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;

namespace DbTableComparerApi.DBContext
{
    public class DAContext
    {
        private readonly string connectionString;

        public DAContext(IConfiguration config)
        {
            this.connectionString = config.GetConnectionString("Main");

            // Configure dapper
            SqlMapper.SetTypeMap(
                typeof(TableInfoDto),
                new CustomPropertyTypeMap(typeof(TableInfoDto),
                (type, columnName) => type.GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(false)
                                  .OfType<ColumnAttribute>()
                                  .Any(attr => attr.Name == columnName))));
        }

        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            using var connection = new SqlConnection(this.connectionString);
            return connection.Query<T>(sql, parameters);
        }
    }
}
