using System;
using System.IO;

namespace Library
{
    public static class Paths
    {
        public static string DEFAULT_GAMEPATH = @"C:\Program Files (x86)\Origin Games\FIFA 19";
        public static string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory, "config.json");
    }
}
