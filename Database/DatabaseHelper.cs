using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CarWash.Database
{
    public class DatabaseHelper
    {
        private static string NameDatabase = ConfigurationManager.AppSettings["NameLDB"];

        private static string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "SplashCar"
        );

        private static string dbFile = Path.Combine(folder, NameDatabase);

        public static SQLiteConnection GetConnection()
        {
            Directory.CreateDirectory(folder);
            return new SQLiteConnection(dbFile);
        }

        public static bool Insert<T>(T item)
        {
            using (var conn = GetConnection())
            {
                conn.CreateTable<T>();
                return conn.Insert(item) > 0;
            }
        }

        public static bool Insert<T>(T item, out T objetoaux)
        {
            using (var conn = GetConnection())
            {
                conn.CreateTable<T>();
                int rows = conn.Insert(item);

                objetoaux = rows > 0 ? item : default(T);
                return rows > 0;
            }
        }

        public static bool Create<T>()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.CreateTable<T>();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateOrUpdateTable<T>()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var mapping = conn.GetMapping<T>();
                    string tableName = mapping.TableName;

                    conn.CreateTable<T>();

                    var existingColumns = conn.Query<ColumnInfo>(
                        $"PRAGMA table_info({tableName});");

                    var existingColumnNames = existingColumns
                        .Select(c => c.name)
                        .ToList();

                    foreach (var column in mapping.Columns)
                    {
                        if (!existingColumnNames.Contains(column.Name))
                        {
                            string sqlType = GetSQLiteType(column.ColumnType);

                            string alterQuery =
                                $"ALTER TABLE {tableName} ADD COLUMN {column.Name} {sqlType};";

                            conn.Execute(alterQuery);
                        }
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Drop<T>()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.DropTable<T>();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Update<T>(T item)
        {
            using (var conn = GetConnection())
            {
                conn.CreateTable<T>();
                return conn.Update(item) > 0;
            }
        }

        public static bool Delete<T>(T item)
        {
            using (var conn = GetConnection())
            {
                conn.CreateTable<T>();
                return conn.Delete(item) > 0;
            }
        }

        public static List<T> Read<T>() where T : new()
        {
            using (var conn = GetConnection())
            {
                conn.CreateTable<T>();
                return conn.Table<T>().ToList();
            }
        }

        public static List<T> Read<T>(T item) where T : new()
        {
            using (var conn = GetConnection())
            {
                conn.CreateTable<T>();
                return conn.Table<T>().ToList();
            }
        }

        public static List<T> ReadList<T>() where T : new()
        {
            using (var conn = GetConnection())
            {
                conn.CreateTable<T>();
                return conn.Table<T>().ToList();
            }
        }

        private static string GetSQLiteType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(int)) return "INTEGER";
            if (type == typeof(long)) return "INTEGER";
            if (type == typeof(bool)) return "INTEGER";
            if (type == typeof(string)) return "TEXT";
            if (type == typeof(double)) return "REAL";
            if (type == typeof(float)) return "REAL";
            if (type == typeof(DateTime)) return "TEXT";

            return "TEXT";
        }
    }
}