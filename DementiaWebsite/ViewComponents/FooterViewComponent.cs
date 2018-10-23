using System;
using Microsoft.AspNetCore.Mvc;

namespace DementiaWebsite.ViewComponents
{
	public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(){

            return View();
        }
    }
}
