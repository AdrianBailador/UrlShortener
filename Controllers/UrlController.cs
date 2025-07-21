using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Models;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Controllers;

public class UrlController : Controller
{
    private readonly AppDbContext _context;

    public UrlController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index() => View();


[HttpPost]
public async Task<IActionResult> Shorten(string originalUrl, string? customCode, DateTime? expiresAt)
{
    if (!Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
    {
        TempData["Error"] = "Invalid URL.";
        return RedirectToAction("Index");
    }

    if (expiresAt.HasValue && expiresAt.Value < DateTime.UtcNow)
    {
        TempData["Error"] = "Expiration date must be in the future.";
        return RedirectToAction("Index");
    }

    var existing = await _context.UrlMappings
        .FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl);

    if (existing != null)
    {
        var existingUrl = $"{Request.Scheme}://{Request.Host}/{existing.ShortCode}";
        TempData["ShortUrl"] = existingUrl;
        return RedirectToAction("Index");
    }

    string shortCode;

    if (!string.IsNullOrWhiteSpace(customCode))
    {
        var trimmedCode = customCode.Trim();

        if (!System.Text.RegularExpressions.Regex.IsMatch(trimmedCode, @"^[a-zA-Z0-9_-]+$"))
        {
            TempData["Error"] = "The custom code contains invalid characters. Use only letters, numbers, '-' or '_'.";
            return RedirectToAction("Index");
        }

        bool exists = await _context.UrlMappings.AnyAsync(x => x.ShortCode == trimmedCode);
        if (exists)
        {
            TempData["Error"] = "This code is already in use.";
            return RedirectToAction("Index");
        }

        shortCode = trimmedCode;
    }
    else
    {
        do
        {
            shortCode = Guid.NewGuid().ToString("N")[..6];
        }
        while (await _context.UrlMappings.AnyAsync(x => x.ShortCode == shortCode));
    }

    var mapping = new UrlMapping
    {
        OriginalUrl = originalUrl,
        ShortCode = shortCode,
        ExpiresAt = expiresAt
    };

    _context.UrlMappings.Add(mapping);
    await _context.SaveChangesAsync();

    var host = Request.Host.HasValue ? Request.Host.Value : "localhost:5098";
    var fullShortUrl = $"{Request.Scheme}://{host}/{shortCode}";
    TempData["ShortUrl"] = fullShortUrl;

    return RedirectToAction("Index");
}



   [HttpGet("/{code}")]
public async Task<IActionResult> RedirectToOriginal(string code)
{
    var mapping = await _context.UrlMappings
        .FirstOrDefaultAsync(x => x.ShortCode == code);

    if (mapping == null)
        return NotFound();

    if (mapping.ExpiresAt.HasValue && mapping.ExpiresAt.Value < DateTime.UtcNow)
        return BadRequest("This URL has expired.");

    var uri = new Uri(mapping.OriginalUrl);
    if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        return BadRequest("Invalid redirection.");

    return Redirect(mapping.OriginalUrl);
}

}
