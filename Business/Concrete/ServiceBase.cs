using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
namespace Business.Concrete
{
    public class ServiceBase
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ServiceBase()
        {

        }

        protected Guid GetCurrentUserId()
        {
            IServiceCollection services = new ServiceCollection();

            var serviceProvider = services.BuildServiceProvider();
            
            using (var scope = serviceProvider.CreateScope())
            {
                _httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();

            }

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, out var userId);
            return userId;
        }
    }
}
