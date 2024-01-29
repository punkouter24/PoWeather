using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PoWeather.Services;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<List<string>> GetLocationsAsync(string userId)
    {
        // Assuming each user's locations are stored in a blob named "{userId}_locations"
        var blobContent = await GetBlobAsync("userlocations", $"{userId}_locations");
        if (blobContent == null)
        {
            return new List<string>();
        }

        using (var sr = new StreamReader(blobContent))
        {
            var content = await sr.ReadToEndAsync();
            return content.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(p => p.Trim())
                          .ToList();
        }
    }

    public async Task SaveLocationsAsync(string userId, List<string> locations)
    {
        var blobContent = string.Join(",", locations);
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent)))
        {
            await UploadBlobAsync("userlocations", $"{userId}_locations", ms);
        }
    }


    public async Task UploadBlobAsync(string containerName, string blobName, Stream content)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = "text/plain" });
    }

    public async Task<Stream> GetBlobAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        if (await blobClient.ExistsAsync())
        {
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }
        return null;
    }



    // Add methods for other operations like delete, list blobs, etc.
}

