using AspNetCoreHero.ToastNotification.Abstractions;
using Infrastructure.Interface;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GearShopWeb.Areas.Admin.Controllers;

[Area("Customer")]
public class AccountController : Controller
{

    #region Readonlys

    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _contxt;
    private readonly IAccountService _accountService;

    #endregion

    public INotyfService _notyfService { get; }

    #region Constructor

    public AccountController(IUnitOfWork unitOfWork,
        INotyfService notyfService,
        ApplicationDbContext db,
        IHttpContextAccessor contxt,
        IAccountService accountService)
    {
        _unitOfWork = unitOfWork;
        _notyfService = notyfService;
        _db = db;
        _contxt = contxt;
        _accountService = accountService;
    }
    #endregion

    // GET: LoginController
    public async Task<IActionResult> Login() => View();
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var account = _accountService.Login(username, password);

        if (account != null)
        {
            _contxt.HttpContext.Session.SetString("User", username);
            _contxt.HttpContext.Session.SetString("Admin", account.Role.RoleName.ToString());
            _contxt.HttpContext.Session.SetString("ID", account.Id.ToString());
            _notyfService.Success("Login Successfully");
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Account does not exist");
            _notyfService.Error("Your information is wrong");
            return View();
        }
    }

    public async Task<IActionResult> SignUp() => View();
    [HttpPost]
    public async Task<IActionResult> SignUp(string name, string username, string email, string password)
    {
        //var account = _accountService.Login(username, password);

        //if (account != null)
        //{
        //    _contxt.HttpContext.Session.SetString("User", username);
        //    _contxt.HttpContext.Session.SetString("Admin", account.Role.RoleName.ToString());
        //    _contxt.HttpContext.Session.SetString("ID", account.Id.ToString());
        //    return RedirectToAction("Index", "Home");
        //}
        //else
        //{
        //    ModelState.AddModelError("", "Account does not exist");
        //    return View();
        //}
        if (ModelState.IsValid)
        {
            _accountService.SignUp(name, username, email, password);
            _notyfService.Success("Sign-up successfully. Now you are able to login !!");
            return RedirectToAction("Login", "Account");
        }
        return View();

    }

    // GET: LoginController1/Details/5
    public async Task<IActionResult> Logout()
    {
        _contxt.HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Details(int? id)
    {
        var user = await _unitOfWork.User.Get(x => x.Id == id);
        return View(user);
    }

    public async Task<IActionResult> MyOrder()
    {
        var userIdString = _contxt.HttpContext.Session.GetString("ID");
        if (!int.TryParse(userIdString, out int userId))
        {
            // Handle the case where the session string is not a valid integer
            Console.WriteLine("Fail");
        }

        var userOrders = await _db.Order
            .Where(x => x.UserID == userId)
            .Include(x => x.User)
            .Include(x => x.OrderDetails)
                .ThenInclude(od => od.Product)
            .ToListAsync();

        return View(userOrders);
    }


}
