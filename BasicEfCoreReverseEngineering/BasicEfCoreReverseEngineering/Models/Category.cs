namespace BasicEfCoreReverseEngineering.Models;

public class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}