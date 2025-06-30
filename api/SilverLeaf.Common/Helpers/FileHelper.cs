using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Helpers
{
    public static class FileHelper
    {
        public static async Task DownloadFile(HttpClient client, string fileUrl, string storageLocation)
        {
            try
            {
                if (!Directory.Exists(storageLocation))
                {
                    Directory.CreateDirectory(storageLocation);
                }
                if (File.Exists($@"{storageLocation}\{Path.GetFileName(fileUrl)}"))
                {
                    Console.WriteLine($@"{Path.GetFileName(fileUrl)} already exists. Skipping...");
                    return;
                }

                using (var result = await client.GetAsync(fileUrl))
                {
                    Console.WriteLine($@"Attempting to download file from {fileUrl}");
                    if (result.IsSuccessStatusCode)
                    {
                        var imageAsBytes = await result.Content.ReadAsByteArrayAsync();
                        Console.WriteLine($@"Download successful. Writing File... {storageLocation}\{ Path.GetFileName(fileUrl)}");
                        File.WriteAllBytes($@"{storageLocation}\{Path.GetFileName(fileUrl)}", imageAsBytes);
                    }
                    else
                    {
                        Console.WriteLine($@"Cannot access {fileUrl} aborting...");
                    }
                }
            }
            catch
            {
                Console.WriteLine($@"Couldn't download in {client.Timeout.TotalSeconds} seconds, aborting...");
            }
        }
    }

}

