using System.Text.Json.Serialization;

namespace BasicEfCoreRelationships.Models;

public class Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int ReleaseYear { get; set; }

    // many-to-many
    // Automatic many-to-many
    public List<Actor> Actors { get; set; } = new();

    // Obligatory when we create custom join entity and table
    [JsonIgnore] public List<MovieActor> MovieActors { get; set; } = new();
}