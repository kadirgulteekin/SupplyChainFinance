using SupplierService.API.Services;

namespace SupplierService.API.Mapping
{
    public class ServiceConfigurator
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISupplierServices, SupplierServices>();

        }
    }
}

