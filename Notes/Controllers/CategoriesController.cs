using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models;
using Notes.Models.ViewModels;
using Notes.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? search = "")
        {
            const int maxSearchLength = 50;

            if (search != null && search.Length > maxSearchLength)
                return RedirectToAction(nameof(Error), new { message = $"The search term for the category title cannot exceed {maxSearchLength} characters." });

            return View(await _categoryService.SearchAsync(search));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "The category ID was not provided." });

            var category = await _categoryService.FindByIdAsync(id.Value);

            if (category == null)
                return RedirectToAction(nameof(Error), new { message = $"The specified category with ID {id} was not found." });

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _categoryService.InsertAsync(category);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "The category ID was not provided." });

            var category = await _categoryService.FindByIdAsync(id.Value);

            if (category == null)
                return RedirectToAction(nameof(Error), new { message = $"The specified category with ID {id} was not found." });

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
                return RedirectToAction(nameof(Error), new { message = "The provided category ID does not match the ID of the category being edited." });

            if (!ModelState.IsValid)
                return View(category);
           
            try
            {
                await _categoryService.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException exception)
            {
                return RedirectToAction(nameof(Error), new { message = exception.Message });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "The category ID was not provided." });

            var category = await _categoryService.FindByIdAsync(id.Value);

            if (category == null)
                return RedirectToAction(nameof(Error), new { message = $"The specified category with ID {id} was not found." });

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categoryService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException exception)
            {
                return RedirectToAction(nameof(Error), new { message = exception.Message });
            }
        }

        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier, message));
        }
    }
}