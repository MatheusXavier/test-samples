namespace Product.API.Domain.Entities;

public class Product
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public double Value { get; private set; }
    public bool Active { get; private set; }

    public Product(Guid id, string name, double value, bool active)
    {
        Id = id;
        Name = name;
        Value = value;
        Active = active;
    }

    public void Update(string name, double value)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (name.Length < 4)
        {
            throw new ArgumentException($"'{nameof(name)}' must be more than 3 characters.", nameof(name));
        }

        if (value <= 0)
        {
            throw new ArgumentException($"'{nameof(value)}' cannot be less or equal to zero", nameof(value));
        }

        Name = name;
        Value = value;
    }
}