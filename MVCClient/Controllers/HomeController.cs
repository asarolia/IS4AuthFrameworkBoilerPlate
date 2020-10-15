using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Models;
using Newtonsoft.Json.Linq;

namespace MVCClient.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        ////[Authorize]
        ////public IActionResult Privacy()
        ////{
        ////    return View();
        ////}
        //[Authorize]
        //public async Task<IActionResult> LoginAsync()
        //{

        //    //return View();
        //    var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
        //    return View(model);

        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AllowAnonymous]
        public IActionResult Index() => View();

        [AllowAnonymous]
        public IActionResult Register()
        {
            return Redirect("https://localhost:5000/Account/Register?ReturnUrl=https://localhost:5002");
        }

        [Authorize]
        public IActionResult Manage()
        {
            var username = HttpContext.User.Identity.Name.ToString();
            return Redirect("https://localhost:5000/Account/Manage?username="+username+"&ReturnUrl=https://localhost:5002");
        }

        [Authorize]
        public IActionResult Secure() => View();

        public IActionResult Logout() => SignOut("cookie", "oidc");

        public async Task<IActionResult> CallApiAsUser()
        {
            var client = _httpClientFactory.CreateClient("user_client");

            var response = await client.GetStringAsync("https://localhost:5004/api/Test");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }

        public async Task<IActionResult> CallApiAsUserTyped([FromServices] TypedUserClient client)
        {
            var response = await client.CallApi();
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }

        [AllowAnonymous]
        public async Task<IActionResult> CallApiAsClient()
        {
            var client = _httpClientFactory.CreateClient("client");

            var response = await client.GetStringAsync("https://localhost:5004/api/Test");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }

        [AllowAnonymous]
        public async Task<IActionResult> CallApiAsClientTyped([FromServices] TypedClientClient client)
        {
            var response = await client.CallApi();
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }


    }
}
