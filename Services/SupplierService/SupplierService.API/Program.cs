using BuyerService.Infrastructure.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using SupplierService.API.Consumers;
using SupplierService.API.Mapping;
using SupplierService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ServiceConfigurator.Configure(builder.Services);

builder.Services.AddDbContext<SupplierDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.MigrationsAssembly("SupplierService.Infrastructure");
    });
});


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<InvoiceUploadedEventConsumer>(); 
    x.AddConsumer<PaymentCompletedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("invoice-uploaded-event", e =>
        {
            e.ConfigureConsumer<InvoiceUploadedEventConsumer>(context); 
        });

        cfg.ReceiveEndpoint("payment-completed-event", e =>
        {
            e.ConfigureConsumer<PaymentCompletedConsumer>(context);
        });


    });
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_supplier";
    options.RequireHttpsMetadata = false;
});

var app = builder.Build();


app.UseRouting();

app.UseAuthentication();


app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
