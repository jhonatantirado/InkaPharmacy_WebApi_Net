using FluentNHibernate.Mapping;

namespace InkaPharmacy.Api.Customers.Infrastructure.Persistence.NHibernate.Mapping
{
    using InkaPharmacy.Api.Common.Application.Enum;
    using InkaPharmacy.Api.Product;

    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Id(x => x.Id).Column("Product_id");
            Map(x => x.Name).Column("Name");
            Component(x => x.Price, m =>
            {
                m.Map(x => x.Amount, "Price");
                m.Map(x => x.Currency, "currency_Id").CustomType<Currency>();
            });
            Map(x => x.Stock).Column("Stock");
            Map(x => x.Category_id).Column("Category_id");
            Map(x => x.Lot_number).Column("Lot_number");
            Map(x => x.Sanitary_registration_number).Column("Sanitary_registration_number");
            Map(x => x.Registration_date).Column("Registration_date");
            Map(x => x.Expiration_date).Column("Expiration_date");
            Map(x => x.Status).Column("Status");
            Map(x => x.Stock_status).Column("Stock_status");
            Map(x => x.ImageUrl).Column("image_url").Nullable();
        }
    }
}
