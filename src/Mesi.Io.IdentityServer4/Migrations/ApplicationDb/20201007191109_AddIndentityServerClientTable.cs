using Microsoft.EntityFrameworkCore.Migrations;

namespace Mesi.Io.IdentityServer4.Migrations.ApplicationDb
{
    public partial class AddIndentityServerClientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "identityserver_clients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 50, nullable: false),
                    ClientName = table.Column<string>(maxLength: 100, nullable: false),
                    AllowedGrantTypes = table.Column<string>(nullable: true),
                    RequireClientSecret = table.Column<bool>(nullable: false, defaultValue: true),
                    ClientSecrets = table.Column<string>(nullable: true),
                    AccessTokenLifetime = table.Column<int>(nullable: false, defaultValue: 300),
                    RedirectUris = table.Column<string>(nullable: true),
                    PostLogoutRedirectUris = table.Column<string>(nullable: true),
                    AllowedScopes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityserver_clients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_identityserver_clients_ClientId",
                table: "identityserver_clients",
                column: "ClientId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identityserver_clients");
        }
    }
}
