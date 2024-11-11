using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBData.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<string>(type: "text", nullable: false),
                    IsPurchased = table.Column<bool>(type: "boolean", nullable: false),
                    PurchaseTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShoppingListId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingItemParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ShoppingItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingItemParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingItemParameters_ShoppingItems_ShoppingItemId",
                        column: x => x.ShoppingItemId,
                        principalTable: "ShoppingItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItemParameters_ShoppingItemId",
                table: "ShoppingItemParameters",
                column: "ShoppingItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_ShoppingListId",
                table: "ShoppingItems",
                column: "ShoppingListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingItemParameters");

            migrationBuilder.DropTable(
                name: "ShoppingItems");

            migrationBuilder.DropTable(
                name: "ShoppingLists");
        }
    }
}
