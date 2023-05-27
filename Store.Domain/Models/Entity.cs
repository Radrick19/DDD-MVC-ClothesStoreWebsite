namespace Store.Domain.Models
{
    public abstract class Entity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
    }
    public abstract class Entity : Entity<int>
    {

    }
}
