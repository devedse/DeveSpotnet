using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeveSpotnet.Migrations.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpotHeaders",
                columns: table => new
                {
                    ArticleNumber = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Subject = table.Column<string>(type: "TEXT", nullable: true),
                    From = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    MessageID = table.Column<string>(type: "TEXT", nullable: true),
                    References = table.Column<string>(type: "TEXT", nullable: true),
                    Bytes = table.Column<int>(type: "INTEGER", nullable: false),
                    Lines = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_Valid = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParsedHeader_Header = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_SelfSignedPubKey = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_UserSignature = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_Verified = table.Column<bool>(type: "INTEGER", nullable: true),
                    ParsedHeader_FileSize = table.Column<int>(type: "INTEGER", nullable: true),
                    ParsedHeader_MessageId = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_Stamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ParsedHeader_Poster = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_Category = table.Column<int>(type: "INTEGER", nullable: true),
                    ParsedHeader_KeyId = table.Column<int>(type: "INTEGER", nullable: true),
                    ParsedHeader_SubCatA = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_SubCatB = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_SubCatC = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_SubCatD = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_SubCatZ = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_WasSigned = table.Column<bool>(type: "INTEGER", nullable: true),
                    ParsedHeader_SpotterId = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_Title = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_Tag = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_HeaderSign = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_UserKey_Modulo = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_UserKey_Exponent = table.Column<string>(type: "TEXT", nullable: true),
                    ParsedHeader_XmlSignature = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotHeaders", x => x.ArticleNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotHeaders");
        }
    }
}
