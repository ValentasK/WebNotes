using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotes.Models.ViewModels
{
    public class NoteViewModel
    {
        public Note Note { get; set; }
        public bool IsAdmin { get; set; }
        public SharedNote SharedNote { get; set; }

        public List<SelectListItem> Users { get; set; }
        public int SharedNoteId { get; set; }

    }
}
