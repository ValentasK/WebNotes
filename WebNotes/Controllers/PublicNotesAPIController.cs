using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNotes.Data;
using WebNotes.Models;

namespace WebNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicNotesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PublicNotesAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PublicNotesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicNote>>> GetPublicNotes()
        {
            return await _context.PublicNotes.ToListAsync();
        }

        // GET: api/PublicNotesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicNote>> GetPublicNote(int id)
        {
            var publicNote = await _context.PublicNotes.FindAsync(id);

            if (publicNote == null)
            {
                return NotFound();
            }

            return publicNote;
        }

        // PUT: api/PublicNotesAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublicNote(int id)
        {
            var note = await _context.Note.FindAsync(id); // get note object with received id

            PublicNote pn = new PublicNote // create new shared note and copy details 
            {
                Title = note.Title, 
                NoteContent = note.NoteContent,
                Created = DateTime.Now,
                UserName = note.UserName
            };

            _context.PublicNotes.Add(pn);        // add shared note to db
            await _context.SaveChangesAsync();   // save changes 


            return new ObjectResult(pn.id);  // send back new shared note id 
        }

        // POST: api/PublicNotesAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PublicNote>> PostPublicNote(PublicNote publicNote)
        {
            _context.PublicNotes.Add(publicNote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublicNote", new { id = publicNote.id }, publicNote);
        }

        // DELETE: api/PublicNotesAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PublicNote>> DeletePublicNote(int id)
        {
            var publicNote = await _context.PublicNotes.FindAsync(id);
            if (publicNote == null)
            {
                return NotFound();
            }

            _context.PublicNotes.Remove(publicNote);
            await _context.SaveChangesAsync();

            return publicNote;
        }

        private bool PublicNoteExists(int id)
        {
            return _context.PublicNotes.Any(e => e.id == id);
        }
    }
}
