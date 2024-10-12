using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using System.Security.Claims;

namespace WebApI
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase 
    {
        private const string Source = "Samavat API";
        protected readonly IServiceContainer serviceContainer;
        private System.ComponentModel.Design.IServiceContainer serviceContainer1;

        // Max Code = 000043
        public BaseController(IServiceContainer _serviceContainer)
        {
            serviceContainer = _serviceContainer;
        }

        protected BaseController(System.ComponentModel.Design.IServiceContainer serviceContainer1)
        {
            this.serviceContainer1 = serviceContainer1;
        }

        
    }
}
