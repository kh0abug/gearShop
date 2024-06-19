using Application.Common.ExtensionMethods;
using AspNetCoreHero.ToastNotification.Abstractions;
using Domain.Entities;
using GearShopWeb.Areas.Admin.Controllers;
using Infrastructure.Interface.IRepository;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _db;
    public INotyfService _notyfService { get; }
    public CategoryController(IUnitOfWork unitOfWork, INotyfService notyfService, ApplicationDbContext db)
    {
        _unitOfWork = unitOfWork;
        _notyfService = notyfService;
        _db = db;
    }
    public async Task<IActionResult> Index(int? pageNumber)
    {
        var paginatedList = await _db.Categories.PaginatedListAsync(pageNumber ?? 1, 4);
        return View(paginatedList);
    }

    public async Task<IActionResult> CreateAndUpdate(int? id)
    {
        if (id == null || id == 0)
        {
            Category c = new Category();
            return View(c);
        }
        else
        {
            Category c = await _unitOfWork.Category.Get(u => u.Id == id);
            return View(c);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAndUpdate(Category obj)
    {

        if (ModelState.IsValid)
        {
            //Create new
            if (obj.Id == 0)
            {
                await _unitOfWork.Category.Add(obj);
                _notyfService.Success("Category Added Successfully");
            }
            else
            {
                _unitOfWork.Category.Update(obj);
                _notyfService.Success("Category Updated Successfully");
            }
            await _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int? id)
    {
        Category? category = await _unitOfWork.Category.Get(x => x.Id == id);
        if (id == null || id == 0)
        {
            return NotFound();
        }
        await _unitOfWork.Category.Remove(category);
        await _unitOfWork.Save();
        _notyfService.Success("Deleted!");
        return RedirectToAction("Index");
    }



}
