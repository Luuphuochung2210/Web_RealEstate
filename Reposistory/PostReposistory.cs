using Web_RealEstate.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_RealEstate.Reposistory
{
    public interface IPostReposistory
    {
        public List<Post> GetAll();
        public List<Post> GetAll(int page, int pageSize);
        public List<Post> GetProjects(int page, int pageSize);
        public List<Post> GetBuy(int page, int pageSize);
        public List<Post> GetRent(int page, int pageSize);

        public int GetProjectCount();
        public int GetBuyCount();
        public int GetRentCount();

        public int GetTotalCount();
        public List<Post> GetTop3();
    }

    public class PostReposistory : IPostReposistory
    {
        private Ntphu24072001CnaContext _ctx;
        public PostReposistory(Ntphu24072001CnaContext ctx)
        {
            _ctx = ctx;
        }

        public List<Post> GetAll()
        {
            return _ctx.Posts
                .Include(x => x.RealEstate)
                .ThenInclude(cate => cate.Category)
                .Include(x => x.RealEstate)
                .ThenInclude(locate => locate.Location)
                .Include(x => x.RealEstate)
                .ThenInclude(invest => invest.ChuDauTu).ToList();
        }

        //PAGINATION OF ALL PROPERTY
        public List<Post> GetAll(int page, int pageSize)
        {
            return _ctx.Posts
                .Include(x => x.RealEstate)
                .ThenInclude(cate => cate.Category)
                .Include(x => x.RealEstate)
                .ThenInclude(locate => locate.Location)
                .Include(x => x.RealEstate)
                .ThenInclude(invest => invest.ChuDauTu)
                .Skip((page - 1) * pageSize) // Skip the appropriate number of items
                .Take(pageSize) // Take only the desired number of items
                .ToList();
        }

        public int GetTotalCount()
        {
            return _ctx.Posts.Count();
        }

        //PAGINATION OF BUY
        public List<Post> GetBuy(int page, int pageSize)
        {
            return _ctx.Posts
                .Where(p => p.RealEstate.CategoryId == 3)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetBuyCount()
        {
            return _ctx.Posts.Count(p => p.RealEstate.CategoryId == 3);
        }

        //PAGINATION OF RENT 
        public List<Post> GetRent(int page, int pageSize)
        {
            return _ctx.Posts
                .Where(p => p.RealEstate.CategoryId == 4)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetRentCount()
        {
            return _ctx.Posts.Count(p => p.RealEstate.CategoryId == 4);
        }

        //PAGINATION OF ALL PROJECT 
        public List<Post> GetProjects(int page, int pageSize)
        {
            return _ctx.Posts
                .Where(p => p.RealEstate.CategoryId == 2)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetProjectCount()
        {
            return _ctx.Posts.Count(p => p.RealEstate.CategoryId == 2);
        }

        public List<Post> GetTop3()
        {
            return _ctx.Posts
                .Include(x => x.RealEstate)
                .ThenInclude(cate => cate.Category)
                .Include(x => x.RealEstate)
                .ThenInclude(locate => locate.Location)
                .Include(x => x.RealEstate)
                .ThenInclude(invest => invest.ChuDauTu)
                .OrderByDescending(x => x.Id).Take(3).ToList();
        }
    }
}