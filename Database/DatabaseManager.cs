using System;
using System.Configuration;
using System.IO;
using SQLite;

namespace CarWash.Database
{
    public static class DatabaseManager
    {
        private static string databaseName = ConfigurationManager.AppSettings["NameLDB"];

        private static string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "SplashCar"
        );

        private static string dbPath = Path.Combine(folder, databaseName);

        static DatabaseManager()
        {
            // Crear carpeta si no existe
            Directory.CreateDirectory(folder);
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(dbPath);
        }

        public static string GetDatabasePath()
        {
            return dbPath;
        }
    }
}
