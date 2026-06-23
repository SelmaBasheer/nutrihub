using CatalogService.Domain.Entities;
using MongoDB.Driver;
using Serilog;

namespace CatalogService.Infrastructure.Persistence;

public class DataSeeder
{
    private readonly CatalogDbContext _context;
    private readonly ILogger _logger = Log.ForContext<DataSeeder>();

    public DataSeeder(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedCategoriesAsync();
        await SeedProductsAsync();
    }

    private async Task SeedCategoriesAsync()
    {
        var count = await _context.Categories.CountDocumentsAsync(_ => true);
        if (count > 0)
        {
            _logger.Information("Categories already seeded — skipping");
            return;
        }

        var categories = new List<Category>
        {
            Category.Create("Vitamins & Supplements", "Essential vitamins and dietary supplements", "https://example.com/vitamins.jpg"),
            Category.Create("Proteins & Fitness", "Protein powders and fitness nutrition", "https://example.com/proteins.jpg"),
            Category.Create("Organic & Natural Foods", "Certified organic and natural food products", "https://example.com/organic.jpg"),
            Category.Create("Herbal & Ayurvedic", "Traditional herbal and ayurvedic products", "https://example.com/herbal.jpg")
        };

        await _context.Categories.InsertManyAsync(categories);
        _logger.Information("Seeded {Count} categories", categories.Count);
    }

    private async Task SeedProductsAsync()
    {
        var count = await _context.Products.CountDocumentsAsync(_ => true);
        if (count > 0)
        {
            _logger.Information("Products already seeded — skipping");
            return;
        }

        var vitaminsId = (await _context.Categories
            .Find(c => c.Name == "Vitamins & Supplements")
            .FirstOrDefaultAsync()).Id;

        var proteinsId = (await _context.Categories
            .Find(c => c.Name == "Proteins & Fitness")
            .FirstOrDefaultAsync()).Id;

        var organicId = (await _context.Categories
            .Find(c => c.Name == "Organic & Natural Foods")
            .FirstOrDefaultAsync()).Id;

        var herbalId = (await _context.Categories
            .Find(c => c.Name == "Herbal & Ayurvedic")
            .FirstOrDefaultAsync()).Id;

        var products = new List<Product>
        {
            // Vitamins & Supplements
            Product.Create("Vitamin C 1000mg", "High potency Vitamin C for immune support", 12.99m, 100, vitaminsId, new List<string> { "https://example.com/vitc.jpg" }),
            Product.Create("Vitamin D3 5000IU", "Supports bone health and immunity", 14.99m, 80, vitaminsId, new List<string> { "https://example.com/vitd.jpg" }),
            Product.Create("Omega-3 Fish Oil", "Premium fish oil for heart and brain health", 19.99m, 60, vitaminsId, new List<string> { "https://example.com/omega3.jpg" }),
            Product.Create("Multivitamin Daily", "Complete daily multivitamin for adults", 24.99m, 120, vitaminsId, new List<string> { "https://example.com/multi.jpg" }),
            Product.Create("Zinc 50mg", "Essential mineral for immune function", 9.99m, 150, vitaminsId, new List<string> { "https://example.com/zinc.jpg" }),

            // Proteins & Fitness
            Product.Create("Whey Protein Chocolate", "Premium whey protein isolate 2kg", 49.99m, 50, proteinsId, new List<string> { "https://example.com/whey-choc.jpg" }),
            Product.Create("Whey Protein Vanilla", "Premium whey protein isolate 2kg", 49.99m, 50, proteinsId, new List<string> { "https://example.com/whey-van.jpg" }),
            Product.Create("Plant Protein Powder", "Vegan pea and rice protein blend", 44.99m, 40, proteinsId, new List<string> { "https://example.com/plant.jpg" }),
            Product.Create("BCAA Energy Drink", "Branched chain amino acids with caffeine", 29.99m, 70, proteinsId, new List<string> { "https://example.com/bcaa.jpg" }),
            Product.Create("Creatine Monohydrate", "Pure creatine for strength and power", 22.99m, 90, proteinsId, new List<string> { "https://example.com/creatine.jpg" }),

            // Organic & Natural Foods
            Product.Create("Organic Chia Seeds", "Raw organic chia seeds 500g", 8.99m, 200, organicId, new List<string> { "https://example.com/chia.jpg" }),
            Product.Create("Organic Quinoa", "Premium white quinoa 1kg", 11.99m, 150, organicId, new List<string> { "https://example.com/quinoa.jpg" }),
            Product.Create("Raw Honey", "Pure unfiltered raw honey 500g", 13.99m, 100, organicId, new List<string> { "https://example.com/honey.jpg" }),
            Product.Create("Coconut Oil Extra Virgin", "Cold pressed extra virgin coconut oil", 15.99m, 80, organicId, new List<string> { "https://example.com/coconut.jpg" }),
            Product.Create("Almond Butter", "Natural creamy almond butter 350g", 12.99m, 90, organicId, new List<string> { "https://example.com/almond.jpg" }),

            // Herbal & Ayurvedic
            Product.Create("Ashwagandha Root Extract", "Adaptogen for stress relief and energy", 18.99m, 110, herbalId, new List<string> { "https://example.com/ashwa.jpg" }),
            Product.Create("Turmeric Curcumin", "Anti-inflammatory turmeric with black pepper", 16.99m, 130, herbalId, new List<string> { "https://example.com/turmeric.jpg" }),
            Product.Create("Moringa Leaf Powder", "Superfood rich in nutrients and antioxidants", 14.99m, 95, herbalId, new List<string> { "https://example.com/moringa.jpg" }),
            Product.Create("Triphala Tablets", "Traditional ayurvedic digestive formula", 11.99m, 120, herbalId, new List<string> { "https://example.com/triphala.jpg" }),
            Product.Create("Green Tea Extract", "High potency green tea with EGCG", 13.99m, 140, herbalId, new List<string> { "https://example.com/greentea.jpg" })
        };

        await _context.Products.InsertManyAsync(products);
        _logger.Information("Seeded {Count} products", products.Count);
    }
}