using System;
using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models
{
    public class IdeaViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string IdeaText { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

    }
}