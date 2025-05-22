using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dkt_learn_core.shared.Datas.Migrations
{
    /// <inheritdoc />
    public partial class NomePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Replies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Replies");
        }
    }
}
