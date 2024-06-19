using AspNetCoreHero.ToastNotification.Abstractions;
using Domain.Entities;
using GearShopWeb.Areas.Admin.Controllers;
using Infrastructure.Interface.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace GearShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : BaseController
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notyfService { get; }
        public OrderController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _notyfService = notyfService;
        }
        #endregion

        // GET: OrderController
        public ActionResult Index()
        {
            IEnumerable<Order> orders = (IEnumerable<Order>)_unitOfWork.User.GetAll();
            return View(orders);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
