﻿namespace DatabaseOperations.Services.TableCompare
{
    public class CompareColumn
    {
        public CompareColumn(string columnName)
        {
            this.ColumnName = columnName;
            this.MatchingProperties = new List<ColumnProperty>();
            this.UnmatchingProperties = new List<UnMatchedColumnProperty>();
        }

        public string ColumnName { get; set; }

        public List<ColumnProperty> MatchingProperties { get; set; }

        public List<UnMatchedColumnProperty> UnmatchingProperties { get; set; }
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
