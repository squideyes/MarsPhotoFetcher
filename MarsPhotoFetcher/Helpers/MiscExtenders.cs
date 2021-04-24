using System;
using System.Collections.Generic;

namespace MarsPhotoFetcher
{
    public static class MiscExtenders
    {
        public static Camera ToCamera(this string value)
        {
            return value switch
            {
                "CHEMCAM" => Camera.ChemistryandCameraComplex,
                "FHAZ" => Camera.FrontHazardAvoidanceCamera,
                "MARDI" => Camera.MarsDescentImager,
                "MAHLI" => Camera.MarsHandLensImager,
                "MAST" => Camera.MastCamera,
                "MINITES" => Camera.MiniatureThermalEmissionSpectrometer,
                "NAVCAM" => Camera.NavigationCamera,
                "PANCAM" => Camera.PanoramicCamera,
                "RHAZ" => Camera.RearHazardAvoidanceCamera,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        public static string ToCode(this Camera camera)
        {
            return camera switch
            {
                Camera.ChemistryandCameraComplex => "CHEMCAM",
                Camera.FrontHazardAvoidanceCamera => "FHAZ",
                Camera.MarsDescentImager => "MARDI",
                Camera.MarsHandLensImager => "MAHLI",
                Camera.MastCamera => "MAST",
                Camera.MiniatureThermalEmissionSpectrometer => "MINITES",
                Camera.NavigationCamera => "NAVCAM",
                Camera.PanoramicCamera => "PANCAM",
                Camera.RearHazardAvoidanceCamera => "RHAZ",
                _ => throw new ArgumentOutOfRangeException(nameof(camera))
            };
        }

        public static List<Rover> ToRovers(this Camera camera)
        {
            static List<Rover> GetRovers(bool curiosity, bool opportunity, bool spirit)
            {
                var rovers = new List<Rover>();

                if (curiosity)
                    rovers.Add(Rover.Curiosity);

                if (opportunity)
                    rovers.Add(Rover.Opportunity);

                if (spirit)
                    rovers.Add(Rover.Spirit);

                return rovers;
            }

            return camera switch
            {
                Camera.ChemistryandCameraComplex => GetRovers(true, false, false),
                Camera.FrontHazardAvoidanceCamera => GetRovers(true, true, true),
                Camera.MarsDescentImager => GetRovers(true, false, false),
                Camera.MarsHandLensImager => GetRovers(true, false, false),
                Camera.MastCamera => GetRovers(true, false, false),
                Camera.MiniatureThermalEmissionSpectrometer => GetRovers(false, true, true),
                Camera.NavigationCamera => GetRovers(true, true, true),
                Camera.PanoramicCamera => GetRovers(false, true, true),
                Camera.RearHazardAvoidanceCamera => GetRovers(true, true, true),
                _ => throw new ArgumentOutOfRangeException(nameof(camera))
            };
        }
    }
}
