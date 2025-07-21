using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models;

public class UrlMapping
{
    public int Id { get; set; }

    [Required]
    [Url]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(10, MinimumLength = 4, ErrorMessage = "ShortCode must be between 4 and 10 characters.")]
    public string ShortCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.Date)]
    public DateTime? ExpiresAt { get; set; } // ✅ Campo opcional de expiración
}
