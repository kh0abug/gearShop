using Application.Pagination;
using Domain.Entities;
using GearShopWeb.ViewModels;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GearShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        #region ReadOnly
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        #endregion

        public ProductController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        // GET: ProductController
        public async Task<ActionResult> Index(int? pageNumber, int CatID = 0)
        {
            ViewBag.SelectList = _db.Categories.AsNoTracking().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            PaginatedList<HomeProductVM> productList;

            if (CatID != 0)
            {
                var query = _db.Products.AsNoTracking()
                    .Where(x => x.CategoryId == CatID)
                    .Select(x => new HomeProductVM
                    {
                        Name = x.Name,
                        Price = x.Price,
                        ImgUrl = x.ImgUrl,
                        Id = x.Id,
                        category = _db.Categories.AsNoTracking().ToList()
                    })
                    .OrderByDescending(x => x.Id);

                productList = await PaginatedList<HomeProductVM>.CreateAsync(query, pageNumber ?? 1, 4);
            }
            else
            {
                var query = _db.Products.AsNoTracking()
                    .Select(x => new HomeProductVM
                    {
                        Name = x.Name,
                        Price = x.Price,
                        ImgUrl = x.ImgUrl,
                        Id = x.Id,
                        category = _db.Categories.AsNoTracking().ToList()
                    })
                    .OrderByDescending(x => x.Id);

                productList = await PaginatedList<HomeProductVM>.CreateAsync(query, pageNumber ?? 1, 4);
            }
            //var productList = await _db.Products.AsNoTracking()
            //   .Select(x => new HomeProductVM { Name = x.Name, Price = x.Price, ImgUrl = x.ImgUrl, Id = x.Id })
            //   .PaginatedListAsync(pageNumber ?? 1, 6);
            return View(productList); // Pass 'productList' to the view
        }



        public ActionResult Filter(int CatID = 0)
        {
            var url = $"/Customer/Product/Index?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Customer/Product";
            }
            //var products = _unitOfWork.Product.GetAll();
            //return View(products);
            return Json(new
            {
                status = "success",
                redirectUrl = url
            });
        }

        public async Task<ActionResult> Details(int id)
        {
            Product product = await _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
            return View(product);
        }
    }
}
