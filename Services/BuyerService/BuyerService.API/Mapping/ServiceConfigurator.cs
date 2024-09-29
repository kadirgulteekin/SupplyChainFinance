using BuyerService.API.Services;
using System.Dynamic;

namespace BuyerService.API.Mapping
{
    public class ServiceConfigurator 
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBuyerManageService, BuyerManageService>();

        }
    }
}
