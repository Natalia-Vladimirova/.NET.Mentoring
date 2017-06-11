using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: /Home/
        public async Task<ActionResult> Index()
        {
            _logger.Info("Getting albums");

            return View(await _storeContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(6)
                .ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger.Debug("Disposing {0} context in {1} controller", nameof(MusicStoreEntities), nameof(HomeController));
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}