using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.DTOs
{
    public record CategoryDto
    (
        Guid Id,
        string Name,
        string Description, 
        string ImageUrl,
        bool IsActive,
        DateTime CreatedAt
    );
}
