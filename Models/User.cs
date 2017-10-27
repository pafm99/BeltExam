using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public abstract class BaseEntity {}
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}

        public List<Idea> Ideas { get; set;}
        public List<Like> Likes {get;set;}
        public User() {
            Ideas = new List<Idea>();
            Likes = new List<Like>();
        }
    }
}