using Dapper;
using DatabaseOperations.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Reflection;

namespace DatabaseOperations.DBContext
{
    public class DAContext
    {
        private readonly string connectionString;

        public DAContext(IConfiguration config)
        {
            this.connectionString = config.GetConnectionString("Main");

            // Configure dapper
            var customTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IQueryResult)));

            foreach (var customType in customTypes)
            {
                SqlMapper.SetTypeMap(
                customType,
                new CustomPropertyTypeMap(
                    customType,
                    (type, columnName) => type.GetProperties()
                        .FirstOrDefault(prop => prop.GetCustomAttributes(false)
                        .OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using var connection = new SqlConnection(this.connectionString);
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using var connection = new SqlConnection(this.connectionString);
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }
    }
}
