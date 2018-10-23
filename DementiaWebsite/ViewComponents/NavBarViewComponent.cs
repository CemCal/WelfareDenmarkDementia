using System;
using Microsoft.AspNetCore.Mvc;

namespace DementiaWebsite.ViewComponents
{
	public class NavBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(){
            return View();
        }
    }
}
