using Application.Common.ExtensionMethods;
using AspNetCoreHero.ToastNotification.Abstractions;
using Domain.Entities;
using GearShopWeb.Areas.Admin.Controllers;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;

namespace GearShop.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : BaseController
{
    #region Readonlys

    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    #endregion

    public INotyfService _notyfService { get; }

    #region Constructor

    public ProductController(IUnitOfWork unitOfWork,
        IWebHostEnvironment webHostEnvironment,
        INotyfService notyfService,
        ApplicationDbContext db)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        _notyfService = notyfService;
        _db = db;
    }

    #endregion

    public async Task<IActionResult> Index(int? pageNumber)
    {
        var productList = await _db.Products.Include(x => x.Category)
                                            .PaginatedListAsync(pageNumber ?? 1, 4);
        return View(productList);
    }
    public async Task<IActionResult> CreateAndUpdate(int? id)
    {
        ProductVM productVM = new()
        {
            CategoryList = await _db.Categories.AsNoTracking()
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }).ToListAsync(),
            product = new Product()
        };
        if (id == null || id == 0)
        {
            // create 
            return View(productVM);
        }
        else
        {
            //update
            productVM.product = await _unitOfWork.Product.Get(u => u.Id == id);
            return View(productVM);
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateAndUpdate(ProductVM productVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            String wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");
                if (!string.IsNullOrEmpty(productVM.product.ImgUrl))
                {
                    //Delete old image
                    var oldImage = Path.Combine(wwwRootPath, productVM.product.ImgUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                productVM.product.ImgUrl = @"\images\product\" + fileName;
                //productVM.Product.ImgUrl = fileName;
            }

            if (productVM.product.Id == 0)
            {
                await _unitOfWork.Product.Add(productVM.product);
                _notyfService.Success("Product Added Successfully");
            }
            else
            {
                await _unitOfWork.Product.Update(productVM.product);
                _notyfService.Success("Product Updated Successfully");
            }

            await _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        else
        {
            productVM.CategoryList = _db.Categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int? id)
    {
        Product? product = await _unitOfWork.Product.Get(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        await _unitOfWork.Product.Remove(product);
        await _unitOfWork.Save();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }

}
