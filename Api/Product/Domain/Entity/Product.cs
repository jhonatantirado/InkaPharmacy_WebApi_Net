using InkaPharmacy.Api.Common.Application;
using InkaPharmacy.Api.Common.Domain.ValueObject;
using System;

namespace InkaPharmacy.Api.Product
{
    public class Product
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Money Price { get; set; }
        public virtual int Stock { get; set; }
        public virtual long Category_id { get; set; }
        public virtual string Lot_number { get; set; }
        public virtual string Sanitary_registration_number { get; set; }
        public virtual DateTime Registration_date { get; set; }
        public virtual DateTime Expiration_date { get; set; }
        public virtual int Status { get; set; }
        public virtual int Stock_status { get; set; }

        public virtual string ImageUrl { get; set; }

        public Product()
        {
        }

        public virtual Notification ValidateFindByProductName(string ProductName)
        {
            Notification notification = new Notification();

            if (string.IsNullOrEmpty(ProductName))
            {
                notification.AddError("The Product Name is null");
            }

            return notification;
        }

        public virtual Notification ValidateFindByCategory(long Category_id)
        {
            Notification notification = new Notification();

            if (Category_id== 0)
            {
                notification.AddError("The Category is null");
            }

            return notification;
        }

        public virtual Notification ValidateGetProductById(long ProductId)
        {
            Notification notification = new Notification();

            if (ProductId == 0)
            {
                notification.AddError("The Product Id is null");
            }

            return notification;
        }

        public virtual Notification ValidateDeleteProduct(Product product)
        {
            Notification notification = new Notification();

            if (product == null || product.Id == 0)
            {
                notification.AddError("The Product is null");
            }

            return notification;
        }


        public virtual Notification ValidateForSave(string action = "")
        {
            Notification notification = new Notification();

            if (this == null)
            {
                notification.AddError("The Product is null");
            }

            if (action == "U")
            {
                if (this.Id == 0)
                {
                    notification.AddError("The Product doesn´t have a valid Id");
                }
            }


            if (string.IsNullOrEmpty(this.Name))
            {
                notification.AddError("The Product doesn´t have a valid Name");
            }

            if (this.Price == null)
            {
                notification.AddError("The Product doesn´t have a valid Price");
            }

            if (this.Stock == 0)
            {
                notification.AddError("The Product doesn´t have a valid Stock");
            }

            if (this.Category_id == 0)
            {
                notification.AddError("The Product doesn´t have a valid Category");
            }

            return notification;
        }
    }
}
