namespace BasicEfCoreReverseEngineering.Models;

public class Actor
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MovieActor> MovieActors { get; } = new List<MovieActor>();
}