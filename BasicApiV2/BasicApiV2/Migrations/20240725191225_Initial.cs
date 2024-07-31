using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BasicApiV2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Cloud" },
                    { new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), ".NET" },
                    { new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "DevOps" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryId", "Content", "Title" },
                values: new object[,]
                {
                    { new Guid("01f15ff2-8eb9-40e1-ba26-f019ba3a7efd"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 25 content", "Post 25" },
                    { new Guid("03d8b1e3-aa5d-4a46-b55c-e36306c4c408"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 21 content", "Post 21" },
                    { new Guid("08e69089-2fa7-42b0-94a7-a29b6c27a405"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 5 content", "Post 5" },
                    { new Guid("0b4f02e5-0157-48bb-abbd-683334178808"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 2 content", "Post 2" },
                    { new Guid("0b74512a-c06d-414a-a0e3-e64935b67189"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 7 content", "Post 7" },
                    { new Guid("0f262ca3-fad3-4fbb-a3b0-64547c4a3d88"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 3 content", "Post 3" },
                    { new Guid("11a71a0b-8517-40f5-a64d-86a867e41446"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 23 content", "Post 23" },
                    { new Guid("16f57303-db3a-4788-8bde-1148226a08a4"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 12 content", "Post 12" },
                    { new Guid("193207b8-4838-48bf-9349-f20fbd1180fe"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 1 content", "Post 1" },
                    { new Guid("1fe20a8f-64df-41ea-a2df-d8870bd0d817"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 24 content", "Post 24" },
                    { new Guid("2b6471d5-b141-42c9-83a2-e578a016bf14"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 8 content", "Post 8" },
                    { new Guid("350f7f91-7d60-45d2-807e-60e06df04ad4"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 14 content", "Post 14" },
                    { new Guid("3baa7d32-bb93-4cbd-96df-f287855f3938"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 19 content", "Post 19" },
                    { new Guid("3d24bd1a-c9db-4c54-a881-9a669f4d83df"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 11 content", "Post 11" },
                    { new Guid("4238e1b8-0981-475b-a84d-73453eeec130"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 27 content", "Post 27" },
                    { new Guid("53cf80bc-ad03-40dd-a0ce-a5cbec9ef398"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 29 content", "Post 29" },
                    { new Guid("5e7fdcba-3372-4047-b032-13095bf361dc"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 18 content", "Post 18" },
                    { new Guid("7a6caa2c-7ec8-4697-8ef7-d76f6b954ef4"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 13 content", "Post 13" },
                    { new Guid("8b1fdca2-b3bd-41e5-bd3e-98ba5c6916af"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 4 content", "Post 4" },
                    { new Guid("911d8c20-2379-440b-8b83-c3ef9daa1f96"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 9 content", "Post 9" },
                    { new Guid("944e53b8-5e1d-4251-b574-ca3c9e66f1a2"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 30 content", "Post 30" },
                    { new Guid("946db6f3-6bfe-471e-97c6-c88ff2f39bb0"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 20 content", "Post 20" },
                    { new Guid("9ef6ba95-87ec-46ef-98b4-efc71f9f1b62"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 26 content", "Post 26" },
                    { new Guid("b2b9c57b-98aa-46ea-ac2c-5dfcad92b9d9"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 28 content", "Post 28" },
                    { new Guid("b2bcce5a-f294-4d03-8bd2-4838021cc7bf"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 17 content", "Post 17" },
                    { new Guid("b5995223-db2e-4ff1-9ffd-b7329dc39202"), new Guid("cc1968d0-2e7e-4815-994d-37ba6fc2f914"), "Post 22 content", "Post 22" },
                    { new Guid("c0d9fba1-7481-4803-9fcc-47c8720041d8"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 16 content", "Post 16" },
                    { new Guid("c13c41b7-2420-4cd8-827e-60d3014b2893"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 10 content", "Post 10" },
                    { new Guid("df79408d-fef7-4b83-875a-c61c99c43d7d"), new Guid("aaed6841-852f-42b7-a3ed-655f8f1fccfc"), "Post 6 content", "Post 6" },
                    { new Guid("e41a65c2-1607-48c4-9470-a7c6f818089a"), new Guid("05615567-12c5-42a2-b8f0-cceb01e2e9d3"), "Post 15 content", "Post 15" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
