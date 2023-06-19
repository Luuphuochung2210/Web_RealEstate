using Web_RealEstate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Plugins;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Web_RealEstate.Reposistory
{
    public interface INewsReposistory
    {
        public List<News> GetAll();

        public List<News> GetTop3();

        public News GetById(int Id);

        public void Addnew(News news);

        public void EditingNew(News news);

        public void DeleteNews(News news);

        public void UploadFile(News news);
    }

    public class NewsReposistory : INewsReposistory
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private Ntphu24072001CnaContext _ctx;
        public NewsReposistory(Ntphu24072001CnaContext ctx, IWebHostEnvironment webHost)
        {
            _ctx = ctx;
            webHostEnvironment = webHost;
        }

        public List<News> GetAll()
        {
            return _ctx.News.ToList();
        }

        public List<News> GetTop3()
        {
            return _ctx.News.OrderByDescending(x => x.Id).Take(3).ToList();
        }

        public News GetById(int Id)
        {
            return _ctx.News
                .Where(x => x.Id == Id)
                .SingleOrDefault();
        }

        public void Addnew(News news)
        {
            _ctx.News.Add(news);
            _ctx.SaveChanges();
        }

        public void EditingNew(News news)
        {
            if (news != null)
            {
                var existingNew = _ctx.News.Where(x => x.Id == news.Id).FirstOrDefault();
                if (existingNew != null)
                {
                    existingNew.Id = news.Id;
                    existingNew.Contents = news.Contents;
                    existingNew.Date = news.Date;
                    existingNew.Title = news.Title;

                    if (news.ImageUpload != null && news.ImageUpload.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(existingNew.Image))
                        {
                            DeleteImage(existingNew.Image);
                        }
                        existingNew.Image = SaveImage(news.ImageUpload);
                    }
                    _ctx.Attach(existingNew);
                    _ctx.Entry(existingNew).State = EntityState.Modified;
                    _ctx.SaveChanges();
                }
            }
        }

        public void DeleteNews(News news)
        {
            if (news != null)
            {
                _ctx.News.Remove(news);
                _ctx.SaveChanges();
            }
        }

        public void UploadFile(News news)
        {
            _ctx.News.Add(news);
            _ctx.Entry(news).State = EntityState.Added;
            _ctx.SaveChanges();
        }
        private void DeleteImage(string imageName)
        {
            string imagePath = "C://Users//Admin//Desktop//Bài tập về nhà//17,6,2023//RealEstate//RealEstate//wwwroot//img";
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
                string uploadFolder = "C://Users//Admin//Desktop//Bài tập về nhà//17,6,2023//RealEstate//RealEstate//wwwroot//img";
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string imageFilePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}