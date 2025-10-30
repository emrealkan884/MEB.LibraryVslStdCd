using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: new Guid("c129f15b-e842-470f-a6c6-a4c520d6c731"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("19b95d4d-46c8-4a85-bf81-a9e129a9baf3"));

            migrationBuilder.AddColumn<string>(
                name: "SorumluIlKodu",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SorumluIlceKodu",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SorumluKutuphaneId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "Role.SistemYoneticisi");

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "Role.BakanlikYetkilisi");

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Role.IlYetkilisi");

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Role.IlceYetkilisi");

            migrationBuilder.InsertData(
                table: "OperationClaims",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "UpdatedDate" },
                values: new object[] { 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Role.OkulKutuphaneYoneticisi", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthenticatorType", "CreatedDate", "DeletedDate", "Email", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "SorumluIlKodu", "SorumluIlceKodu", "SorumluKutuphaneId", "Status", "UpdatedDate" },
                values: new object[] { new Guid("d22496aa-4ed9-4c1e-9fd0-06c27dbcdd79"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "narch@kodlama.io", "", "", new byte[] { 112, 89, 108, 13, 226, 36, 98, 10, 201, 152, 110, 82, 101, 184, 93, 86, 25, 91, 48, 123, 208, 74, 53, 190, 202, 78, 220, 16, 236, 163, 124, 56, 26, 160, 140, 105, 32, 121, 55, 170, 78, 40, 118, 183, 103, 255, 80, 148, 72, 251, 248, 36, 107, 26, 220, 15, 120, 96, 125, 80, 132, 86, 27, 199 }, new byte[] { 251, 252, 240, 150, 164, 86, 252, 23, 125, 214, 226, 217, 170, 248, 113, 158, 106, 216, 227, 20, 97, 58, 54, 210, 90, 237, 139, 214, 96, 252, 194, 233, 117, 41, 69, 89, 227, 80, 222, 119, 221, 22, 108, 8, 227, 120, 181, 176, 86, 247, 92, 34, 96, 16, 115, 55, 111, 4, 3, 198, 251, 217, 94, 80, 18, 41, 33, 243, 225, 28, 76, 139, 44, 198, 2, 215, 55, 205, 63, 243, 202, 145, 191, 232, 59, 108, 254, 6, 165, 251, 161, 237, 165, 67, 22, 205, 86, 209, 210, 250, 149, 25, 8, 178, 239, 237, 124, 125, 75, 40, 146, 255, 234, 127, 127, 134, 6, 10, 136, 233, 220, 192, 188, 75, 79, 214, 137, 225 }, null, null, null, true, null });

            migrationBuilder.InsertData(
                table: "UserOperationClaims",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "OperationClaimId", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("2cd88658-4f63-4241-b950-dab6eb5e8391"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 24, null, new Guid("d22496aa-4ed9-4c1e-9fd0-06c27dbcdd79") },
                    { new Guid("6148d16a-a58d-4dc5-a73f-ba6ead9bc49c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, new Guid("d22496aa-4ed9-4c1e-9fd0-06c27dbcdd79") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: new Guid("2cd88658-4f63-4241-b950-dab6eb5e8391"));

            migrationBuilder.DeleteData(
                table: "UserOperationClaims",
                keyColumn: "Id",
                keyValue: new Guid("6148d16a-a58d-4dc5-a73f-ba6ead9bc49c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d22496aa-4ed9-4c1e-9fd0-06c27dbcdd79"));

            migrationBuilder.DropColumn(
                name: "SorumluIlKodu",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SorumluIlceKodu",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SorumluKutuphaneId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "Role.BakanlikYetkilisi");

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "Role.IlYetkilisi");

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Role.IlceYetkilisi");

            migrationBuilder.UpdateData(
                table: "OperationClaims",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Role.OkulKutuphaneYoneticisi");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthenticatorType", "CreatedDate", "DeletedDate", "Email", "FirstName", "LastName", "PasswordHash", "PasswordSalt", "Status", "UpdatedDate" },
                values: new object[] { new Guid("19b95d4d-46c8-4a85-bf81-a9e129a9baf3"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "narch@kodlama.io", "", "", new byte[] { 9, 164, 219, 150, 59, 254, 98, 227, 252, 207, 138, 98, 50, 88, 29, 16, 37, 228, 53, 245, 165, 228, 146, 227, 88, 62, 89, 139, 26, 207, 240, 85, 170, 90, 89, 42, 99, 146, 244, 27, 126, 204, 142, 60, 238, 188, 69, 123, 25, 0, 192, 220, 235, 195, 9, 33, 247, 175, 167, 106, 168, 155, 10, 209 }, new byte[] { 126, 125, 163, 87, 255, 136, 222, 28, 0, 0, 168, 94, 186, 99, 205, 75, 177, 160, 69, 204, 164, 222, 107, 235, 5, 79, 28, 179, 237, 234, 133, 4, 58, 119, 108, 65, 15, 83, 119, 130, 97, 136, 143, 229, 44, 37, 199, 185, 26, 233, 104, 200, 47, 149, 59, 108, 78, 55, 16, 227, 158, 78, 254, 78, 80, 60, 128, 97, 76, 40, 99, 3, 196, 105, 210, 183, 156, 17, 25, 187, 73, 74, 170, 113, 178, 192, 130, 236, 92, 37, 113, 51, 116, 187, 126, 142, 39, 24, 101, 27, 204, 66, 191, 86, 146, 254, 203, 128, 1, 185, 201, 120, 16, 27, 184, 127, 29, 145, 143, 128, 66, 176, 30, 174, 102, 104, 173, 154 }, true, null });

            migrationBuilder.InsertData(
                table: "UserOperationClaims",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "OperationClaimId", "UpdatedDate", "UserId" },
                values: new object[] { new Guid("c129f15b-e842-470f-a6c6-a4c520d6c731"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, new Guid("19b95d4d-46c8-4a85-bf81-a9e129a9baf3") });
        }
    }
}
