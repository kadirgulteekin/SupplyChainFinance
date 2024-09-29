using FinancialInstitutionService.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ControllerBases;
using Shared.Dtos;

namespace FinancialInstitutionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialInstitutionController : CustomBaseController
    {
        private readonly IFinancialInstitutionService _financialInstitutionService;

        public FinancialInstitutionController(IFinancialInstitutionService financialInstitutionService)
        {
            _financialInstitutionService = financialInstitutionService;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> ProcessPayment(PaymentRequestDto paymentRequest)
        {
           
            try
            {

                await _financialInstitutionService.ProcessPayment(paymentRequest.InvoiceNumber, paymentRequest.PaymentAmount);

                return Ok(ResponseDto<string>.Success("Payment processed successfully", StatusCodes.Status200OK));
            }
            catch (Exception ex)
            {
                // Hata loglanabilir veya detaylı hata mesajı döndürülebilir
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseDto<string>.Failed(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

    }
}
