namespace DatabaseOperations.Services
{
    public class CompareResponse
    {
        public CompareResponse()
        {
            this.ColumnsWithSameName = new List<CompareColumn>();
            this.FirstTableColumns = new List<string>();
            this.SecondTableColumns = new List<string>();
        }

        // Columns first table have
        public List<string> FirstTableColumns { get; set; }

        // Columns second table table have
        public List<string> SecondTableColumns { get; set; }

        // Columns sharing same name with other compared properties like DataType, IsNullable etc.
        public List<CompareColumn> ColumnsWithSameName { get; set; }
    }

    public class CompareColumn
    {
        public CompareColumn(string columnName)
        {
            this.ColumnName = columnName;
            this.MatchingProperties = new List<ColumnProperty>();
            this.UnMatchingProperties = new List<UnMatchedColumnProperty>();
        }

        public string ColumnName { get; set; }

        public List<ColumnProperty> MatchingProperties { get; set; }

        public List<UnMatchedColumnProperty> UnMatchingProperties { get; set; }
    }

    public class ColumnProperty
    {
        public ColumnProperty(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class UnMatchedColumnProperty
    {
        public UnMatchedColumnProperty(string name, string fValue, string sValue)
        {
            this.Name = name;
            this.FirstValue = fValue;
            this.SecondValue = sValue;
        }

        public string Name { get; set; }

        public string FirstValue { get; set; }

        public string SecondValue { get; set; }
    }
}
