using DatabaseOperations.Services.TableCompare;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseOperations.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TableCompareController : ControllerBase
    {
        private readonly TableCompareService _tableCompareService;

        public TableCompareController(TableCompareService tableCompareService)
        {
            this._tableCompareService = tableCompareService;
        }



        [HttpGet("compare")]
        public async Task<CompareResponse> Compare([FromQuery] CompareQuery query)
        {
            return await this._tableCompareService.Compare(query);
        }

    }
}
