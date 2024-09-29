using BuyerService.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ControllerBases;
using Shared.Models;

namespace BuyerService.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : CustomBaseController
    {
        private readonly IBuyerManageService _buyerManageService;

        public BuyerController(IBuyerManageService buyerManageService)
        {
            _buyerManageService = buyerManageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadInvoice(Invoice invoice)
        {
            var uploadedInvoice = await _buyerManageService.UploadInvoice(invoice);

            return CreateActionResultInstance(uploadedInvoice);
        }
    }
}
