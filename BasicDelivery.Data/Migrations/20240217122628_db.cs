using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicDelivery.Data.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    driverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    passwordHash = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    salt = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    lastLogin = table.Column<DateTime>(type: "datetime", nullable: true),
                    reviewRate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.driverId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeProduct = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    nameProduct = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    imagesProduct = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productVolume = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    passwordHash = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    salt = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    lastLogin = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "DriverDetail",
                columns: table => new
                {
                    driverDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    driverId = table.Column<int>(type: "int", nullable: false),
                    licenseNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    vehicleModel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    font = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    back = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverDetail", x => x.driverDetailId);
                    table.ForeignKey(
                        name: "FK__DriverDeta__back__29572725",
                        column: x => x.driverId,
                        principalTable: "Drivers",
                        principalColumn: "driverId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    driverId = table.Column<int>(type: "int", nullable: false),
                    receiverAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    receiverDistrict = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    receiverWard = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    receiverName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    receiverPhone = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    shipCost = table.Column<int>(type: "int", nullable: true),
                    totalMoney = table.Column<int>(type: "int", nullable: true),
                    paymentMethod = table.Column<bool>(type: "bit", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    driverAcceptAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    completeAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    userNote = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    deliveryNote = table.Column<int>(type: "int", nullable: true),
                    totalGamPackage = table.Column<int>(type: "int", nullable: true),
                    imagesPackages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    widePackage = table.Column<int>(type: "int", nullable: true),
                    heightPackage = table.Column<int>(type: "int", nullable: true),
                    longPackage = table.Column<int>(type: "int", nullable: true),
                    totalPriceProduct = table.Column<int>(type: "int", nullable: true),
                    totalCod = table.Column<int>(type: "int", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.orderId);
                    table.ForeignKey(
                        name: "FK__Orders__driverId__2D27B809",
                        column: x => x.driverId,
                        principalTable: "Drivers",
                        principalColumn: "driverId");
                    table.ForeignKey(
                        name: "FK__Orders__totalCod__2C3393D0",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    historyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: true),
                    orderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    changeDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.historyId);
                    table.ForeignKey(
                        name: "FK__Histories__chang__36B12243",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "orderId");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    orderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    imagesProduct = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gam = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.orderDetailId);
                    table.ForeignKey(
                        name: "FK__OrderDeta__order__33D4B598",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "orderId");
                    table.ForeignKey(
                        name: "FK__OrderDeta__produ__32E0915F",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "productId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverDetail_driverId",
                table: "DriverDetail",
                column: "driverId");

            migrationBuilder.CreateIndex(
                name: "UQ__Drivers__AB6E61642760FB05",
                table: "Drivers",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_orderId",
                table: "Histories",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_orderId",
                table: "OrderDetail",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_productId",
                table: "OrderDetail",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_driverId",
                table: "Orders",
                column: "driverId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_userId",
                table: "Orders",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "UQ__Products__3AE90233EE0111D0",
                table: "Products",
                column: "nameProduct",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__AB6E616421824B57",
                table: "Users",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverDetail");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
