﻿// <auto-generated />
using System;
using BasicApiV2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BasicApiV2.Migrations
{
    [DbContext(typeof(SampleDbContext))]
    partial class SampleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BasicApiV2.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Name = ".NET"
                        },
                        new
                        {
                            Id = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Name = "Cloud"
                        },
                        new
                        {
                            Id = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Name = "DevOps"
                        });
                });

            modelBuilder.Entity("BasicApiV2.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CategoryId");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("Content");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Posts", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("193207b8-4838-48bf-9349-f20fbd1180fe"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 1 content",
                            Title = "Post 1"
                        },
                        new
                        {
                            Id = new Guid("0b4f02e5-0157-48bb-abbd-683334178808"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 2 content",
                            Title = "Post 2"
                        },
                        new
                        {
                            Id = new Guid("0f262ca3-fad3-4fbb-a3b0-64547c4a3d88"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 3 content",
                            Title = "Post 3"
                        },
                        new
                        {
                            Id = new Guid("8b1fdca2-b3bd-41e5-bd3e-98ba5c6916af"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 4 content",
                            Title = "Post 4"
                        },
                        new
                        {
                            Id = new Guid("08e69089-2fa7-42b0-94a7-a29b6c27a405"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 5 content",
                            Title = "Post 5"
                        },
                        new
                        {
                            Id = new Guid("df79408d-fef7-4b83-875a-c61c99c43d7d"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 6 content",
                            Title = "Post 6"
                        },
                        new
                        {
                            Id = new Guid("0b74512a-c06d-414a-a0e3-e64935b67189"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 7 content",
                            Title = "Post 7"
                        },
                        new
                        {
                            Id = new Guid("2b6471d5-b141-42c9-83a2-e578a016bf14"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 8 content",
                            Title = "Post 8"
                        },
                        new
                        {
                            Id = new Guid("911d8c20-2379-440b-8b83-c3ef9daa1f96"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 9 content",
                            Title = "Post 9"
                        },
                        new
                        {
                            Id = new Guid("c13c41b7-2420-4cd8-827e-60d3014b2893"),
                            CategoryId = new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"),
                            Content = "Post 10 content",
                            Title = "Post 10"
                        },
                        new
                        {
                            Id = new Guid("3d24bd1a-c9db-4c54-a881-9a669f4d83df"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 11 content",
                            Title = "Post 11"
                        },
                        new
                        {
                            Id = new Guid("16f57303-db3a-4788-8bde-1148226a08a4"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 12 content",
                            Title = "Post 12"
                        },
                        new
                        {
                            Id = new Guid("7a6caa2c-7ec8-4697-8ef7-d76f6b954ef4"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 13 content",
                            Title = "Post 13"
                        },
                        new
                        {
                            Id = new Guid("350f7f91-7d60-45d2-807e-60e06df04ad4"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 14 content",
                            Title = "Post 14"
                        },
                        new
                        {
                            Id = new Guid("e41a65c2-1607-48c4-9470-a7c6f818089a"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 15 content",
                            Title = "Post 15"
                        },
                        new
                        {
                            Id = new Guid("c0d9fba1-7481-4803-9fcc-47c8720041d8"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 16 content",
                            Title = "Post 16"
                        },
                        new
                        {
                            Id = new Guid("b2bcce5a-f294-4d03-8bd2-4838021cc7bf"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 17 content",
                            Title = "Post 17"
                        },
                        new
                        {
                            Id = new Guid("5e7fdcba-3372-4047-b032-13095bf361dc"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 18 content",
                            Title = "Post 18"
                        },
                        new
                        {
                            Id = new Guid("3baa7d32-bb93-4cbd-96df-f287855f3938"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 19 content",
                            Title = "Post 19"
                        },
                        new
                        {
                            Id = new Guid("946db6f3-6bfe-471e-97c6-c88ff2f39bb0"),
                            CategoryId = new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"),
                            Content = "Post 20 content",
                            Title = "Post 20"
                        },
                        new
                        {
                            Id = new Guid("03d8b1e3-aa5d-4a46-b55c-e36306c4c408"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 21 content",
                            Title = "Post 21"
                        },
                        new
                        {
                            Id = new Guid("b5995223-db2e-4ff1-9ffd-b7329dc39202"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 22 content",
                            Title = "Post 22"
                        },
                        new
                        {
                            Id = new Guid("11a71a0b-8517-40f5-a64d-86a867e41446"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 23 content",
                            Title = "Post 23"
                        },
                        new
                        {
                            Id = new Guid("1fe20a8f-64df-41ea-a2df-d8870bd0d817"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 24 content",
                            Title = "Post 24"
                        },
                        new
                        {
                            Id = new Guid("01f15ff2-8eb9-40e1-ba26-f019ba3a7efd"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 25 content",
                            Title = "Post 25"
                        },
                        new
                        {
                            Id = new Guid("9ef6ba95-87ec-46ef-98b4-efc71f9f1b62"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 26 content",
                            Title = "Post 26"
                        },
                        new
                        {
                            Id = new Guid("4238e1b8-0981-475b-a84d-73453eeec130"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 27 content",
                            Title = "Post 27"
                        },
                        new
                        {
                            Id = new Guid("b2b9c57b-98aa-46ea-ac2c-5dfcad92b9d9"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 28 content",
                            Title = "Post 28"
                        },
                        new
                        {
                            Id = new Guid("53cf80bc-ad03-40dd-a0ce-a5cbec9ef398"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 29 content",
                            Title = "Post 29"
                        },
                        new
                        {
                            Id = new Guid("944e53b8-5e1d-4251-b574-ca3c9e66f1a2"),
                            CategoryId = new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"),
                            Content = "Post 30 content",
                            Title = "Post 30"
                        });
                });

            modelBuilder.Entity("BasicApiV2.Models.Post", b =>
                {
                    b.HasOne("BasicApiV2.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BasicApiV2.Models.Category", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}