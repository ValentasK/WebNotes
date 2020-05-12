using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotes.Models
{
    public class Note
    {
        public int id { get; set; }
       [Required]
       [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(800)]
        public string NoteContent { get; set; }
        public DateTime LastModified { get; set; }
        public string UserUniqueName { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
        
    }
}
