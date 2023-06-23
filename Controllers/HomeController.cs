using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Web_RealEstate.Models;
using Web_RealEstate.Reposistory;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace Web_RealEstate.Controllers
{
    public class HomeController : Controller
    {
        private IPostReposistory _postReposistory;
        private ISellerReposistory _sellerReposistory;
        private IPropertyReposistory _propertyReposistory;
        private INewsReposistory _newsReposistory;
        private IUserReposistory _userReposistory;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(IPostReposistory postReposistory,
                              ISellerReposistory sellerReposistory,
                              IPropertyReposistory propertyReposistory,
                              INewsReposistory newsReposistory,
                              IUserReposistory userReposistory,
                              IWebHostEnvironment webHost
            )
        //store all connection string and retrieve table top data
        {
            _postReposistory = postReposistory;
            _sellerReposistory = sellerReposistory;
            _propertyReposistory = propertyReposistory;
            _newsReposistory = newsReposistory;
            _userReposistory = userReposistory;
            webHostEnvironment = webHost;
        }

        //GET HOME
        public IActionResult Index()
        {
            HomeModel model = new HomeModel();

            List<Post> objPostList = _postReposistory.GetTop3();
            model.MyPost = objPostList;

            List<Property> objPropertyList = _propertyReposistory.GetAll();
            model.MyProp = objPropertyList;

            List<Seller> objSellerList = _sellerReposistory.GetTop3();
            model.MySeller = objSellerList;

            List<News> objNewsList = _newsReposistory.GetTop3();
            model.MyNews = objNewsList;

            List<LoginUser> objUserList = _userReposistory.GetAll();
            model.MyUser = objUserList;

            return View(model);
        }

        public IActionResult AllProperties()
        {
            HomeModel model = new HomeModel();

            List<Post> objPostList = _postReposistory.GetAll();
            model.MyPost = objPostList;

            List<Property> objPropertyList = _propertyReposistory.GetAll();
            model.MyProp = objPropertyList;

            return View(model);
        }

        public IActionResult Agent(int id)
        {
            var objSeller = _sellerReposistory.GetById(id);

            return View("Agent", objSeller);
        }

        public IActionResult AllAgents()
        {
            var objSellerList = _sellerReposistory.GetAll();

            return View("AllAgents", objSellerList);
        }

        public IActionResult New(int id)
        {
            var objNew = _newsReposistory.GetById(id);

            return View("New", objNew);
        }

        public IActionResult AllNews()
        {
            var objNewsList = _newsReposistory.GetAll();

            return View("AllNews", objNewsList);
        }

        public IActionResult Project()
        {
            HomeModel model = new HomeModel();

            List<Post> objPostList = _postReposistory.GetAll();
            model.MyPost = objPostList;

            List<Property> objPropertyList = _propertyReposistory.GetAll();
            model.MyProp = objPropertyList;

            return View(model);
        }

        public IActionResult Buy()
        {
            HomeModel model = new HomeModel();

            List<Post> objPostList = _postReposistory.GetAll();
            model.MyPost = objPostList;

            List<Property> objPropertyList = _propertyReposistory.GetAll();
            model.MyProp = objPropertyList;

            return View(model);
        }

        public IActionResult Rent()
        {
            HomeModel model = new HomeModel();

            List<Post> objPostList = _postReposistory.GetAll();
            model.MyPost = objPostList;

            List<Property> objPropertyList = _propertyReposistory.GetAll();
            model.MyProp = objPropertyList;

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            Property property = _propertyReposistory.GetPropertyById(id);

            return View("Detail", property);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult UserDetail(int id)
        {
            LoginUser user = _userReposistory.GetById(id);

            return View("UserDetail",user);
        }
        [HttpPost]
        public IActionResult UserDetail(LoginUser user)
        {
            _userReposistory.EditingHomeUser(user);
            TempData["upMessage"] = "Update User Successfully!!!";

            return RedirectToAction("Index");
        }
        public string UploadImage(LoginUser user)
        {
            string uniqueFileName = null;

            if (user.ImageUpload != null)
            {
                //duong dan vao folder
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + user.ImageUpload.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    user.ImageUpload.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        /* --------------- Login --------------- */

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUser loginUser)
        {
            var adlog = _userReposistory.HomeLogin(loginUser);
            var isAccountDisabled = adlog?.FirstOrDefault()?.Status == 0;

            if (isAccountDisabled)
            {
                TempData["disMessage"] = "Your account has been disabled, please contact the admin!";
                return View("Login");
            }

            if (adlog != null && adlog.Count > 0)
            {
                /* Successful login */
                HttpContext.Session.SetString("UserName", adlog[0].UserName);
                return RedirectToAction("Index");
            }
            else
            {
                // Invalid credentials, show an error message or redirect to the login page
                ModelState.AddModelError("", "Invalid username or password");

                // Clear the input fields by creating a new instance of LoginUser
                var emptyhome = new LoginUser();

                // Assign the new instance to the loginUser parameter
                loginUser.UserName = emptyhome.UserName;
                loginUser.PassWord = emptyhome.PassWord;

            }

            TempData["Message"] = "Incorrect Password or Username. Try Again !!!";
            return View(loginUser);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(LoginUser loginUser)
        {
            var existingUser = _userReposistory.GetExistingUser(loginUser.UserName);

            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Username already exists. Please choose a different username!";
                return View(loginUser);
            }
            _userReposistory.AddNewUser(loginUser);
            TempData["regMessage"] = "Account successfully registered!";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            // Clear the session
            _userReposistory.ClearSession();

            // Redirect to the login page or any other appropriate page
            return RedirectToAction("Index", "Home");
        }

        /* --------------- PAGES --------------- */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SearchResult()
        {
            var name = Request.Form["name"].ToString();

            //int id = Convert.ToInt32(Request.Form["id"].ToString());

            //var dientich = Request.Form["dientich"].ToString();

            List<Property> searchResult = _propertyReposistory.SearchProperties(name);

            return View("SearchResult", searchResult);
        }
    }
}