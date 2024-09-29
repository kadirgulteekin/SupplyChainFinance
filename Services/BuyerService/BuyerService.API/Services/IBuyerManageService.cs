using Shared.Dtos;
using Shared.Models;

namespace BuyerService.API.Services
{
    public interface IBuyerManageService
    {
        Task<ResponseDto<Invoice>> UploadInvoice(Invoice invoice);
    }
}
