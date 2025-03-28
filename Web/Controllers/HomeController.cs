using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        
        var settings = new MySettings();
        ViewData["Title"]=settings.AppInfo.ToString();

        ViewData["GithubUrl"] = Constants.SystemConstants.GithubProjectUrl;                                          ;
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
