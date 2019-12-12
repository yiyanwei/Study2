using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreStudy.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspnetCoreStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        public OperationService OperationService { get; }
        public IOperationTransient TransientOperation { get; }
        //public IOperationScoped ScopedOperation { get; }
        public IOperationSingleton SingletonOperation { get; }
        public IOperationSingletonInstance SingletonInstanceOperation { get; }

        public OperationController(OperationService operationService,
        IOperationTransient transientOperation,
        //IOperationScoped scopedOperation,
        IOperationSingleton singletonOperation,
        IOperationSingletonInstance singletonInstanceOperation)
        {
            OperationService = operationService;
            TransientOperation = transientOperation;
            //ScopedOperation = scopedOperation;
            SingletonOperation = singletonOperation;
            SingletonInstanceOperation = singletonInstanceOperation;
        }

        [HttpGet]
        public string GetValue()
        {
            var serviceProvider = this.HttpContext.RequestServices;
            var logger = serviceProvider.GetRequiredService<ILogger<OperationController>>();
            //写入日志,OperationServices
            logger.LogInformation(
                string.Format("Services Transient: {0}", this.OperationService.TransientOperation.OperationId.ToString()));
            //logger.LogInformation(
             //   string.Format("Services Scoped: {0}", this.OperationService.ScopedOperation.OperationId.ToString()));
            logger.LogInformation(
                string.Format("Services Singleton: {0}", this.OperationService.SingletonOperation.OperationId.ToString()));
            logger.LogInformation(
                string.Format("Services SingletonInstance: {0}", this.OperationService.SingletonInstanceOperation.OperationId.ToString()));
            //
            logger.LogInformation(
                string.Format("Transient: {0}", this.TransientOperation.OperationId.ToString()));
            //logger.LogInformation(
             //   string.Format("Scoped: {0}", this.ScopedOperation.OperationId.ToString()));
            logger.LogInformation(
                string.Format("Singleton: {0}", this.SingletonOperation.OperationId.ToString()));
            logger.LogInformation(
                string.Format("SingletonInstance: {0}", this.SingletonInstanceOperation.OperationId.ToString()));
            return string.Empty;
        }
    }
}