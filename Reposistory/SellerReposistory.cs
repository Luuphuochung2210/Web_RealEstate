using Web_RealEstate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_RealEstate.Reposistory
{
    public interface ISellerReposistory
    {
        public List<Seller> GetAll();

        public List<Seller> GetTop3();

        public Seller GetById(int Id);

        public void DeleteAgent(Seller seller);

        public void EditingAgent(Seller seller);
    }

    public class SellerReposistory : ISellerReposistory
    {
        private Ntphu24072001CnaContext _ctx;
        public SellerReposistory(Ntphu24072001CnaContext ctx)
        {
            _ctx = ctx;
        }

        public List<Seller> GetAll()
        {
            return _ctx.Sellers.ToList();
        }

        public List<Seller> GetTop3()
        {
            return _ctx.Sellers.OrderByDescending(x => x.Id).Take(3).ToList();
        }

        public Seller GetById(int Id)
        {
            return _ctx.Sellers
                .Where(x => x.Id == Id) 
                .Include(x => x.Posts)
                .ThenInclude(p => p.RealEstate).ThenInclude(real => real.Category)
                .Include(x => x.Posts)
                .ThenInclude(p => p.RealEstate).ThenInclude(real => real.Location)
                .Include(x => x.Posts)
                .ThenInclude(p => p.RealEstate).ThenInclude(real => real.ChuDauTu)
                .SingleOrDefault();
        }

        public void EditingAgent(Seller seller)
        {
            if (seller != null)
            {
                var existingAgent = _ctx.Sellers.Where(x => x.Id == seller.Id).Include(prop => prop.Image).FirstOrDefault();
                if (existingAgent != null)
                {
                    existingAgent.Id = seller.Id;
                    existingAgent.UserName = seller.UserName;
                    existingAgent.PassWord = seller.PassWord;
                    existingAgent.Phone = seller.Phone;
                    existingAgent.Address = seller.Address;
                    existingAgent.Email = seller.Email;
                    existingAgent.Name = seller.Name;

                    _ctx.SaveChanges();
                }
            }
        }
        public void DeleteAgent(Seller seller)
        {
            if (seller != null)
            {
                _ctx.Sellers.Remove(seller);
                _ctx.SaveChanges();
            }
        }
    }
}
