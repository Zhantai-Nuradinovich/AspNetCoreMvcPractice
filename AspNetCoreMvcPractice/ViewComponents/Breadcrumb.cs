using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.ViewComponents
{
    public class Breadcrumb : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var controller = ViewContext.RouteData.Values["controller"].ToString();
            var action = ViewContext.RouteData.Values["action"].ToString();
            var data = new BreadcrumbData() { Controller = controller, Action = action };
            return View(data);
        }
    }

    public class BreadcrumbData
    {
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
