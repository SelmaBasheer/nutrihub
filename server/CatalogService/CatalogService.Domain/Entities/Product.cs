

namespace CatalogService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public Guid CategoryId { get; private set; }  // reference to Category
        public List<string> ImageUrls { get; private set; } = new();
        public bool IsAvailable { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Product() { }

        public static Product Create(
        string name,
        string description,
        decimal price,
        int stock,
        Guid categoryId,
        List<string>? imageUrls = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty");
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero");
            if (stock < 0)
                throw new ArgumentException("Stock cannot be negative");
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Category is required");

            return new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                CategoryId = categoryId,
                ImageUrls = imageUrls ?? new List<string>(),
                IsAvailable = stock > 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void Update(
        string name,
        string description,
        decimal price,
        Guid categoryId,
        List<string> imageUrls)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero");
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Category is required");

            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            ImageUrls = imageUrls;
            UpdatedAt = DateTime.UtcNow;
        }

        // add single image
        public void AddImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL cannot be empty");
            ImageUrls.Add(imageUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        // Remove specific image
        public void RemoveImage(string imageUrl)
        {
            ImageUrls.Remove(imageUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStock(int quantity)
        {
            if (Stock + quantity < 0)
                throw new InvalidOperationException("Insufficient stock");

            Stock = Stock + quantity;
            IsAvailable = Stock > 0;
            UpdatedAt = DateTime.UtcNow;
        }
    }
    }
