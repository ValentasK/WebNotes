using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotes.Models.ViewModels
{
    public class PublicNotesViewModel
    {
        public List<PublicNote> PublicNotes { get; set; }
        public string SearchString { get; set; }
        public bool IsAdmin { get; set; }
    }
}
