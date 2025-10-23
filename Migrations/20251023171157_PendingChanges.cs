using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScrumMaster.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_m_ScrumDetails_m_ProjectDetail_ProjectId",
                table: "m_ScrumDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_m_ScrumDetails_m_UserDetail_UserId",
                table: "m_ScrumDetails");

            migrationBuilder.DropIndex(
                name: "IX_m_ScrumDetails_ProjectId",
                table: "m_ScrumDetails");

            migrationBuilder.DropIndex(
                name: "IX_m_ScrumDetails_UserId",
                table: "m_ScrumDetails");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "m_ScrumDetails",
                newName: "TaskID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskID",
                table: "m_ScrumDetails",
                newName: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_m_ScrumDetails_ProjectId",
                table: "m_ScrumDetails",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_m_ScrumDetails_UserId",
                table: "m_ScrumDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_m_ScrumDetails_m_ProjectDetail_ProjectId",
                table: "m_ScrumDetails",
                column: "ProjectId",
                principalTable: "m_ProjectDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_m_ScrumDetails_m_UserDetail_UserId",
                table: "m_ScrumDetails",
                column: "UserId",
                principalTable: "m_UserDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
