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
    public class NotesController : Controller
    {
        private readonly NoteService _noteService;
        private readonly CategoryService _categoryService;

        public NotesController(NoteService noteService, CategoryService categoryService)
        {
            _noteService = noteService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? search = "")
        {
            const int maxSearchLength = 50;

            if (search != null && search.Length > maxSearchLength)
                return RedirectToAction(nameof(Error), new { message = $"The search term for the note title cannot exceed {maxSearchLength} characters." });

            return View(await _noteService.SearchAsync(search));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "The note ID was not provided." });

            var note = await _noteService.FindByIdAsync(id.Value);

            if (note == null)
                return RedirectToAction(nameof(Error), new { message = $"The specified note with ID {id} was not found." });

            return View(note);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.FindAllAsync();

            var categoryList = new List<SelectListItem>()
            {
                new SelectListItem() { Value = null, Text = "No Category" }
            };

            categoryList.AddRange(categories.Select(category => new SelectListItem()
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }));

            ViewBag.CategoryId = new SelectList(categoryList, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId")] Note note)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.FindAllAsync();

                var categoryList = new List<SelectListItem>();
                {
                    new SelectListItem { Value = null, Text = "No Category" };
                };

                categoryList.AddRange(categories.Select(category => new SelectListItem()
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                }));

                ViewBag.CategoryId = new SelectList(categoryList, "Value", "Text");

                return View(note);
            }

            await _noteService.InsertAsync(note);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "The note ID was not provided." });

            var note = await _noteService.FindByIdAsync(id.Value);

            if (note == null)
                return RedirectToAction(nameof(Error), new { message = $"The specified note with ID {id} was not found." });

            var categories = await _categoryService.FindAllAsync();

            var categoryList = new List<SelectListItem>()
            {
                new SelectListItem { Value = null, Text = "No Category" }
            };

            categoryList.AddRange(categories.Select(category => new SelectListItem()
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }));

            ViewBag.CategoryId = new SelectList(categoryList, "Value", "Text");

            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CategoryId")] Note note)
        {
            if (id != note.Id)
                return RedirectToAction(nameof(Error), new { message = "The provided note ID does not match the ID of the note being edited." });

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.FindAllAsync();

                var categoryList = new List<SelectListItem>()
                {
                    new SelectListItem { Value = null, Text = "No Category" }
                };

                categoryList.AddRange(categories.Select(category => new SelectListItem()
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                }));

                ViewBag.CategoryId = new SelectList(categoryList, "Value", "Text");
                return View(note);
            }

            try
            {
                await _noteService.UpdateAsync(note);
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
                return RedirectToAction(nameof(Error), new { message = "The note ID was not provided." });

            var note = await _noteService.FindByIdAsync(id.Value);

            if (note == null)
                return RedirectToAction(nameof(Error), new { message = $"The specified note with ID {id} was not found." });

            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _noteService.RemoveAsync(id);
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