using BasicEfCoreRelationships.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicEfCoreRelationships.Data;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.Title).HasColumnName("Title").HasMaxLength(32).IsRequired();
        builder.Property(p => p.Content).HasColumnName("Content").HasMaxLength(256).IsRequired();
        builder.Property(p => p.CategoryId).HasColumnName("CategoryId");
        // one-to-many
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Posts)
            .HasForeignKey(p => p.CategoryId)
            // It is not necessary to delete blog posts when a category is deleted, because the blog posts can
            // still exist without a category and can be assigned to another category. However, if a category is
            // deleted, the CategoryId property of a blog post, which is a foreign key, will no longer match
            // the primary key of any category. Therefore, you may want to set the CategoryId property
            // to null when a category is deleted. This behavior is called nullification. To allow this, the
            // requirement is that the CategoryId property is nullable. If the CategoryId property of
            // a blog post entity is not nullable, EF Core will throw an exception when you try to delete a
            // category because it will violate the foreign key constraint.
            // In the sample code, there is an example of this case. You can find the Category and Post
            // classes in the Models folder. Similar to the Invoice and InvoiceItem classes, they have a
            // one-to-many relationship. However, the CategoryId property in the Post class is nullable.
            // Therefore, you can set DeleteBehavior to ClientSetNull to nullify the CategoryId
            // property when a category is deleted.
            .OnDelete(DeleteBehavior.ClientSetNull);
        // In the OnDelete() method, you can pass the DeleteBehavior enum to set
        // DeleteBehavior to ClientSetNull. The ClientSetNull value means that the
        // foreign key property will be set to null when the principal entity is deleted.
    }
}