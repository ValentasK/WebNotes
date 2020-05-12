using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNotes.Data;
using WebNotes.Models;

namespace WebNotes.Controllers
{
    [EnableCors("MyPolicy")]  // allow cors
    [Route("api/[controller]")]
    [ApiController]
    public class NotesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/NotesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNote()
        {
            return await _context.Note.ToListAsync();
        }

        // GET: api/NotesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _context.Note.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // PUT: api/NotesApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            Note dbNote = _context.Note.Single(n => n.id == id);

            if (dbNote != null)
            {
                dbNote.LastModified = DateTime.Now;
                dbNote.NoteContent = note.NoteContent;
                dbNote.Title = note.Title;
                _context.SaveChanges();
            }

            //return new ObjectResult(dbnote);

            return new OkResult();
        }

        // POST: api/NotesApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note note)
        {
            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.id }, note);
        }

        // DELETE: api/NotesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Note>> DeleteNote(int id)
        {
            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return note;
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.id == id);
        }
    }
}
