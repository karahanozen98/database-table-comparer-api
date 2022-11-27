using DatabaseOperations.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseOperations.Services.GenerateDDL
{
    public class TableConstraint : IQueryResult
    {
        [Column("CONSTRAINT_CATALOG")]
        public string Catalog { get; set; }

        [Column("TABLE_SCHEMA")]
        public string TableSchema { get; set; }

        [Column("CONSTRAINT_NAME")]
        public string ConstraintName { get; set; }
    }
}
