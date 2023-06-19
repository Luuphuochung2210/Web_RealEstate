using Web_RealEstate.Models;
using Microsoft.EntityFrameworkCore;
using Web_RealEstate.Models.Admin_Models;

namespace Web_RealEstate.Reposistory
{
    public interface IAdminReposistory
    {
        public List<Admin> GetAll();
        public List<Admin> AdminLogin(Admin admin);
    }

    public class AdminReposistory : IAdminReposistory
    {
        private Ntphu24072001CnaContext _ctx;

        public AdminReposistory(Ntphu24072001CnaContext ctx)
        {
            _ctx = ctx;
        }
        public List<Admin> GetAll()
        {
            return _ctx.Admins.ToList();
        }

        public List<Admin> AdminLogin(Admin admin)
        {
            return _ctx.Admins.Where(a => a.UserName.Equals(admin.UserName) && a.PassWord.Equals(admin.PassWord)).ToList();
        }
    }
}