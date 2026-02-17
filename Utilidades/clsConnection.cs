using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Utilidades
{
    class clsConnection
    {
        public static bool blnVentanasEnbebidas = false;
        public static bool blnAyudaEnlinea = false;
        public static bool blnCargarProd = false;
        public static bool seleccionaUsuarios = false;

        public static int idUser;
        public static int intCodigoPerfil;
        public static int intCodigoProducto;


        public static decimal decFactorConversion;

        public static string strNombreUsuario;
        public static string strEmailUsuario;
        public static string strNombreProducto;
        public static string strCodigoVentaProducto;
        public static string strReferenciaProducto;
        public static string strNombreProdBuscar;

        String mstrConnectionString;  /* '= System.Configuration.ConfigurationSettings.AppSettings("db")*/
        String mstrNameProcedure;
        SqlCommand mcmTemp = new SqlCommand();

        public enum TipoDato
        {
            Table = 1,
            View = 2,
            DataSet = 3,
            DataReader = 4
        }
        public string ConnectionString
        {
            get
            {
                return mstrConnectionString;
            }
            set
            {
                mstrConnectionString = value;
            }
        }

        public bool IsNumeric(string text)
        {
            double test;
            return double.TryParse(text, out test);
        }

        public string NameProcedure
        {
            get
            {
                return mstrNameProcedure;
            }
            set
            {
                mstrNameProcedure = value;
            }
        }

        public int Query(string strSQL)
        {
            SqlConnection cnConexion = new SqlConnection(mstrConnectionString);
            SqlCommand cmComando = new SqlCommand(strSQL, cnConexion);
            cnConexion.Open();
            return cmComando.ExecuteNonQuery();
            cnConexion.Close();
            cnConexion.Dispose();
            cmComando.Dispose();
            GC.Collect();
        }

        public object Query(string strSQL, TipoDato eTipoDato)
        {
            SqlConnection cnConexion = new SqlConnection(mstrConnectionString);
            SqlCommand cmComando = new SqlCommand(strSQL, cnConexion);
            SqlDataAdapter dtaAdaptador = new SqlDataAdapter();
            DataSet dtsTemp = new DataSet();
            DataTable dttTemp = new DataTable();
            DataView dtvTemp = new DataView();
            SqlDataReader dtrTemp;
            cnConexion.Open();
            try
            {
                cmComando.CommandType = CommandType.Text;
                dtaAdaptador.SelectCommand = cmComando;
                switch (eTipoDato)
                {
                    case TipoDato.DataSet:
                        dtaAdaptador.Fill(dtsTemp, "Temp");
                        return (dtsTemp);
                    case TipoDato.Table:
                        dtaAdaptador.Fill(dttTemp);
                        return dttTemp;

                    case TipoDato.View:
                        dtaAdaptador.Fill(dttTemp);
                        dttTemp.TableName = "Temp";
                        dtvTemp.Table = dttTemp;
                        return dtvTemp;

                    case TipoDato.DataReader:
                        dtrTemp = cmComando.ExecuteReader();
                        return dtrTemp;

                }
            }
            catch (Exception e)
            {
                return e;
            }
            return dttTemp;
        }

        public void ExecuteProc()
        {
            SqlConnection cnTemp = new SqlConnection(mstrConnectionString);
            mcmTemp.Connection = cnTemp;
            cnTemp.Open();
            mcmTemp.CommandText = mstrNameProcedure;
            mcmTemp.CommandType = CommandType.StoredProcedure;
            mcmTemp.CommandTimeout = 0;
            mcmTemp.ExecuteNonQuery();
            cnTemp.Close();
            cnTemp.Dispose();
        }
        public object ExecuteProc(TipoDato eTipoDato)
        {
            SqlDataAdapter dtaTemp = new SqlDataAdapter();
            DataTable dttTemp = new DataTable();
            DataSet dtsTemp = new DataSet();
            DataView dtvTemp = new DataView();
            SqlDataReader dtrTemp;
            SqlConnection cnTemp = new SqlConnection(mstrConnectionString);
            try
            {
                mcmTemp.Connection = cnTemp;
                mcmTemp.CommandTimeout = 0;
                cnTemp.Open();
                mcmTemp.CommandText = mstrNameProcedure;
                mcmTemp.CommandType = CommandType.StoredProcedure;
                dtaTemp.SelectCommand = mcmTemp;
                switch (eTipoDato)
                {
                    case TipoDato.DataSet:
                        dtaTemp.Fill(dtsTemp, "Temp");
                        return (dtsTemp);
                    case TipoDato.Table:
                        dtaTemp.Fill(dttTemp);
                        return dttTemp;
                    case TipoDato.View:
                        dtaTemp.Fill(dttTemp);
                        dttTemp.TableName = "Temp";
                        dtvTemp.Table = dttTemp;
                        return dtvTemp;
                    case TipoDato.DataReader:
                        dtrTemp = mcmTemp.ExecuteReader();
                        return dtrTemp;

                }


            }
            catch (Exception e)
            {
                return e;
            }
            return dttTemp;
        }


        public string ParametersOut(string strNameParameter)
        {
            return Convert.ToString(mcmTemp.Parameters[strNameParameter].Value);
        }
        public string ParametersOut(int intIndex)
        {
            return Convert.ToString(mcmTemp.Parameters[intIndex].Value);
        }


        public void Parameters(string strNameParameter, string strValor)
        {
            mcmTemp.Parameters.AddWithValue(strNameParameter, strValor);
            //mcmTemp.Parameters[strNameParameter].Value = strValor;
        }
        public void SetParametersOut(string strNameParameter, string strValor)
        {
            SqlParameter outParam = new SqlParameter();
            outParam.SqlDbType = System.Data.SqlDbType.Int;
            outParam.ParameterName = strNameParameter;
            outParam.Value = strValor;
            outParam.Direction = System.Data.ParameterDirection.Output;
            mcmTemp.Parameters.Add(outParam);

        }
        public void Parameters(string strNameParameter, object objValor)
        {
            try
            {
                mcmTemp.Parameters.AddWithValue(strNameParameter, objValor);
            }
            catch
            {
            }
        }

        public void StoredProcedure()
        {
            DataTable dttParameters = new DataTable();
            SqlConnection cnCon = new SqlConnection(mstrConnectionString);
            SqlCommand cmCom = new SqlCommand("sp_sproc_columns", cnCon);
            SqlDataAdapter dtaAd = new SqlDataAdapter();
            int intContador;
            mcmTemp = new SqlCommand();
            cnCon.Open();
            cmCom.CommandType = CommandType.StoredProcedure;
            cmCom.Parameters.Add(new SqlParameter("@procedure_name", SqlDbType.NVarChar, 390));
            cmCom.Parameters["@procedure_name"].Value = mstrNameProcedure;
            dtaAd.SelectCommand = cmCom;
            dtaAd.Fill(dttParameters);
            for (intContador = 0; intContador < (dttParameters.Rows.Count - 1); intContador++)
            {
                switch (dttParameters.Rows[intContador]["COLUMN_TYPE"].ToString())
                {
                    case "1":
                        mcmTemp.Parameters.Add(dttParameters.Rows[intContador]["COLUMN_NAME"].ToString(), TipoDatoBD(dttParameters.Rows[intContador]["TYPE_NAME"].ToString()), Convert.ToInt16(dttParameters.Rows[intContador]["PRECISION"].ToString()));
                        break;
                    case "2":
                        mcmTemp.Parameters.Add(dttParameters.Rows[intContador]["COLUMN_NAME"].ToString(), TipoDatoBD(dttParameters.Rows[intContador]["TYPE_NAME"].ToString()), Convert.ToInt16(dttParameters.Rows[intContador]["PRECISION"].ToString()));
                        mcmTemp.Parameters[Convert.ToString(dttParameters.Rows[intContador]["COLUMN_NAME"].ToString())].Direction = ParameterDirection.Output;
                        break;
                }
            }
        }

        public SqlDbType TipoDatoBD(String strNameType)
        {
            switch (strNameType)
            {
                case "bigint":
                    return SqlDbType.BigInt;
                case "binary":
                    return SqlDbType.Binary;
                case "bit":
                    return SqlDbType.Bit;
                case "char":
                    return SqlDbType.Char;
                case "datetime":
                    return SqlDbType.DateTime;
                case "numeric":
                    return SqlDbType.Decimal;
                case "decimal":
                    return SqlDbType.Decimal;
                case "float":
                    return SqlDbType.Float;
                case "image":
                    return SqlDbType.Image;
                case "int":
                    return SqlDbType.Int;
                case "money":
                    return SqlDbType.Money;
                case "nchar":
                    return SqlDbType.NChar;
                case "ntext":
                    return SqlDbType.NText;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "real":
                    return SqlDbType.Real;
                case "smalldatetime":
                    return SqlDbType.SmallDateTime;
                case "smallint":
                    return SqlDbType.SmallInt;
                case "smallmoney":
                    return SqlDbType.SmallMoney;
                case "text":
                    return SqlDbType.Text;
                case "timestamp":
                    return SqlDbType.Timestamp;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                case "varbinary":
                    return SqlDbType.VarBinary;
                case "varchar":
                    return SqlDbType.VarChar;
            }
            return SqlDbType.Variant;
        }

        public void DisposeProc()
        {
            mcmTemp.Dispose();
        }


    }
}
