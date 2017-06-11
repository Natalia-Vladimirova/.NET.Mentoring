using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        private readonly ILogger _logger;
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        public StoreManagerController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: /StoreManager/
        public async Task<ActionResult> Index()
        {
            _logger.Info("Getting albums with genre and artist");

            return View(await _storeContext.Albums
                .Include(a => a.Genre)
                .Include(a => a.Artist)
                .OrderBy(a => a.Price).ToListAsync());
        }

        // GET: /StoreManager/Details/5
        public async Task<ActionResult> Details(int id = 0)
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

        // GET: /StoreManager/Create
        public async Task<ActionResult> Create()
        {
            return await BuildView(null);
        }

        // POST: /StoreManager/Create
        [HttpPost]
        public async Task<ActionResult> Create(Album album)
        {
            if (ModelState.IsValid)
            {
                _storeContext.Albums.Add(album);
                
                await _storeContext.SaveChangesAsync();

                _logger.Info("Create album (id: {0}, user: {1})", album.AlbumId, User.Identity.Name);

                return RedirectToAction("Index");
            }

            _logger.Warn("Errors when creating album (id: {0}, user: {1})", album.AlbumId, User.Identity.Name);

            return await BuildView(album);
        }

        // GET: /StoreManager/Edit/5
        public async Task<ActionResult> Edit(int id = 0)
        {
            var album = await _storeContext.Albums.FindAsync(id);
            if (album == null)
            {
                _logger.Warn("No album was found (id: {0}, user: {1})", id, User.Identity.Name);
                return HttpNotFound();
            }

            _logger.Info("Album was found (id: {0}, title: {1}, user: {2})", id, album.Title, User.Identity.Name);

            return await BuildView(album);
        }

        // POST: /StoreManager/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                _storeContext.Entry(album).State = EntityState.Modified;
                await _storeContext.SaveChangesAsync();

                _logger.Info("Album was updated (id: {0}, title: {1}, user: {2})", album.AlbumId, album.Title, User.Identity.Name);

                return RedirectToAction("Index");
            }

            _logger.Warn("Errors during updating the album (id: {0}, title: {1}, user: {2})", album.AlbumId, album.Title, User.Identity.Name);

            return await BuildView(album);
        }

        // GET: /StoreManager/Delete/5
        public async Task<ActionResult> Delete(int id = 0)
        {
            var album = await _storeContext.Albums.FindAsync(id);
            if (album == null)
            {
                _logger.Warn("No album to delete (id: {0}, user: {1})", id, User.Identity.Name);

                return HttpNotFound();
            }

            return View(album);
        }

        // POST: /StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var album = await _storeContext.Albums.FindAsync(id);
            if (album == null)
            {
                _logger.Warn("No album to delete (id: {0}, user: {1})", id, User.Identity.Name);

                return HttpNotFound();
            }

            _storeContext.Albums.Remove(album);

            await _storeContext.SaveChangesAsync();

            _logger.Info("The album was deleted (id: {0}, user: {1})", id, User.Identity.Name);

            return RedirectToAction("Index");
        }

        private async Task<ActionResult> BuildView(Album album)
        {
            ViewBag.GenreId = new SelectList(
                await _storeContext.Genres.ToListAsync(),
                "GenreId",
                "Name",
                album == null ? null : (object)album.GenreId);

            ViewBag.ArtistId = new SelectList(
                await _storeContext.Artists.ToListAsync(),
                "ArtistId",
                "Name",
                album == null ? null : (object)album.ArtistId);

            return View(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger.Debug("Disposing {0} context in {1} controller", nameof(MusicStoreEntities), nameof(StoreManagerController));
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}