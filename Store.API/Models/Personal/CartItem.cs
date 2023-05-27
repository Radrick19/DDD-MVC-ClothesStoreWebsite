using Store.Application.Dto.Administration;


namespace Store.API.Models.Personal
{
    public class CartItem
    {
        public string Name { get; set; }
        public string Article { get; set; }
        public string MainPicture { get; set; }
        public ColorDto Color { get; set; }
        public SizeDto Size { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
