using DatabaseOperations.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseOperations.Controllers
{
    public class GenerateDDLController : Controller
    {
        private readonly GenerateDDLService generateDDLService;

        public GenerateDDLController(GenerateDDLService generateDDLService)
        {
            this.generateDDLService = generateDDLService;
        }

        [HttpGet("{objectName}")]
        public string GenerateDDL(string objectName)
        {
            return this.generateDDLService.GenerateDDL(objectName);
        }
    }
}
