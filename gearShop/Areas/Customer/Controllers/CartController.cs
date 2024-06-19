using AspNetCoreHero.ToastNotification.Abstractions;
using Domain.Entities;
using Infrastructure.Interface;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GearShopWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class CartController : Controller
{
    #region Readonlys

    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _contxt;
    private readonly IAccountService _accountService;
    private readonly IEmailSender _emailSender;
    private readonly IWebHostEnvironment _webHostEnvironment;

    #endregion

    public INotyfService _notyfService { get; }

    #region Constructor

    public CartController(IUnitOfWork unitOfWork,
        INotyfService notyfService,
        ApplicationDbContext db,
        IHttpContextAccessor contxt,
        IAccountService accountService,
        IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _notyfService = notyfService;
        _db = db;
        _contxt = contxt;
        _accountService = accountService;
        _emailSender = emailSender;
        _webHostEnvironment = webHostEnvironment;
    }
    #endregion

    public IActionResult Index()
    {
        var cartSession = _contxt.HttpContext.Session.GetString("CartSession");
        var list = new List<CartItem>();
        if (cartSession != null)
        {
            list = JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
        }
        return View(list);
    }
    public IActionResult AddItem(int productID, int quantity, string ctl)
    {
        var product = _db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == productID);
        var cartSession = _contxt.HttpContext.Session.GetString("CartSession");

        var list = cartSession != null ? JsonConvert.DeserializeObject<List<CartItem>>(cartSession) : new List<CartItem>();

        if (list.Any(x => x.product.Id == productID))
        {
            foreach (var item in list)
            {
                if (item.product.Id == productID) item.Quantity += quantity;
            }
        }
        else
        {
            list.Add(new CartItem { product = product, Quantity = quantity });
        }

        _contxt.HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(list));
        _notyfService.Success("Added Successfully");
        return RedirectToAction("Index", "Product");
    }
    public JsonResult Update(string cartModel)
    {
        var cartJson = JsonConvert.DeserializeObject<List<CartItem>>(cartModel);
        var cartSession = _contxt.HttpContext.Session.GetString("CartSession");
        var list = string.IsNullOrEmpty(cartSession) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartSession);

        foreach (var item in list)
        {
            var jsonItem = cartJson.SingleOrDefault(x => x.product.Id == item.product.Id);
            if (jsonItem != null)
            {
                item.Quantity = jsonItem.Quantity;
            }
        }

        _contxt.HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(list));
        return Json(new
        {
            status = true
        });

    }
    public JsonResult RemoveAll()
    {
        _contxt.HttpContext.Session.Remove("CartSession");
        return Json(new
        {
            status = true
        });
    }
    public JsonResult Remove(int id)
    {
        var cartSession = _contxt.HttpContext.Session.GetString("CartSession");
        var list = string.IsNullOrEmpty(cartSession) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartSession);

        list.RemoveAll(x => x.product.Id == id);
        _contxt.HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(list));

        return Json(new
        {
            status = true
        });

    }

    public IActionResult CheckOut()
    {
        if (_contxt.HttpContext.Session.GetString("User") == null)
        {
            return RedirectToAction("Index", "Account");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(string firstName, string lastName, string ShipAddress, string Email, string PhoneNumber)
    {
        var cartSession = _contxt.HttpContext.Session.GetString("CartSession");
        var userId = _contxt.HttpContext.Session.GetString("ID");
        int id;

        if (!int.TryParse(userId, out id))
        {
            // Handle the case where the session string is not a valid integer
            Console.WriteLine("Fail");
        }

        var list = cartSession != null ? JsonConvert.DeserializeObject<List<CartItem>>(cartSession) : new List<CartItem>();

        Order od = new Order
        {
            CreateDate = DateTime.UtcNow,
            FirstName = firstName,
            LastName = lastName,
            ShipAddress = ShipAddress,
            Email = Email,
            PhoneNumber = PhoneNumber,
            UserID = id
        };

        _db.Order.Add(od);
        await _db.SaveChangesAsync();
        double unitPrice = 0.0;
        double totalPrice = 0.0;
        string style = @"style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-                                  align:left;vertical-align:middle;font-family
                       :'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word""";

        foreach (var item in list)
        {
            var price = item.product.Price * item.Quantity;
            OrderDetail orderDetail = new OrderDetail
            {
                ProductID = item.product.Id,
                Quantity = item.Quantity,
                OrderID = od.Id,
                UnitPrice = item.product.Price,
                TotalPrice = price
            };

            _db.OrderDetail.Add(orderDetail);

            unitPrice += price;
            totalPrice += price;
        }

        string templatePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "template", "send2.html");
        var templateContent = System.IO.File.ReadAllText(templatePath);
        var strProduct = "";

        foreach (var it in list)
        {
            var price = it.product.Price * it.Quantity;

            strProduct += "<tr>";
            strProduct += $"<td {style}>{it.product.Name}</td>";
            strProduct += $"<td {style}>{it.Quantity}</td>";
            strProduct += $"<td {style}>{price}</td>";
            strProduct += "</tr>";
        }

        templateContent = templateContent.Replace("{{name}}", od.FirstName)
                                         .Replace("{{id}}", od.Id.ToString())
                                         .Replace("{{date}}", od.CreateDate.ToString("dddd, dd MMMM yyyy"))
                                         .Replace("{{unitPrice}}", unitPrice.ToString("C"))
                                         .Replace("{{totalPrice}}", totalPrice.ToString("C"))
                                         .Replace("{{address}}", od.ShipAddress)
                                         .Replace("{{phone}}", od.PhoneNumber)
                                         .Replace("{{email}}", od.Email)
                                         .Replace("{{product}}", strProduct);

        await _emailSender.SendEmailAsync(Email, "Order Successfully", templateContent);
        await _db.SaveChangesAsync();
        return RedirectToAction("Success");
    }



    public async Task<IActionResult> Success()
    {

        var userId = _contxt.HttpContext.Session.GetString("ID");

        if (!int.TryParse(userId, out int id))
        {
            Console.WriteLine("Fail");
        }
        var order = await _db.Order
                     .AsNoTracking()
                     .Include(x => x.OrderDetails)
                         .ThenInclude(od => od.Product)
                     .Where(x => x.UserID == id)
                     .OrderByDescending(x => x.Id)
                     .FirstOrDefaultAsync();


        _contxt.HttpContext.Session.Remove("CartSession");
        return View(order);
    }


}
