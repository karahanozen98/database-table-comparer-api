using DatabaseOperations.DBContext;

namespace DatabaseOperations.Services
{
    public class GenerateDDLService
    {
        public DAContext context { get; }

        public GenerateDDLService(DAContext context)
        {
            this.context = context;
        }

        public string GenerateDDL(string objectName)
        {
            return string.Empty;
        }
    }
}
