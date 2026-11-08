
using Microsoft.AspNetCore.Mvc;
using onine.core.Repositories;
using onine.Repository;
using onine.services;
using onine.services.Interfaces;
using onineproject.Errors;
using onineproject.Helpers;

namespace onineproject.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped<ProductpictureurlResolver>();

            services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>));

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IOrderServices, OrderServices>();

            services.AddAutoMapper(
                m => m.AddProfile(new MappingProfiles()));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(p => p.Value != null &&
                                    p.Value.Errors.Count > 0)
                        .SelectMany(p => p.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var validationErrorResponse =
                        new ApiValidationErrorResponse
                        {
                            Errors = errors
                        };

                    return new BadRequestObjectResult(
                        validationErrorResponse);
                };
            });

            return services;
        }
    }
}
