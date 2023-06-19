using Web_RealEstate.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Web_RealEstate.Reposistory
{
    public interface IPropertyReposistory
    {
        public List<Property> GetAll();

        public Property GetPropertyById(int id);

        public List<Property> SearchProperties(string name);
    }

    public class PropertyReposistory : IPropertyReposistory
    {
        private Ntphu24072001CnaContext _ctx;
        public PropertyReposistory(Ntphu24072001CnaContext ctx)
        {
            _ctx = ctx;
        }

        public List<Property> GetAll()
        {
            return _ctx.Properties.ToList();
        }


        public Property GetPropertyById(int id)
        {
            return _ctx.Properties
                .Where(x => x.Id == id)
                .Include(x => x.Posts)
                .ThenInclude(post => post.Seller)
                .Include(x => x.Location)
                .Include(x => x.Category)
                .Include(x => x.ChuDauTu)
                .SingleOrDefault();
        }

        public List<Property> SearchProperties(string name)
        {
            var query = _ctx.Properties.AsNoTracking().AsQueryable();

            // Apply search filters
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name) );
            }

            //if (!string.IsNullOrEmpty(dientich))
            //{
            //    query = query.Where(p => p.DienTich == dientich);
            //}

            // Execute the query and return the results
            return query
                .Include(x=>x.Category)
                .Include(x=>x.Location)
                .Include(x=>x.ChuDauTu).ToList();
        }
    }
}