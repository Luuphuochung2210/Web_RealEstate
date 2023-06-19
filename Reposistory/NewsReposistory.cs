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
        //public string UploadImage(News news);
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
        /*public string UploadImage(News news)
        {
            string uniqueFileName = null;
            if (news.FrontImage != null)
            {
                string uploadFile = Path.Combine(webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + news.FrontImage.FileName;
                string filePath = Path.Combine(uploadFile, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    news.FrontImage.CopyTo(fileStream);
                }
                _ctx.Attach(news);
                _ctx.Entry(news).State = EntityState.Added;
                _ctx.SaveChanges();
            }
            return uniqueFileName;
        }*/
    }
}