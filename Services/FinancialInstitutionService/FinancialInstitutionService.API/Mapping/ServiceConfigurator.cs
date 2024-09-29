using FinancialInstitutionService.API.Services;

namespace FinancialInstitutionService.API.Mapping
{
    public class ServiceConfigurator
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFinancialInstitutionService, Services.FinancialInstitutionService>();

        }
    }
}
