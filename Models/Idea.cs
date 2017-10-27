using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; }
        public string IdeaText { get; set; }
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        
        // [ForeignKey("User")]
        public int UserId {get; set;}
        public User User {get; set;}


        public List<Like> Likes {get;set;}
        
        public Idea() {
            Likes = new List<Like>();
          
        }
    }
}