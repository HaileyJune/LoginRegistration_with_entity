using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LoginRegistration.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace LoginRegistration.Controllers
{
    public class HomeController : Controller
{
    private LoginRegContext dbContext;

    // here we can "inject" our context service into the constructor
    public HomeController(LoginRegContext context)
    {
        dbContext = context;
    }

//this is the login register page
    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

//this is what happens when you register
    [HttpPost("register")]
    public IActionResult Register(UserObject user)
    {
        // Check initial ModelState
        if(ModelState.IsValid)
        {
            // If a User exists with provided email
            if(dbContext.Users.Any(u => u.Email == user.Email))
            {
                // Manually add a ModelState error to the Email field, with provided error message
                ModelState.AddModelError("Email", "Email already in use!");
                
                // You may consider returning to the View at this point
                return View("Index", user);
            }
            else
            {
                // Initializing a PasswordHasher object, providing our User class as its
                PasswordHasher<UserObject> Hasher = new PasswordHasher<UserObject>();
                user.Password = Hasher.HashPassword(user, user.Password);
                //Save your user object to the database
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("userid", user.UserId);
                return Redirect("/success"); //This doesn't exist yet
            }
        }
        // other code
        else
        {
            return View("Index", user);
        }
    }

    //this is what happens when you login
    [HttpPost("doeslogin")]
    public IActionResult DoesLogin(LoginUser userSubmission)
    {
        if(ModelState.IsValid)
        {
            // If inital ModelState is valid, query for a user with provided email
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            // If no user exists with provided email
            if(userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "Yeah, I've never seen this email before.");
                return View("Index", userSubmission);
            }
            
            // Initialize hasher object
            var hasher = new PasswordHasher<LoginUser>();
            
            // varify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
            
            // result can be compared to 0 for failure
            if(result == 0)
            {
                // handle failure (this should be similar to how "existing email" is handled)
                ModelState.AddModelError("Email", "How did you forget your password?");
                return View("Index", userSubmission);
            }
            var user = dbContext.Users.SingleOrDefault(u => u.Email == userSubmission.Email);
            HttpContext.Session.SetInt32("userid", user.UserId);
            return Redirect("/success"); //This doesn't exist yet
        }
        else
        {
            return View ("Index", userSubmission);
        }
    }

    [HttpGet]
    [Route("success")]
    public IActionResult Success()
    {
        
        if (HttpContext.Session.GetInt32("userid") != null)
        {
            return View();
        }
        else
        {
            return Redirect("/");
        }
    }

    [HttpGet]
    [Route("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Redirect("/");
    }
}
}