using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    public partial class product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerRequest",
                columns: table => new
                {
                    productid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    senddate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRequest", x => x.productid);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    productid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productcategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    suppliername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recieveddate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRequest");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
