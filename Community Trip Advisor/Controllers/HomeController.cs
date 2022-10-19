using Community_Trip_Advisor.DAL;
using Community_Trip_Advisor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using Community_Trip_Advisor.Areas.Identity.Data;

namespace Community_Trip_Advisor.Controllers
{

    public class HomeController : Controller
    {

        private PlacesDAL _dalPlace;
        private CommentsDAL _dalCom;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Community_Trip_AdvisorUser> _userManager;

        public HomeController(ILogger<HomeController> logger)
        {
            _dalPlace = new PlacesDAL();
            _dalCom = new CommentsDAL();
        }


        public IActionResult ListPlaces()
        {
            var placeList = _dalPlace.ListPlaces();
            foreach (var place in placeList)
            {
                AddCommentsToPlace(place);
            }
            List<Place> SortedList = placeList.OrderByDescending(o => o.RatingValue).ToList();
            return View(SortedList);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddPlace()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPlace(Place place)
        {
            string fileName = place.ImgTitle.Replace(" ", "_");
            string extension = Path.GetExtension(place.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            place.ImgPath = fileName;
            fileName = "wwwroot\\Images\\" + fileName;
            using (Stream fileStream = new FileStream(fileName, FileMode.Create))
            {
                await place.ImageFile.CopyToAsync(fileStream);
            }   
            ModelState.Clear();
            _dalPlace.AddPlace(place);
            return RedirectToAction("");
        }

        [HttpGet]
        public IActionResult Delete(int id) => View(_dalPlace.GetPlace(id));

        [HttpPost]
        public IActionResult Delete(Place place)
        {
            _dalPlace.DeletePlace(place);
            return RedirectToAction("");
        }
        public IActionResult DeleteComment(int id)
        {
            var currentPlaceId = _dalCom.GetComment(id).PlaceID;
            _dalCom.DeleteCommentById(id);
            return RedirectToAction("Details", currentPlaceId);
    } 
        [HttpGet]
        public IActionResult Details(int id)
        {
            Place place = new Place();
            if (id != 0)
            {
                place = _dalPlace.GetPlace(id);
            }
           

           AddCommentsToPlace(place);
           return View("Details", place);
        }
        [Authorize]
        public void AddCommentsToPlace(Place place)
        {
            place.Comments = _dalCom.ListCommentsWithPlaceID(place.ID.ToString());
            float RatingValue;
            int ratings = 0;
            int numOfRatings = 0;
            foreach(Comment comment in place.Comments)
            {
                ratings += comment.Rating;
                numOfRatings++;
            }
            place.RatingValue = ratings/(float)numOfRatings;
            return;
        }

        [Authorize]
        public IActionResult AddComment (int placeID, int rating, string placeComment)
        {
            Comment comment = new Comment();

            comment.PlaceID = placeID;
            comment.Rating = rating;
            comment.CommmentDescription = placeComment;
            comment.CommentedOn = DateTime.Now;
            comment.AddedBy = User.Identity.GetUserName();

            _dalCom.AddComment(comment);
            return RedirectToAction("Details", _dalPlace.GetPlace(comment.PlaceID));
        }

        public IActionResult Search()
        {
            var placeList = _dalPlace.ListPlaces();
            foreach (var place in placeList)
            {
                AddCommentsToPlace(place);
            }
            return View(placeList);
        }
            

        [HttpPost]
        public IActionResult Search(string searchTerm)
        {
            List<Place> place = _dalPlace.Search(searchTerm);

            return View(place);
        }



        [HttpGet]
        public IActionResult Update(int id) => View(_dalPlace.GetPlace(id));

        [HttpPost]
        public IActionResult Update(Place place)
        {
            _dalPlace.UpdatePlace(place);

            return RedirectToAction("Details", place);
        }




    }


}