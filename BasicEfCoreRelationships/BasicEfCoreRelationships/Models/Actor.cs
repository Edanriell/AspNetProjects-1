using System.Text.Json.Serialization;

namespace BasicEfCoreRelationships.Models;

public class Actor
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // many-to-many
    // Automatic many-to-many
    public List<Movie> Movies { get; set; } = new();

    // Obligatory when we create custom join entity and table
    [JsonIgnore] public List<MovieActor> MovieActors { get; set; } = new();
}