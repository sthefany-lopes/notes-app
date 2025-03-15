using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models;
using Notes.Services.Exceptions;

namespace Notes.Services
{
    public class NoteService
    {
        private readonly NotesContext _context;

        public NoteService(NotesContext context)
        {
            _context = context;
        }

        public async Task<List<Note>> SearchAsync(string? search)
        {
            if (string.IsNullOrEmpty(search))
                return await _context.Note.Include(note => note.Category).ToListAsync();

            search = search.ToLower();
            var query = _context.Note.Include(note => note.Category).AsQueryable().Where(note => note.Title.ToLower().Contains(search));

            return await query.ToListAsync();
        }

        public async Task<Note?> FindByIdAsync(int id)
        {
            return await _context.Note.Include(note => note.Category).FirstOrDefaultAsync(note => note.Id == id);
        }

        public async Task InsertAsync(Note note)
        {
            _context.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note updatedNote)
        {
            if (!(await _context.Note.AnyAsync(note => note.Id == updatedNote.Id)))
                throw new NotFoundException($"The specified note with ID {updatedNote.Id} was not found.");

            try
            {
                _context.Update(updatedNote);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new DbConcurrencyException($"A concurrency conflict occurred while attempting to update the note with ID {updatedNote.Id}. Details: {exception.Message}");
            }
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var note = await _context.Note.FindAsync(id);

                if (note == null)
                    throw new NotFoundException($"The specified note with ID {id} was not found and cannot be removed.");

                _context.Note.Remove(note);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"An error occurred while removing the note. Details: {exception.Message}");
            }
        }
    }
}