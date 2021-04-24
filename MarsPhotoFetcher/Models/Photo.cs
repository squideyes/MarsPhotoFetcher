using System;

namespace MarsPhotoFetcher
{
    public class Photo
    {
        public int PhotoId { get; init; }
        public Rover Rover { get; init; }
        public DateTime EarthDate { get; init; }
        public int Sol { get; init; }
        public Uri ImageUri { get; init; }
        public Camera Camera { get; init; }
    }
}
