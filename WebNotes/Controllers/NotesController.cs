using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebNotes.Data;
using WebNotes.Models;
using WebNotes.Models.ViewModels;


namespace WebNotes.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private bool isAdmin;
        private string user;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
 
        }

        // GET: Notes
        [Authorize]
        public async Task<IActionResult> Index(string SearchString)
        {
            List<Note> notes; // create list of notes 
            List<SharedNote> sharedNotes; // create list of sharednotes 
            user = User.FindFirst(ClaimTypes.NameIdentifier).Value; // get unique user id

            isAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == user) != null;


           // var ids = _context.SharedNotes.Where(sn => sn.UserUniqueName == user).ToList(); to get all shared notes

            var ids = _context.SharedNotes.Where(sn => sn.UserUniqueName == user).Select( sn => sn.Id).ToList();

            //sharedNotes = _context.Note.Where(n => ids.Contains(n.id)).ToList();

            sharedNotes = _context.SharedNotes.Where(n => ids.Contains(n.Id)).ToList();

            if (isAdmin)
            {
                notes = _context.Note.ToList();
            }
            else
            {
                notes = _context.Note.Where(m => m.UserUniqueName == user).ToList(); // Take all notes only with userunique name
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                notes = notes.Where(n => n.UserName.ToUpper().Contains(SearchString.ToUpper()) || n.Title.ToUpper().Contains(SearchString.ToUpper()) || n.NoteContent.ToUpper().Contains(SearchString.ToUpper())).ToList();
            }

            NotesViewModel nsvm = new NotesViewModel
            {
                Notes=notes,
                IsAdmin = isAdmin,
                SharedNotes = sharedNotes
            };

            nsvm.Notes.Reverse();
            nsvm.SharedNotes.Reverse();

            return View(nsvm);

        }

        // GET: Notes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.id == id);
            if (note == null)
            {
                return NotFound();
            }

            user = User.FindFirst(ClaimTypes.NameIdentifier).Value; // get unique user id
            isAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == user) != null; // check if admin is loged in 

            NoteViewModel nvm = new NoteViewModel
            {
                Note = note,
                IsAdmin = isAdmin
            };

            return View(nvm);
        }

        // GET: Notes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id,Title,NoteContent")] Note note)
        {
            if (ModelState.IsValid)
            {
                note.UserName = User.Identity.Name; // get user name

                note.Created = DateTime.Now; // get time once note was created
                note.LastModified = DateTime.Now; // get time once note was created
                note.UserUniqueName = User.FindFirst(ClaimTypes.NameIdentifier).Value; // get unique user id
                _context.Add(note); 
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(note);
        }


        // POST: SharedNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateShared([Bind("Id,UserUniqueName")] SharedNote sharedNote , int SharedNoteId)
        {
            Note note = _context.Note.Single(x => x.id == SharedNoteId);

            sharedNote.TimeNoteSent = DateTime.Now;
            sharedNote.Title = note.Title;
            sharedNote.NoteContent = note.NoteContent;
            sharedNote.SenderUserUniqueName = note.UserUniqueName;
            sharedNote.UserName = note.UserName;



            if (ModelState.IsValid)
            {
                _context.Add(sharedNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sharedNote);
        }



        // POST: Notes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Deleteshared")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deleteshared(int Id)
        {
            var SharedNote = await _context.SharedNotes.FindAsync(Id);
            _context.SharedNotes.Remove(SharedNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }







        // GET: Notes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            List<IdentityUser> userNames = _context.Users.ToList();
            
            NoteViewModel nvm = new NoteViewModel();

             nvm.Users = userNames.Select(p => new SelectListItem
             {
                Text = p.UserName,
                Value = p.Id 
            }).ToList();


            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            nvm.Note = note;
            return View(nvm);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id,Title,NoteContent,Created,UserUniqueName,UserName")] Note note)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    note.LastModified = DateTime.Now;
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.id))
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
            return View(note);
        }


        // GET: Notes/EditShared/5
        [Authorize]
        public async Task<IActionResult> ViewSharedNote(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sharednote = await _context.SharedNotes.FindAsync(id);
           

            NoteViewModel nvm = new NoteViewModel();
            nvm.SharedNote = sharednote;
            

            if (sharednote == null)
            {
                return NotFound();
            }
            return View(nvm);
        }

        // GET: Notes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.id == id);
        }
    }
}
