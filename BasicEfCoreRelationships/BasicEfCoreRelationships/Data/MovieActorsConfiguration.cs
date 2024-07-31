// using BasicEfCoreRelationships.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Another way (before EF Core 5.0) to configure a many-to-many relationship is to use the join entity
// to represent two separate one-to-many relationships. Here is an example of configuring a many-tomany relationship for the Movie and Actor entities:

// namespace BasicEfCoreRelationships.Data;

// OLD WAY OF CREATING MANY-TO-MANY relationship through MovieActor join table
// public class MovieActorsConfiguration :
//     IEntityTypeConfiguration<MovieActor>
// {
//     public void Configure(EntityTypeBuilder<MovieActor> builder)
//     {
//         builder.ToTable("MovieActors");
//         builder.HasKey(sc => new { sc.MovieId, sc.ActorId });
//         builder.HasOne(sc => sc.Actor)
//             .WithMany(s => s.MovieActors)
//             .HasForeignKey(sc => sc.ActorId);
//         builder.HasOne(sc => sc.Movie)
//             .WithMany(c => c.MovieActors)
//             .HasForeignKey(sc => sc.MovieId);
//     }
// }

// In the preceding code, we configured two one-to-many relationships for the Movie and Actor
// entities on the MovieActor join entity. Each one-to-many relationship uses the HasMany(),
// WithMany(), and HasForeignKey() methods to configure the relationship. This combination
// of one-to-one relationships creates a many-to-many relationship.

// You can use either way to configure a many-to-many relationship. The HasMany()/WithMany()
// methods is more convenient and easier to use.

