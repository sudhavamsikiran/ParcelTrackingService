namespace ParcelTracking.API.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ParcelTracking.Application.Interfaces.IParcelService, ParcelTracking.Application.Services.ParcelService>();
            return services;
        }
    }
}
