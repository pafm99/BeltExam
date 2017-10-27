using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BeltExam.Models;
using System.Linq;
using System.Collections.Generic;
using System;
//using Microsoft.Extensions.Logging;

namespace BeltExam.Controllers
{
    public class DashboardController : Controller
    {
    private IdeaContext _context;
 
    public DashboardController(IdeaContext context)
    {
        _context = context;
    }
        // GET: /Home/
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.LoggedIn = new List<string>();
            ViewBag.AllIdeas = new List<string>();
            ViewBag.AllUsers = new List<string>();
           
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if (SessionId == null){
                TempData["error"] = "Must be logged in to view this page";
                return RedirectToAction("Index", "Home");
            }
 
            List<Idea> AllIdeas = _context.Ideas.ToList();
            ViewBag.AllIdeas = AllIdeas;

            List<User> AllUsers = _context.Users.ToList(); 

            User LoggedIn = _context.Users.SingleOrDefault(user => user.UserId == SessionId);
            ViewBag.LoggedIn = LoggedIn;

            // List<Idea> MyIdeas = _context.Ideas.Where(idea => idea.IdeaId == SessionId).Include(user => user.Ideas).ToList();
            // ViewBag.MyIdeas = MyIdeas;

            List<Idea> MyIdeas = _context.Ideas.Where(idea => idea.UserId == SessionId).Include(user => user.User).ToList();
            ViewBag.MyIdeas = MyIdeas;

            List<Idea> AllLikedIdeas = _context.Ideas.Include(l => l.Likes).ToList();
            ViewBag.AllLikedIdeas = AllLikedIdeas;            

            return View();
        }


        [HttpPost]
        [Route("Process")]
        public IActionResult Process(IdeaViewModel model)
        {
            if (ModelState.IsValid)
            {
                int? CurrStudentId = HttpContext.Session.GetInt32("UserId");
                Idea NewIdea = new Idea{
                    IdeaText = model.IdeaText,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = (int)CurrStudentId                  
                };
                    _context.Ideas.Add(NewIdea);
                    _context.SaveChanges();
                    int IdeaId = NewIdea.IdeaId;
                    return Redirect("Index");
            } else {
                ViewBag.errors = ModelState.Values;
                return View("Index");                
            }
        }
        [HttpGet]
        [Route("Display/{Id}")]
        public IActionResult Display(int id)
        {
            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if (SessionId == null){
                TempData["error"] = "Must be logged in to view this page";
                return RedirectToAction("Index", "Home");
            }            
            
            Idea currIdea = _context.Ideas.Include(s => s.User).SingleOrDefault(a => a.IdeaId == id);
            ViewBag.currIdea = currIdea;
 
    
            return View("Display"); 
        }            

        [HttpGet]
        [Route("Add/{Id}")]
        public IActionResult Add(int Id, DateTime DateLiked) {

            ViewBag.LoggedIn = new List<string>();

            List<Idea> AllIdeas = _context.Ideas.ToList();
            ViewBag.AllIdeas = AllIdeas; 
            
            Idea currIdea = _context.Ideas.Include(s => s.User).SingleOrDefault(a => a.IdeaId == Id);
            ViewBag.currIdea = currIdea;

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            User LoggedIn = _context.Users.SingleOrDefault(user => user.UserId == SessionId);
            ViewBag.LoggedIn = LoggedIn;

            int? uId = HttpContext.Session.GetInt32("UserId");
            if (uId == null) { 
                return RedirectToAction("Index", "Home");
            }
           
            Like newLike = new Like {
                IdeaId = Id,
                UserId = (int)uId,
                DateLiked = DateLiked,
                // BeltCategory = BeltCategory
            };
            _context.Likes.Add(newLike);
            _context.SaveChanges();
            return View("Index");
        }

        [HttpGet]
        [Route("Profile/{Id}")]
        public IActionResult Profile(int id)
        {
            ViewBag.OtherUsers = new List<string>();

            int? SessionId = HttpContext.Session.GetInt32("UserId");
            if (SessionId == null){
                TempData["error"] = "Must be logged in to view this page";
                return RedirectToAction("Index", "Login");
            }

            User OtherUsers = _context.Users.Where(user => user.UserId == id).Include(user => user.Ideas).Include(user => user.Likes).SingleOrDefault();
            ViewBag.OtherUsers = OtherUsers;

            List<Idea> TheirIdeas = _context.Ideas.Where(user => user.UserId == id).Include(user => user.User).ToList();
            ViewBag.TheirIdeas = TheirIdeas;


            return View("Profile");
        }


            
    

    }
}