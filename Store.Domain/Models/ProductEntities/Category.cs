namespace Store.Domain.Models.ProductEntities
{
    public class Category : Entity
    {
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual int Order { get; set; }

        public Category(int id, string name, string displayName, string description, int order)
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
            Description = description;
            Order = order;
        }

        protected Category()
        {
            
        }
    }
}
