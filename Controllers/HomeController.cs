using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using aspnetcore_backgroundservice.Models;
using System.Threading.Channels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using aspnetcore_backgroundservice.Data;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_backgroundservice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChannelWriter<UploadModel> _channel;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger,
                              ChannelWriter<UploadModel> channel,
                              ApplicationDbContext context)
        {
            _logger = logger;
            _channel = channel;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile upload)
        {
            var ms = new MemoryStream();
            await upload.CopyToAsync(ms);
            var uploadItem = new UploadModel()
            {
                Data = ms.ToArray()
            };
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMilliseconds(5));
            var uploadTask = _channel.WriteAsync(uploadItem, source.Token);
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var model = await _context.StoreItems.ToListAsync();
            ViewData["Worth"] = (await _context.Stores.FirstOrDefaultAsync())?.StoreWorth;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PrivacyPost()
        {
            var rand = new Random();
            foreach (var item in _context.StoreItems.AsEnumerable())
            {
                item.Stock = rand.Next(1, 100);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
