namespace Web_RealEstate.Models.Admin_Models
{
    public class AdminModel
    {
        public List<Post> MyPost { get; set; }
        public List<Seller> MySeller { get; set; }
        public List<Property> MyProp { get; set; }
        public List<News> MyNews { get; set; }
        public List<LoginUser> MyUser { get; set; }
        public List<ChuDauTu> MyInvestor { get; set; }
        public List<Location> MyLocation { get; set; }
        public List<Category> MyCate { get; set; }
    }
}
