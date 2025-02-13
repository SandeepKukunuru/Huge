﻿
using Huge.Models;
using Huge.DataAccess.Data;

using Microsoft.AspNetCore.Mvc;
using Huge.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Huge.Models.ViewModels;

namespace Huge.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; //replacing DbContext with ProductRepository and we are asking DI to provide that implementation


        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
          
            return View(objProductList);

        }
        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem

                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),
                Product = new Product()

            };
            return View(productVM);
               


        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {


                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem

                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                });
                return View(productVM);
            }
            

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productfromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productfromDb == null)
            {
                return NotFound();
            }
            return View(productfromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {


                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productfromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productfromDb == null)
            {
                return NotFound();
            }
            return View(productfromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");

        }


    }
}
