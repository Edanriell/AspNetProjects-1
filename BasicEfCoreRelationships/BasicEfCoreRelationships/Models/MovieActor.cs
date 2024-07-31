namespace BasicEfCoreRelationships.Models;

// many-to-many
// Join entity is not necessary, we can create an many-to many 
// relationship without it in newer versions of ASP.NET
// It is also automatically configured.
// However, sometimes the automatic detection of the many-to-many relationship may not meet our
// requirements. For example, we may want to call the table MovieActor instead of ActorMovie,
// we may want to specify the foreign key properties as ActorId and MovieId instead of ActorsId
// and MoviesId, or we may even want to add some additional properties to the join table. In these
// cases, we can explicitly configure the many-to-many relationship.
public class MovieActor
{
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    public Guid ActorId { get; set; }
    public Actor Actor { get; set; } = null!;
    public DateTime UpdateTime { get; set; }
}