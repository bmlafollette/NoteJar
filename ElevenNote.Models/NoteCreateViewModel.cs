using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElevenNote.Models
{

    public class NoteCreateViewModel
    {
        // This property will hold a state, selected by user
        [Required]
        [Display(Name = "Importance")]
        public string Importance { get; set; }

        // This property will hold all available states for selection
        public IEnumerable<SelectListItem> ImportanceLevels { get; set; }

        [Required]
        [MinLength(2, ErrorMessage ="Please enter at least two characters.")]
        [MaxLength(128)]

        public string Title { get; set; }

        //[Required]
        //[MinLength(6, ErrorMessage = "Please type Urgent, Normal, or Not Urgent.")]
        //[MaxLength(128)]
        //
        //public string Importance { get; set; }

        [Required]
        [MaxLength(8000)]

        public string Content { get; set; }

    }
}
