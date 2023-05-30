using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Models.ProductEntities
{
    public class Subcategory : Entity
    {
        public virtual int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool CanReturn { get; set; }

        public virtual int Order { get; set; }

        protected Subcategory()
        {
            
        }

        public Subcategory(int id, int categoryId, string name, string displayName, string description, bool canReturn, int order)
        {
            Id = id;
            CategoryId = categoryId;
            Name = name;
            DisplayName = displayName;
            Description = description;
            CanReturn = canReturn;
            Order = order;
        }
    }
}
