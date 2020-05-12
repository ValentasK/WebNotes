using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotes.Models
{
    public class SharedNote
    {
        public int Id { get; set; }
        public string UserUniqueName { get; set; }
        public DateTime TimeNoteSent { get; set; }
        public string Title { get; set; }
        public string NoteContent { get; set; }
        public string SenderUserUniqueName { get; set; }
        public string UserName { get; set; }
    }
}
