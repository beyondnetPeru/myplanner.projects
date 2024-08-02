using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyPlanner.Shared.Domain.ValueObjects;

namespace MyPlanner.Products.Domain
{

    public class ProductProps : IProps
    {
        public IdValueObject Id { get; set; }
        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public Audit Audit { get; private set; }

        public ProductStatus Status { get; private set; }

        public ProductProps(IdValueObject id, Name name)
        {
            Id = id;
            Name = name;
            Description = Description.DefaultValue;
            Audit = Audit.Create("default");
            Status = ProductStatus.Active;
        }

        public object Clone()
        {
            return new ProductProps(Id, Name)
            {
                Description = Description,
                Audit = Audit
            };
        }
    }

    public class Product : Entity<Product, ProductProps>
    {
        private Product(ProductProps props) : base(props)
        {
        }

        public static Product Create(IdValueObject id, Name name)
        {
            return new Product(new ProductProps(id, name));
        }

        public void ChangeName(Name name)
        {
            var props = GetProps();

            props.Name.SetValue(name.GetValue());
            props.Audit.Update("default");

            SetProps(props);
        }

        public void ChangeDescription(Description description)
        {
            var props = GetProps();

            props.Description.SetValue(description.GetValue());
            props.Audit.Update("default");

            SetProps(props);
        }

        public void ChangeStatus(ProductStatus status)
        {
            var props = GetProps();

            props.Status.SetValue<ProductStatus>(status.Id);
            props.Audit.Update("default");

            SetProps(props);
        }
    }

    public class ProductStatus : Enumeration
    {
        public static ProductStatus Active = new ProductStatus(1, "Active");
        public static ProductStatus Inactive = new ProductStatus(2, "Inactive");

        public ProductStatus(int id, string name) : base(id, name)
        {
        }
    }
}
