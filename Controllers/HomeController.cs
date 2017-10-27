using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BeltExam.Models;
using System.Linq;
using System;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
    private IdeaContext _context;
 
    public HomeController(IdeaContext context)
    {
        _context = context;
    }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.errors = ModelState.Values;
            ViewBag.invalid = TempData["error"];
            return View();
        }

        [HttpPost]
        [Route("Register")]

        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User NewUser = new User{
                    Name = model.Name,
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now                    
                };
                    _context.Users.Add(NewUser);
                    _context.SaveChanges();

                    User LoggedIn = _context.Users.SingleOrDefault(user => user.Email == model.Email);
                    HttpContext.Session.SetInt32("UserId", LoggedIn.UserId);
                   
            } else {
                ViewBag.errors = ModelState.Values;
                return View("Index");                
            }
            return RedirectToAction("Index", "Dashboard"); 
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User model)
        {
            User currUser = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (currUser.Password == model.Password)
            {
                HttpContext.Session.SetInt32("UserId", currUser.UserId);

            }else{
                TempData["error"] = "Password/Email does not match";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Dashboard");
        }
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

