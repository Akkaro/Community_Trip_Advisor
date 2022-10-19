using Community_Trip_Advisor.DAL;
using Community_Trip_Advisor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Community_Trip_Advisor.Controllers
{
    public class CommentController : Controller
    {
        private CommentsDAL _dal;

        public CommentController(ILogger<CommentController> logger)
        {
            _dal = new CommentsDAL();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
