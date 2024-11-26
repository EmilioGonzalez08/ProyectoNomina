using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Nomina_Web.Configuration;
using Sistema_Nomina_Web.Models;
using Sistema_Nomina_Web.Models.dbModels;
using Sistema_Nomina_Web.Services;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Sistema_Nomina_Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contactanos()
        {
            return View();
        }

        public IActionResult Trabajador()
        {
            return View();
        }

        public IActionResult Nomina()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // POST: Home/Contactanos
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contactanos([Bind("EmailToId, EmailToName", "EmailSubject, EmailBody")] MailData mailData)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var client = new SmtpClient("live.smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("api", "12b13fc69f71dafa6fca43c44c55439f"),
                EnableSsl = true
            };
            client.Send("hello@demomailtrap.com", mailData.EmailToId, mailData.EmailSubject, mailData.EmailBody);

            return RedirectToAction(nameof(Index));
        }
    }
}