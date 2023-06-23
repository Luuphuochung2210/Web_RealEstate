using Web_RealEstate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace Web_RealEstate.Reposistory
{
    public interface IUserReposistory
    {
        public List<LoginUser> GetAll();
        public LoginUser GetById(int Id);
        public void AddNewUser(LoginUser user);
        public void EditingHomeUser(LoginUser user);
        public void EditingUser(LoginUser user);

        public void DeleteUsers(LoginUser user);

        public void UploadFile(LoginUser user);

        /* Login For Home Page*/
        public List<LoginUser> HomeLogin(LoginUser loginUser);
        public LoginUser GetExistingUser(string username);
        public void ClearSession();
    }

    public class UserReposistory : IUserReposistory
    {
        private Ntphu24072001CnaContext _ctx;
        //Upload Hinh
        private readonly IWebHostEnvironment webHostEnvironment;
        //Logout
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserReposistory(Ntphu24072001CnaContext ctx, IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = ctx;
            webHostEnvironment = webHost;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<LoginUser> GetAll()
        {
            return _ctx.LoginUsers.ToList();
        }

        public LoginUser GetById(int Id)
        {
            return _ctx.LoginUsers
                    .Where(x => x.Id == Id)
                    .SingleOrDefault();
        }

        public LoginUser GetExistingUser(string username)
        {
            return _ctx.LoginUsers.FirstOrDefault(a => a.UserName == username);
        }

        public void AddNewUser(LoginUser user)
        {
            user.Status = 1;
            user.Phone = "0";
            // Add the new user to the database
            _ctx.LoginUsers.Add(user);
            _ctx.SaveChanges();
        }

        public void EditingUser(LoginUser user)
        {
            if (user != null)
            {
                var existingUser = _ctx.LoginUsers.Where(x => x.Id == user.Id).FirstOrDefault();
                if (existingUser != null)
                {
                    existingUser.Id = user.Id;
                    existingUser.UserName = user.UserName;
                    existingUser.PassWord = user.PassWord;
                    existingUser.Name = user.Name;
                    existingUser.Phone = user.Phone;
                    existingUser.Status = user.Status;
                    existingUser.Address = user.Address;
                    existingUser.Email = user.Email;

                    if (user.ImageUpload != null && user.ImageUpload.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(existingUser.Image))
                        {
                            DeleteImage(existingUser.Image);
                        }
                        existingUser.Image = SaveImage(user.ImageUpload);
                    }
                    _ctx.Attach(existingUser);
                    _ctx.Entry(existingUser).State = EntityState.Modified;
                    _ctx.SaveChanges();
                }
            }
        }

        public void EditingHomeUser(LoginUser user)
        {
            if (user != null)
            {
                var existingUser = _ctx.LoginUsers.Where(x => x.Id == user.Id).FirstOrDefault();
                if (existingUser != null)
                {
                    existingUser.Id = user.Id;
                    existingUser.UserName = user.UserName;
                    existingUser.PassWord = user.PassWord;
                    existingUser.Name = user.Name;
                    existingUser.Phone = user.Phone;
                    existingUser.Status = 1;
                    existingUser.Address = user.Address;
                    existingUser.Email = user.Email;

                    if (user.ImageUpload != null && user.ImageUpload.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(existingUser.Image))
                        {
                            DeleteImage(existingUser.Image);
                        }
                        existingUser.Image = SaveImage(user.ImageUpload);
                    }
                    _ctx.Attach(existingUser);
                    _ctx.Entry(existingUser).State = EntityState.Modified;
                    _ctx.SaveChanges();
                }
            }
        }


        public void UploadFile(LoginUser user)
        {
            _ctx.LoginUsers.Add(user);
            _ctx.Entry(user).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        private void DeleteImage(string imageName)
        {
            string imagePath = Path.Combine(webHostEnvironment.WebRootPath, "img");
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        private string SaveImage(IFormFile imageFile)
        {
            string uniqueFileName = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string imageFilePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public List<LoginUser> HomeLogin(LoginUser loginUser)
        {
            return _ctx.LoginUsers.Where(a => a.UserName.Equals(loginUser.UserName) && a.PassWord.Equals(loginUser.PassWord)).ToList();
        }

        public void DeleteUsers(LoginUser user)
        {
            if (user != null)
            {
                _ctx.LoginUsers.Remove(user);
                _ctx.SaveChanges();
            }
        }

        public void ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
