using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Helpers
{
    public class LogActionAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var logger = (ILogger<LogActionFilter>)serviceProvider.GetService(typeof(ILogger<LogActionFilter>));
            return new LogActionFilter(logger);
        }
    }
}
