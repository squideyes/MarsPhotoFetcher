using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MarsPhotoFetcher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .Build();

            var api = new ApiHelper(config["NasaApiKey"]);

            // TODO: Get photos for all three rovers
            var manifests = await api.GetManifests(Rover.Curiosity);

            var photos = new List<Photo>();

            foreach (var manifest in manifests)
            {
                if (manifest.TotalPhotos == 0)
                    continue;

                if (manifest.EarthDate.Year != 2012)
                    continue;

                photos.AddRange(await api.GetPhotos(
                    Rover.Curiosity, manifest.EarthDate));

                Console.WriteLine($"{manifest.EarthDate:MM/dd/yy} - Enqueued {manifest.TotalPhotos:N0} Photos");
            }

            var fetcher = new ActionBlock<Photo>(
                async photo =>
                {
                    var saveTo = GetSaveTo(Rover.Curiosity, "", photo);

                    var folder = Path.GetDirectoryName(saveTo);

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var photoStream = await client.GetStreamAsync(photo.ImageUri);

                    using var fileStream = File.Create(saveTo);

                    await photoStream.CopyToAsync(fileStream);

                    Console.WriteLine($"FETCHED {saveTo}");
                },
                new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });

            photos.ForEach(photo => fetcher.Post(photo));

            fetcher.Complete();

            await fetcher.Completion;
        }

        private static string GetSaveTo(Rover rover, string basePath, Photo photo)
        {
            return Path.Combine(basePath, "Photos", rover.ToString(), 
                photo.Camera.ToCode(), GetFileName(photo));
        }

        private static string GetFileName(Photo photo)
        {
            var sb = new StringBuilder();

            sb.Append(photo.Rover.ToString().ToUpper());
            sb.Append('-');
            sb.Append(photo.EarthDate.ToString("yyyyMMdd"));
            sb.Append('-');
            sb.Append(photo.Camera.ToCode());
            sb.Append('-');
            sb.Append(photo.PhotoId.ToString("00000000"));
            sb.Append(Path.GetExtension(photo.ImageUri.AbsoluteUri));

            return sb.ToString();
        }
    }
}
