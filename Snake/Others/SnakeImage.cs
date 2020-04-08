using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Snake.Others
{
    internal static class SnakeImage
    {
        public static readonly List<Uri> ImageSourceList = ConfigurationManager.AppSettings.AllKeys
            .Where(key => key.StartsWith("img"))
            .Select(key => ConfigurationManager.AppSettings[key])
            .ToList().ConvertAll(x => new Uri(x));
    }
}