using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger _logger;
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        public StoreController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: /Store/
        public async Task<ActionResult> Index()
        {
            _logger.Info("Getting genres");
            return View(await _storeContext.Genres.ToListAsync());
        }

        // GET: /Store/Browse?genre=Disco
        public async Task<ActionResult> Browse(string genre)
        {
            _logger.Info("Searching genre {0}", genre);

            return View(await _storeContext.Genres.Include("Albums").SingleAsync(g => g.Name == genre));
        }

        public async Task<ActionResult> Details(int id)
        {
            _logger.Info("Getting album details id: {0}", id);

            var album = await _storeContext.Albums.FindAsync(id);

            if (album == null)
            {
                _logger.Warn("No album with id {0}", id);
                return HttpNotFound();
            }

            _logger.Info("Album has been found (id: {0}, title: {1})", album.AlbumId, album.Title);
            return View(album);
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            return PartialView(
                _storeContext.Genres.OrderByDescending(
                    g => g.Albums.Sum(a => a.OrderDetails.Sum(od => od.Quantity))).Take(9).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger.Debug("Disposing {0} context in {1} controller", nameof(MusicStoreEntities), nameof(StoreController));
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}