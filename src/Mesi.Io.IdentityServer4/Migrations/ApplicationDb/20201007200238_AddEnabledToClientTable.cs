using Microsoft.EntityFrameworkCore.Migrations;

namespace Mesi.Io.IdentityServer4.Migrations.ApplicationDb
{
    public partial class AddEnabledToClientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "identityserver_clients",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "identityserver_clients");
        }
    }
}
