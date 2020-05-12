using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotes.Models
{
    public class PublicNote
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string NoteContent { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
    }
}
