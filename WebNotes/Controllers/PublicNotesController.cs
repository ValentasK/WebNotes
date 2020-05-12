using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebNotes.Data;
using WebNotes.Models;
using WebNotes.Models.ViewModels;

namespace WebNotes.Controllers
{
    public class PublicNotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicNotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PublicNotes
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.PublicNotes.ToListAsync());
        }

        // GET: PublicNotes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicNote = await _context.PublicNotes
                .FirstOrDefaultAsync(m => m.id == id);
            if (publicNote == null)
            {
                return NotFound();
            }

            return View(publicNote);
        }

        // GET: PublicNotes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PublicNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id,Title,NoteContent,Created,UserName")] PublicNote publicNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publicNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publicNote);
        }

        // GET: PublicNotes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicNote = await _context.PublicNotes.FindAsync(id);
            if (publicNote == null)
            {
                return NotFound();
            }
            return View(publicNote);
        }

        // POST: PublicNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id,Title,NoteContent,Created,UserName")] PublicNote publicNote)
        {
            if (id != publicNote.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publicNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicNoteExists(publicNote.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publicNote);
        }

        // GET: PublicNotes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicNote = await _context.PublicNotes
                .FirstOrDefaultAsync(m => m.id == id);
            if (publicNote == null)
            {
                return NotFound();
            }

            return View(publicNote);
        }

        // POST: PublicNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publicNote = await _context.PublicNotes.FindAsync(id);
            _context.PublicNotes.Remove(publicNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicNoteExists(int id)
        {
            return _context.PublicNotes.Any(e => e.id == id);
        }






        // public view 

        public async Task<IActionResult> PublicIndex(string SearchString)
        {
  

            List<PublicNote> pnotes = new List<PublicNote>();
            pnotes = _context.PublicNotes.ToList();

            if (!String.IsNullOrEmpty(SearchString))
            {
                pnotes = pnotes.Where(n => n.UserName.ToUpper().Contains(SearchString.ToUpper()) || n.Title.ToUpper().Contains(SearchString.ToUpper()) || n.NoteContent.ToUpper().Contains(SearchString.ToUpper())).ToList();
            }

            PublicNotesViewModel pnvm = new PublicNotesViewModel
            {
                PublicNotes = pnotes,
                SearchString = SearchString

            };

            pnvm.PublicNotes.Reverse();

            return View(pnvm);

        }

        public async Task<IActionResult> ViewPublicNote(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicNote = await _context.PublicNotes
                .FirstOrDefaultAsync(m => m.id == id);

            if (publicNote == null)
            {
                return NotFound();
            }

            return View(publicNote);
        }
    }
}
