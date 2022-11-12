namespace DbTableComparerApi.Services
{
    public class CompareQuery
    {
        public string First { get; set; }

        public string Second { get; set; }

        public bool? CompareDataType { get; set; }

        public bool? CompareIsNullable { get; set; }

        public bool? CompareOrdinalPosition { get; set; }

        public bool? CompareColumnDefault { get; set; }

        public bool? CompareCharMaxLength { get; set; }

        public bool? CompareCharOctetLength { get; set; }

        public bool? CompareNumericPrecision { get; set; }
    }
}
