using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models;
using Notes.Services.Exceptions;

namespace Notes.Services
{
    public class CategoryService
    {
        private readonly NotesContext _context;

        public CategoryService(NotesContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> FindAllAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<List<Category>> SearchAsync(string? search)
        {
            if (string.IsNullOrEmpty(search))
                return await _context.Category.ToListAsync();

            search = search.ToLower();
            var query = _context.Category.AsQueryable().Where(category => category.Name.ToLower().Contains(search));

            return await query.ToListAsync();
        }

        public async Task<Category?> FindByIdAsync(int id)
        {
            return await _context.Category.FirstOrDefaultAsync(category => category.Id == id);
        }

        public async Task<Category?> FindByNameAsync(string name)
        {
            return await _context.Category.FirstOrDefaultAsync(category => category.Name.ToLower() == name.ToLower());
        }

        public async Task InsertAsync(Category category)
        {
            if (await FindByNameAsync(category.Name) != null)
                return;

            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category updatedCategory)
        {
            if (!(await _context.Category.AnyAsync(category => category.Id == updatedCategory.Id)))
                throw new NotFoundException($"The specified category with ID {updatedCategory.Id} was not found.");

            var existingCategory = await FindByNameAsync(updatedCategory.Name);

            if (existingCategory != null && existingCategory.Id != updatedCategory.Id)
                return;

            var trackedCategory = await _context.Category.FindAsync(updatedCategory.Id);

            try
            {
                if (trackedCategory != null)
                    _context.Entry(trackedCategory).CurrentValues.SetValues(updatedCategory);
                else
                    _context.Update(updatedCategory);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new DbConcurrencyException($"A concurrency conflict occurred while attempting to update the category with ID {updatedCategory.Id}. Details: {exception.Message}");
            }
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var category = await _context.Category.FindAsync(id);

                if (category == null)
                    throw new NotFoundException($"The specified category with ID {id} was not found and cannot be removed.");

                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"An error occurred while removing the category. Details: {exception.Message}");
            }
        }
    }
}