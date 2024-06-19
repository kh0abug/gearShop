using Application.Common.ExtensionMethods;
using AspNetCoreHero.ToastNotification.Abstractions;
using GearShopWeb.Areas.Admin.Controllers;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GearShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController
    {
        #region DI
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public INotyfService _notyfService { get; }
        public UserController(IUnitOfWork unitOfWork,
            INotyfService notyfService,
            ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _notyfService = notyfService;
            _db = db;
        }
        #endregion
        // GET: UserController1
        public async Task<ActionResult> Index(int? pageNumber)
        {
            var users = await _db.Users.Include(x => x.Role)
                                       .Where(x => x.Role.IsAdmin == false)
                                       .PaginatedListAsync(pageNumber ?? 1, 4);
            return View(users);
        }

        // GET: UserController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController1/Create
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

        // GET: UserController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController1/Edit/5
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

        // GET: UserController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController1/Delete/5
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
