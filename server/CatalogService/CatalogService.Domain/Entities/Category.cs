

namespace CatalogService.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Category() { }

        public static Category Create(
        string name,
        string description,
        string imageUrl = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty");

            return new Category
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Update(string name, string description, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");

            Name = name;
            Description = description;
            ImageUrl = imageUrl;
        }

        //hide from customers - soft delete : instead of deleting from db,
        //hide from customers so that deletion won't affect linked products
        public void Deactivate() => IsActive = false;

        //show to customers
        public void Activate() => IsActive = true;
    }
}
