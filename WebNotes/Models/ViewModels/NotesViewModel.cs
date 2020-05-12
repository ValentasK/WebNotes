using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotes.Models.ViewModels
{
    public class NotesViewModel
    {
        public List<Note> Notes { get; set; }
        public List<SharedNote> SharedNotes { get; set; }
        public bool IsAdmin { get; set; }
        public string SearchString { get; set; }
    }
}
