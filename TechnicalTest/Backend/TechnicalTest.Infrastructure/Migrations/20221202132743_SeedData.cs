using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Rates (\"From\", \"To\", \"Rate\") \r\nVALUES ('EUR', 'USD', 1.359), ('CAD', 'EUR', 0.732), ('USD', 'EUR', 0.736), ('EUR', 'CAD', 1.366)");
            migrationBuilder.Sql("INSERT INTO Transactions (Sku, Amount, Currency) VALUES ('T2006', 10.00, 'USD'), ('M2007', 34.59, 'CAD'), ('R2008', 17.95, 'USD'), ('T2006', 7.63, 'EUR'), ('B2009', 21.23, 'USD')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
