using Congestion_Models.Dto;
using CongestionTaxApi.Filters;
using CongestionTaxApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxApi.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class CongestionTaxController : ControllerBase
    {
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly ILogger<CongestionTaxController> _logger;

        public CongestionTaxController(ITaxCalculatorService taxCalculatorService, ILogger<CongestionTaxController> logger)
        {
            _taxCalculatorService = taxCalculatorService;
            _logger = logger;
        }

        [HttpPost("GetTax")]
        public ActionResult GetTax([FromBody] TaxDto taxDto)
        {
            var result = _taxCalculatorService.GetTax(taxDto.Vehicle, taxDto.Dates);
            return StatusCode(200, result);
        }


        [HttpPost("GetTollFee")]
        public ActionResult GetTollFee([FromBody] TollFeeDto tollFeeDto)
        {
            var result = _taxCalculatorService.GetTollFee(tollFeeDto.Vehicle, tollFeeDto.Date);
            return StatusCode(200, result);
        }
    }
}
