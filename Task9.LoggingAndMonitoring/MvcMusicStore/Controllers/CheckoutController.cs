using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Infrastructure;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ILogger _logger;
        private readonly CounterInstance _counterInstance;
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        private const string PromoCode = "FREE";
        
        public CheckoutController(ILogger logger, CounterInstance counterInstance)
        {
            _logger = logger;
            _counterInstance = counterInstance;
        }

        // GET: /Checkout/
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public async Task<ActionResult> AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            if (ModelState.IsValid 
                && string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase))
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                _storeContext.Orders.Add(order);

                await ShoppingCart.GetCart(_storeContext, this).CreateOrder(order);

                await _storeContext.SaveChangesAsync();

                _logger.Info("AddressAndPayment success (order id: {0})", order.OrderId);

                return RedirectToAction("Complete", new { id = order.OrderId });
            }

            _logger.Warn("AddressAndPayment errors (order id: {0})", order.OrderId);

            return View(order);
        }

        // GET: /Checkout/Complete
        public async Task<ActionResult> Complete(int id)
        {
            bool success = await _storeContext.Orders.AnyAsync(o => o.OrderId == id && o.Username == User.Identity.Name);

            if (success)
            {
                _logger.Info("Checkout has been completed (order id: {0}, user: {1})", id, User.Identity.Name);

                _counterInstance.CounterHelper?.Increment(PerformanceCounters.SuccessfulCheckout);

                return View(id);
            }

            _logger.Warn("Checkout errors (order id: {0}, user: {1})", id, User.Identity.Name);

            return View("Error");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger.Debug("Disposing {0} context in {1} controller", nameof(MusicStoreEntities), nameof(CheckoutController));
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}