using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScrumMaster.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScrumMasterSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "m_UserDetail",
                type: "TEXT",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "m_UserDetail");
        }
    }
}
