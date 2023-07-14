using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Web_RealEstate.Models;
using Web_RealEstate.Reposistory;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Mail;
using System.Text;

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
        private const int PageSize = 6;

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

        public IActionResult AllProperties(int page = 1)
        {
            int totalCount = _postReposistory.GetTotalCount();

            List<Post> posts = _postReposistory.GetAll(page, PageSize);

            HomeModel model = new HomeModel
            {
                MyPost = posts,
                CurrentPageIndex = page,
                PageSize = PageSize,
                TotalCount = totalCount
            };

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

        public IActionResult AllNews(int page = 1)
        {
            int pagesize = 6;

            int totalCount = _newsReposistory.GetTotalCount();

            List<News> news = _newsReposistory.GetAll(page, PageSize);

            HomeModel model = new HomeModel
            {
                MyNews = news,
                CurrentPageIndex = page,
                PageSize = PageSize,
                TotalCount = totalCount
            };

            return View(model);
        }

        public IActionResult Project(int page = 1)
        {
            int pageSize = 6;

            List<Post> objPostList = _postReposistory.GetAll();

            List<Property> objPropertyList = _propertyReposistory.GetAll();

            // Filter the posts based on the Property's CategoryId
            List<Post> filteredPosts = objPostList.Where(p => objPropertyList.Any(prop => prop.Id == p.RealEstateId && prop.CategoryId == 2)).ToList();

            int totalCount = filteredPosts.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            List<Post> posts = filteredPosts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            HomeModel model = new HomeModel
            {
                MyPost = posts,
                CurrentPageIndex = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View(model);
        }

        public IActionResult Buy(int page = 1)
        {
            int pageSize = 6;

            List<Post> objPostList = _postReposistory.GetAll();

            List<Property> objPropertyList = _propertyReposistory.GetAll();

            // Filter the posts based on the Property's CategoryId for buying properties
            List<Post> filteredPosts = objPostList.Where(p => objPropertyList.Any(prop => prop.Id == p.RealEstateId && prop.CategoryId == 3)).ToList();

            int totalCount = filteredPosts.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            List<Post> posts = filteredPosts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            HomeModel model = new HomeModel
            {
                MyPost = posts,
                CurrentPageIndex = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View(model);
        }

        public IActionResult Rent(int page = 1)
        {
            int pageSize = 6;

            List<Post> objPostList = _postReposistory.GetAll();

            List<Property> objPropertyList = _propertyReposistory.GetAll();

            // Filter the posts based on the Property's CategoryId for rental properties
            List<Post> filteredPosts = objPostList.Where(p => objPropertyList.Any(prop => prop.Id == p.RealEstateId && prop.CategoryId == 4)).ToList();

            int totalCount = filteredPosts.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            List<Post> posts = filteredPosts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            HomeModel model = new HomeModel
            {
                MyPost = posts,
                CurrentPageIndex = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

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

            return View("UserDetail", user);
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
                HttpContext.Session.SetInt32("Id", adlog[0].Id);
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
            var existingEmail = _userReposistory.GetExistingEmail(loginUser.Email);

            if (existingUser != null)
            {
                TempData["NameMessage"] = "Username already exists. Please choose a different username!";
                return View(loginUser);
            }
            if (existingEmail != null)
            {
                TempData["MailMessage"] = "Email already been registered. Try Again !!";
                return View(loginUser);
            }
            _userReposistory.AddNewUser(loginUser);
            TempData["regMessage"] = "Account successfully registered!";
            return RedirectToAction("Login");
        }

        public IActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPass(LoginUser loginUser)
        {
            var existingUser = _userReposistory.GetExistingEmail(loginUser.Email);

            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "Email does not exist. Please enter a valid email!";
                return View(loginUser);
            }

            TempData["regMessage"] = "Success!!";
            return RedirectToAction("ForgotPass");
        }

        public IActionResult ResetPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPass(string email, string password)
        {
            var existingUser = _userReposistory.GetExistingEmail(email);

            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "Incorrect email . Please Try Again!";
                return RedirectToAction("ResetPass");
            }

            if (existingUser.PassWord == password)
            {
                TempData["ErrorMessage"] = "This is an old password. Please enter a new Password!";
                return RedirectToAction("ResetPass");
            }

            existingUser.PassWord = password;
            _userReposistory.SaveChanges();

            TempData["upMessage"] = "Password updated successfully!";
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