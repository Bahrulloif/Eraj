using System.Net;
using System.Text;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly DataContext _context;

    public FileService(IWebHostEnvironment hostEnvironment,DataContext context)
    {
        _hostEnvironment = hostEnvironment;
        _context = context;
    }

    public Response<string> CreateFile(IFormFile file)
    {
        try
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            using var readStream = file.OpenReadStream();
            if (!IsAllowedImage(extension, readStream))
            {
                // Previously accepted (and publicly served, via app.UseStaticFiles()) literally
                // any file the caller named with any extension - no content check at all. An
                // .html file uploaded through a product-image field got stored and served back
                // with Content-Type: text/html, script tag and all: stored XSS on the site's own
                // origin, reachable by any Businessman (the lowest role allowed to add products
                // with images), confirmed live. Extension alone isn't enough either - a renamed
                // evil.html wearing a .png extension would still pass an extension-only check, so
                // this also checks the real file signature. .svg is deliberately not in the
                // allow-list even though it's a legitimate image format: SVG is XML and can carry
                // <script>, served as image/svg+xml with script execution on direct navigation -
                // the same vector this is closing.
                return new Response<string>(HttpStatusCode.BadRequest,
                    "Only PNG, JPEG, GIF or WEBP images are allowed.");
            }
            readStream.Position = 0;

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath= Path.Combine(_hostEnvironment.WebRootPath,"Images",fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                readStream.CopyTo(stream);
            }

            return new Response<string>(fileName);
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    // Checks the claimed extension against the file's actual leading bytes, not just its name.
    private static bool IsAllowedImage(string extension, Stream stream)
    {
        if (stream.Length == 0)
        {
            return false;
        }
        var header = new byte[12];
        var read = stream.Read(header, 0, header.Length);
        stream.Position = 0;

        return extension switch
        {
            ".jpg" or ".jpeg" => read >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
            ".png" => read >= 8 && header.AsSpan(0, 8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }),
            ".gif" => read >= 6 && (Encoding.ASCII.GetString(header, 0, 6) == "GIF87a" || Encoding.ASCII.GetString(header, 0, 6) == "GIF89a"),
            ".webp" => read >= 12 && Encoding.ASCII.GetString(header, 0, 4) == "RIFF" && Encoding.ASCII.GetString(header, 8, 4) == "WEBP",
            _ => false
        };
    }

    public Response<bool> DeleteFile(string file)
    {
        try
        {
            var fullPath = Path.Combine(_hostEnvironment.WebRootPath, "Images", file);
            File.Delete(fullPath);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}