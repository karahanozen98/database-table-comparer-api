namespace DatabaseOperations.Services.TableCompare
{
    public class CompareResponse
    {
        public CompareResponse()
        {
            this.ColumnsWithSameName = new List<CompareColumn>();
            this.FirstTableColumns = new List<string>();
            this.SecondTableColumns = new List<string>();
        }

        public List<string> FirstTableColumns { get; set; }

        public List<string> SecondTableColumns { get; set; }

        public List<CompareColumn> ColumnsWithSameName { get; set; }
    }
}
