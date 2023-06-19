using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Web_RealEstate.Models;
using Web_RealEstate.Reposistory;
using System.Diagnostics;

namespace Web_RealEstate.Controllers
{
    public class HomeController : Controller
    {
        private IPostReposistory _postReposistory;
        private ISellerReposistory _sellerReposistory;
        private IPropertyReposistory _propertyReposistory;
        private INewsReposistory _newsReposistory;
        private IUserReposistory _userReposistory;

        public HomeController(IPostReposistory postReposistory,
                              ISellerReposistory sellerReposistory,
                              IPropertyReposistory propertyReposistory,
                              INewsReposistory newsReposistory,
                              IUserReposistory userReposistory
            )
        //store all connection string and retrieve table top data
        {
            _postReposistory = postReposistory;
            _sellerReposistory = sellerReposistory;
            _propertyReposistory = propertyReposistory;
            _newsReposistory = newsReposistory;
            _userReposistory = userReposistory;
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

        public IActionResult UserDetail()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

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