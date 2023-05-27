namespace Store.Domain.Models.ProductEntities
{
    public class Category : Entity
    {
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual int Order { get; set; }

        protected Category()
        {
            
        }
    }
}
