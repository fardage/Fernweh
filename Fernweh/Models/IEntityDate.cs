using System;
namespace Fernweh.Models
{
    public interface IEntityDate
    {
        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
