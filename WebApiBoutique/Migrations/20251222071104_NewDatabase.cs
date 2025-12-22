using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBoutique.Migrations
{
    /// <inheritdoc />
    public partial class NewDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmationToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtpCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    OtpGeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtpResendCount = table.Column<int>(type: "int", nullable: false),
                    LastOtpResendAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "PasswordHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordHistories_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerMeasurements",
                columns: table => new
                {
                    MeasurementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    GarmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MeasurementValue = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "inches"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMeasurements", x => x.MeasurementId);
                    table.CheckConstraint("CK_Gender", "Gender IN ('M', 'F')");
                    table.CheckConstraint("CK_MenChest", "NOT (Gender = 'M' AND MeasurementType = 'Chest' AND (MeasurementValue < 34 OR MeasurementValue > 52))");
                    table.CheckConstraint("CK_MenHips", "NOT (Gender = 'M' AND MeasurementType = 'Hips' AND (MeasurementValue < 34 OR MeasurementValue > 48))");
                    table.CheckConstraint("CK_MenNeck", "NOT (Gender = 'M' AND MeasurementType = 'Neck' AND (MeasurementValue < 14 OR MeasurementValue > 20))");
                    table.CheckConstraint("CK_MenShoulder", "NOT (Gender = 'M' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 15 OR MeasurementValue > 22))");
                    table.CheckConstraint("CK_MenSleeveLength", "NOT (Gender = 'M' AND MeasurementType = 'Sleeve Length' AND (MeasurementValue < 22 OR MeasurementValue > 27))");
                    table.CheckConstraint("CK_MenWaist", "NOT (Gender = 'M' AND MeasurementType = 'Waist' AND (MeasurementValue < 28 OR MeasurementValue > 44))");
                    table.CheckConstraint("CK_WomenBust", "NOT (Gender = 'F' AND MeasurementType = 'Bust' AND (MeasurementValue < 30 OR MeasurementValue > 46))");
                    table.CheckConstraint("CK_WomenHips", "NOT (Gender = 'F' AND MeasurementType = 'Hips' AND (MeasurementValue < 32 OR MeasurementValue > 48))");
                    table.CheckConstraint("CK_WomenShoulder", "NOT (Gender = 'F' AND MeasurementType = 'Shoulder' AND (MeasurementValue < 13 OR MeasurementValue > 19))");
                    table.CheckConstraint("CK_WomenUpperArm", "NOT (Gender = 'F' AND MeasurementType = 'Upper Arm' AND (MeasurementValue < 10 OR MeasurementValue > 16))");
                    table.CheckConstraint("CK_WomenWaist", "NOT (Gender = 'F' AND MeasurementType = 'Waist' AND (MeasurementValue < 24 OR MeasurementValue > 40))");
                    table.ForeignKey(
                        name: "FK_CustomerMeasurements_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStatuses",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdvanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatuses", x => x.DeliveryId);
                    table.ForeignKey(
                        name: "FK_DeliveryStatuses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    MeasurementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    FabricImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FabricColor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.MeasurementId);
                    table.ForeignKey(
                        name: "FK_Measurements_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurements_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blazer_Women",
                columns: table => new
                {
                    BlazerWomenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Bust = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Armhole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BlazerLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BackWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LapelDepth = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blazer_Women", x => x.BlazerWomenId);
                    table.ForeignKey(
                        name: "FK_Blazer_Women_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blazer_Women_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Frocks",
                columns: table => new
                {
                    FrockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Bust = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Armhole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckDepthFront = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckDepthBack = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FrockLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlareWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YokeDepth = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frocks", x => x.FrockId);
                    table.ForeignKey(
                        name: "FK_Frocks_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Frocks_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KurtaMen",
                columns: table => new
                {
                    KurtaMenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Chest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Armhole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KurtaLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckDepth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SideSlitHeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurtaMen", x => x.KurtaMenId);
                    table.ForeignKey(
                        name: "FK_KurtaMen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KurtaMen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KurtiWomen",
                columns: table => new
                {
                    KurtiWomenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Bust = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Armhole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KurtiLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckDepthFront = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckDepthBack = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderToBust = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderToWaist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SideSlitHeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KurtiWomen", x => x.KurtiWomenId);
                    table.ForeignKey(
                        name: "FK_KurtiWomen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KurtiWomen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    MeasurementId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId");
                });

            migrationBuilder.CreateTable(
                name: "PantMen",
                columns: table => new
                {
                    PantMenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Thigh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Knee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Calf = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BottomOpening = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CrotchDepth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlyLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantMen", x => x.PantMenId);
                    table.ForeignKey(
                        name: "FK_PantMen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PantMen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayjamaMen",
                columns: table => new
                {
                    PayjamaMenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Thigh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Knee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Calf = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BottomOpening = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CrotchDepth = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayjamaMen", x => x.PayjamaMenId);
                    table.ForeignKey(
                        name: "FK_PayjamaMen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayjamaMen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayjamaWomen",
                columns: table => new
                {
                    PayjamaWomenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    WaistCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HipCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThighCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KneeCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CalfCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BottomOpening = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutseamLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CrotchDepth = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayjamaWomen", x => x.PayjamaWomenId);
                    table.ForeignKey(
                        name: "FK_PayjamaWomen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayjamaWomen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShirtMen",
                columns: table => new
                {
                    ShirtMenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Chest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Armhole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShirtLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CuffCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BackWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShirtMen", x => x.ShirtMenId);
                    table.ForeignKey(
                        name: "FK_ShirtMen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShirtMen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopWomen",
                columns: table => new
                {
                    TopWomenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Bust = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShoulderWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Armhole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckDepth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TopLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopWomen", x => x.TopWomenId);
                    table.ForeignKey(
                        name: "FK_TopWomen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopWomen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TShirtMen",
                columns: table => new
                {
                    TShirtMenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Chest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShoulderWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Armhole = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SleeveCircumference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TShirtLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NeckWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TShirtMen", x => x.TShirtMenId);
                    table.ForeignKey(
                        name: "FK_TShirtMen_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TShirtMen_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_Email",
                table: "ApplicationUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blazer_Women_MeasurementId",
                table: "Blazer_Women",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Blazer_Women_TypeId",
                table: "Blazer_Women",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMeasurements_CustomerId_Gender",
                table: "CustomerMeasurements",
                columns: new[] { "CustomerId", "Gender" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatuses_CustomerId",
                table: "DeliveryStatuses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Frocks_MeasurementId",
                table: "Frocks",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Frocks_TypeId",
                table: "Frocks",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KurtaMen_MeasurementId",
                table: "KurtaMen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_KurtaMen_TypeId",
                table: "KurtaMen",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KurtiWomen_MeasurementId",
                table: "KurtiWomen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_KurtiWomen_TypeId",
                table: "KurtiWomen",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_CustomerId",
                table: "Measurements",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_TypeId",
                table: "Measurements",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MeasurementId",
                table: "Orders",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_PantMen_MeasurementId",
                table: "PantMen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_PantMen_TypeId",
                table: "PantMen",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistories_UserId",
                table: "PasswordHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PayjamaMen_MeasurementId",
                table: "PayjamaMen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_PayjamaMen_TypeId",
                table: "PayjamaMen",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayjamaWomen_MeasurementId",
                table: "PayjamaWomen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_PayjamaWomen_TypeId",
                table: "PayjamaWomen",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CustomerId",
                table: "Payments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_ShirtMen_MeasurementId",
                table: "ShirtMen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_ShirtMen_TypeId",
                table: "ShirtMen",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TopWomen_MeasurementId",
                table: "TopWomen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_TopWomen_TypeId",
                table: "TopWomen",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TShirtMen_MeasurementId",
                table: "TShirtMen",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_TShirtMen_TypeId",
                table: "TShirtMen",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blazer_Women");

            migrationBuilder.DropTable(
                name: "CustomerMeasurements");

            migrationBuilder.DropTable(
                name: "DeliveryStatuses");

            migrationBuilder.DropTable(
                name: "Frocks");

            migrationBuilder.DropTable(
                name: "KurtaMen");

            migrationBuilder.DropTable(
                name: "KurtiWomen");

            migrationBuilder.DropTable(
                name: "PantMen");

            migrationBuilder.DropTable(
                name: "PasswordHistories");

            migrationBuilder.DropTable(
                name: "PayjamaMen");

            migrationBuilder.DropTable(
                name: "PayjamaWomen");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ShirtMen");

            migrationBuilder.DropTable(
                name: "TopWomen");

            migrationBuilder.DropTable(
                name: "TShirtMen");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
