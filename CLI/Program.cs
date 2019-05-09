using Library;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CLI
{
    class Program
    {
        static readonly Patcher patcher = new Patcher();
        static AppConfig cfg = ConfigManager.AppConfig;

        static void Main(string[] args)
        {
            Console.WriteLine($"Welcome to the FIFA Launcher Patcher CLI");
            Console.WriteLine($"CLI Version: {Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            Console.WriteLine($"Library Version: {FileVersionInfo.GetVersionInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.dll")).FileVersion}\n");

            if (args.ToList().Contains("config")) ConfigMode();
            else
            {
                bool success = patcher.Patch(cfg, Patcher.MessageType.CLI);

                Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(success ? "Patching successful" : "Patching unsuccessful");
                Console.ForegroundColor = ConsoleColor.White;
            }

            PrintConfig(cfg);

            Console.Write("\nPress any key to continue . . .");
            Console.ReadKey();
        }

        static void ConfigMode()
        {
            Console.WriteLine("Config Mode:");
            AppConfig _cfg = new AppConfig();

            Console.WriteLine($"Gamepath (Press Enter to keep current path) ({cfg.path})");
            _cfg.path = Console.ReadLine();
            if (_cfg.path.Trim() == "") _cfg.path = cfg.path;
            Console.CursorTop -= 1;
            Console.WriteLine(_cfg.path);

            _cfg.skipGameLauncher = GetBool("\nSkip Game Launcher", cfg.skipGameLauncher);
            _cfg.skipLanguageSelection = GetBool("\nSkip Language Selection", cfg.skipLanguageSelection);
            _cfg.forceMetricUnits = GetBool("\nForce Metric Units", cfg.forceMetricUnits);

            cfg = _cfg;
            patcher.Patch(_cfg, Patcher.MessageType.CLI);
        }

        private static void PrintConfig(AppConfig cfg)
        {
            Console.WriteLine("\nConfiguration:");
            Console.WriteLine("Path: " + cfg.path);
            Console.WriteLine("Skip Game Launcher: " + cfg.skipGameLauncher);
            Console.WriteLine("Skip Language Selection: " + cfg.skipLanguageSelection);
            Console.WriteLine("Force Metric Units: " + cfg.forceMetricUnits);
        }

        private static bool GetBool(string label, bool defaulValue = true)
        {
            string selection = defaulValue ? "[Y/n]" : "[y/N]";

            Console.WriteLine($"{label} {selection}");
            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.Y:
                        Console.WriteLine("Y");
                        return true;
                    case ConsoleKey.N:
                        Console.WriteLine("N");
                        return false;
                    case ConsoleKey.Enter:
                        Console.WriteLine(defaulValue ? "Y" : "N");
                        return defaulValue;
                    default:
                        break;
                }
            }
        }
    }
}
