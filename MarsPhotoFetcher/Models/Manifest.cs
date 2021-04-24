using System;
using System.Collections.Generic;

namespace MarsPhotoFetcher
{
    public class Manifest
    {
        public List<Camera> Cameras { get; init; }
        public DateTime EarthDate { get; init; }
        public int Sol { get; init; }
        public int TotalPhotos { get; init; }
    }
}
