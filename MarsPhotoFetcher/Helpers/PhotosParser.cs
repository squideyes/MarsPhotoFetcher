using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MarsPhotoFetcher
{
    public static class PhotosParser
    {
        private class Root
        {
            public PhotoInfo[] Photos { get; init; }
        }

        private class PhotoInfo
        {
            public int Id { get; init; }
            public int Sol { get; init; }
            public CameraInfo Camera { get; init; }
            public string Img_Src { get; init; }
            public string Earth_Date { get; init; }
        }

        private class CameraInfo
        {
            public string Name { get; init; }
        }

        public static List<Photo> GetPhotos(Rover rover, DateTime earthDate, string json)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            var root = JsonSerializer.Deserialize<Root>(json, options);

            var photos = new List<Photo>();

            foreach (var photoInfo in root.Photos)
            {
                photos.Add(new Photo()
                {
                    PhotoId = photoInfo.Id,
                    Rover = rover,
                    Sol = photoInfo.Sol,
                    EarthDate = earthDate,
                    ImageUri = new Uri(photoInfo.Img_Src),
                    Camera = photoInfo.Camera.Name.ToCamera()
                });
            }

            return photos;
        }
    }
}
