using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseOperations.Common
{
    public class TableInfoDto
    {
        [Column("COLUMN_NAME")]
        public string Name { get; set; }

        [Column("DATA_TYPE")]
        public string DataType { get; set; }

        [Column("IS_NULLABLE")]
        public string IsNullable { get; set; }

        [Column("CHARACTER_MAXIMUM_LENGTH")]
        public int CharMaxLength { get; set; }

        [Column("ORDINAL_POSITION")]
        public int OrdinalPosition { get; set; }

        [Column("COLUMN_DEFAULT")]
        public string ColumnDefault { get; set; }

        [Column("CHARACTER_OCTET_LENGTH")]
        public int OctetLength { get; set; }

        [Column("NUMERIC_PRECISION")]
        public int NumericPrecision { get; set; }
    }
}
