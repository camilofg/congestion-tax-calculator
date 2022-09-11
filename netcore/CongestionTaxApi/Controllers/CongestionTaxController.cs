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
            try
            {
                var result = _taxCalculatorService.GetTax(taxDto.Vehicle, taxDto.Dates);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex);
            }
            
        }


        [HttpPost("GetTollFee")]
        public ActionResult GetTollFee([FromBody] TollFeeDto tollFeeDto)
        {
            try
            {
                var result = _taxCalculatorService.GetTollFee(tollFeeDto.Vehicle, tollFeeDto.Date);
                return StatusCode(200, result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex);
            }
        }
    }
}
