using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Helpers
{
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            _logger.LogInformation("----STARTING----" + actionName);

            foreach (var p in context.ActionArguments)
                _logger.LogInformation($"{p.Key} ::: {p.Value}");

            _logger.LogInformation(string.Concat("----ENDING----", actionName));
            await next();
        }
    }
}
