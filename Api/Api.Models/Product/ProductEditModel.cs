namespace Api.Models.Product
{
    public class ProductEditModel : ProductCreateModel
    {
        public bool IsBlocked { get; set; }
    }
}
