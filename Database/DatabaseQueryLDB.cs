using CarWash.Entidades;
using CarWash.ModelosRespuestas;
using CarWash.Utilidades;
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CarWash.Database
{
    public class DatabaseQueryLDB
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

        public static RespuestaUsuarioLogin Login(string username, string password)
        {
            RespuestaUsuarioLogin respuestaUsuario = new RespuestaUsuarioLogin();

            try
            {
                using (var conn = GetConnection())
                {
                    var query = new SQLiteCommand(conn);
                    query.CommandText = "select idUser,displayName,Nombres,Apellidos,PerfilId,Email from Usuarios where UserName = '" + username + "' and password = '" + password + "'";

                    var result = query.ExecuteQuery<Usuario>().FirstOrDefault();

                    if (result != null)
                    {
                        respuestaUsuario.esValido = true;
                        respuestaUsuario.respuesta = "Ok";
                        respuestaUsuario.Usuario = result;
                    }
                    else
                    {
                        respuestaUsuario.esValido = false;
                        respuestaUsuario.respuesta = "Usuario o contraseña incorrectos";
                        respuestaUsuario.Usuario = null;
                    }
                }
            }
            catch (Exception ex)
            {
                respuestaUsuario.esValido = false;
                respuestaUsuario.respuesta = ex.Message;
                respuestaUsuario.Usuario = null;
            }

            return respuestaUsuario;
        }

        public static T ExecuteScalar<T>(string queryStr, params object[] args)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    return conn.ExecuteScalar<T>(queryStr, args);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error ejecutando scalar: {ex.Message}");
            }
        }

        public static List<T> ExecuteList<T>(string queryStr, params object[] args) where T : new()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    return conn.Query<T>(queryStr, args).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error ejecutando query: {ex.Message}");
            }
        }

        public static int ExecuteNonQuery(string queryStr, params object[] args)
        {
            using (var conn = GetConnection())
            {
                return conn.Execute(queryStr, args);
            }
        }

        public static long ExecuteInsert(string queryStr, params object[] args)
        {
            using (var conn = GetConnection())
            {
                conn.Execute(queryStr, args);
                return conn.ExecuteScalar<long>("SELECT last_insert_rowid()");
            }
        }

        public static string GetDatabasePath()
        {
            return dbFile;
        }
    }
}