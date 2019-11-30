using FluentMigrator;

namespace InkaPharmacy.Api.Migrations.MySQL
{
    [Migration(15)]
    public class Product_ImageUrl : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("15_Product_ImageUrl.sql");
        }

        public override void Down()
        {
        }
    }
}
