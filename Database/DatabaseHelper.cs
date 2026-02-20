using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLiteConnection;

namespace CarWash.Database
{
    public class DatabaseHelper
    {
        private static string NameDatabase = ConfigurationManager.AppSettings["NameLDB"];
        private static string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameDatabase);

        // Generamos a generic method 
        public static bool Insert<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Insert(item);
                if (rows > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        public static bool Insert<T>(T item, out T objetoaux)
        {
            bool result = false;
            T objeto = default(T);
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Insert(item);
                if (rows > 0)
                {
                    objeto = item;
                    result = true;

                }
            }
            objetoaux = objeto;
            return result;
        }

        public static bool Create<T>()
        {
            bool result = false;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                try
                {
                    conn.CreateTable<T>();
                    result = true;
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                    result = false;
                }
            }
            return result;
        }


        public static bool CreateOrUpdateTable<T>()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                try
                {
                    var mapping = conn.GetMapping<T>();
                    string tableName = mapping.TableName;

                    // 1️⃣ Crear tabla si no existe
                    conn.CreateTable<T>();

                    // 2️⃣ Obtener columnas existentes
                    var existingColumns = conn.Query<ColumnInfo>(
                        $"PRAGMA table_info({tableName});");

                    var existingColumnNames = existingColumns
                        .Select(c => c.name)
                        .ToList();

                    // 3️⃣ Comparar con propiedades del modelo
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
                catch (Exception ex)
                {
                    string error = ex.Message;
                    return false;
                }
            }
        }


        public static bool Drop<T>()
        {
            bool result = false;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                try
                {
                    conn.DropTable<T>();
                    result = true;
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                    result = false;
                }
            }
            return result;
        }
        public bool Insert1<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Insert(item);
                if (rows > 0)
                {
                    result = true;
                }
                conn.Dispose();
            }
            return result;
        }
        public static bool Update<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Update(item);
                if (rows > 0)
                {
                    result = true;
                }
                conn.Dispose();
            }
            return result;
        }
        public static bool Delete<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                int rows = conn.Delete(item);
                if (rows > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        // where T:new() decimos que T puede ser lo que sea y va a tener un parametro 
        public static List<T> Read<T>(T item) where T : new()
        {
            List<T> items;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                items = conn.Table<T>().ToList();

            }
            return items;
        }

        public static List<T> Read<T>() where T : new()
        {
            List<T> items;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                items = conn.Table<T>().ToList();

            }
            return items;
        }



        public List<T> ReadList<T>(T item) where T : new()
        {
            List<T> items;
            using (SQLiteConnection conn = new SQLiteConnection(dbFile))
            {
                conn.CreateTable<T>();
                items = conn.Table<T>().ToList();

            }
            return items;
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
