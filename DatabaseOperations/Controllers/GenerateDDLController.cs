using DatabaseOperations.Services.GenerateDDL;
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
        public async Task<string> GenerateDDL(string objectName)
        {
            return await this.generateDDLService.GenerateDDL(objectName);
        }
    }
}
