using CarWash.Entidades;
using CarWash.ModelosRespuestas;
using CarWash.Utilidades;
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Database
{
    public class DatabaseQueryLDB
    {
        private static string NameDatabase = ConfigurationManager.AppSettings["NameLDB"];
        private static string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameDatabase);

        private static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(dbFile);
        }

        //public static ControlActualizacionAgente obtenerDatosControlActualizacionAgenteLocal(string idCliente)
        //{
        //    ControlActualizacionAgente controlActualizacion = new ControlActualizacionAgente();

        //    try
        //    {
        //        using (SQLiteConnection conn = new SQLiteConnection(dbFile))
        //        {
        //            var query = new SQLiteCommand(conn);

        //            string fechaActual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //            query.CommandText = $@" SELECT IdConsec, id, idCliente, DescripCambiosUpgrade, FechaVigenciaUpgrade, TieneUpgrade, VersionActual, UrlDescargaZip
        //                                    FROM ControlActualizacionAgente
        //                                    WHERE idCliente = '{idCliente}'
        //                                    AND strftime('%Y-%m-%d %H:%M:%S', FechaVigenciaUpgrade / 10000000 - 62135596800, 'unixepoch') >= '{fechaActual}'";

        //            var result = query.ExecuteQuery<ControlActualizacionAgente>().ToList();
        //            controlActualizacion = result.FirstOrDefault();
        //            return controlActualizacion;
        //        }
        //    }
        //    catch
        //    {
        //        return controlActualizacion;
        //    }
        //}


        //public static Maquinas obtenerIdClienteLocal(string MACUser)
        //{
        //    //Parametrizacion.CertificateClient(Global.IdCliente);
        //    Maquinas Of = new Maquinas();

        //    try
        //    {
        //        using (SQLiteConnection conn = new SQLiteConnection(dbFile))
        //        {
        //            var query = new SQLiteCommand(conn);
        //            query.CommandText = "SELECT IdConsec,id,idCliente,macEquipo,nombreEquipo,ipEquipo,idTipoMaquina,serialBiometrico,idEstadoRegistro, estadoRegistro, idOficina, identificadorOficina, idEstadoRegistro  " +
        //            "FROM Maquinas " +
        //            "WHERE  (idEstadoRegistro = 1 and macEquipo = '" + MACUser + "')";
        //            var result = query.ExecuteQuery<Maquinas>().ToList();
        //            Of = result.FirstOrDefault();
        //            return Of;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}


        public static RespuestaUsuarioLogin Login(string username, string password)
        {
            RespuestaUsuarioLogin respuestaUsuario = new RespuestaUsuarioLogin();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbFile))
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
            using (var conn = new SQLiteConnection(dbFile))
            {
                return conn.Execute(queryStr, args);
            }
        }

    }
}
