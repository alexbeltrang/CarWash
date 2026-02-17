using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Utilidades
{
    public static class FunctionsEncrip
    {
        public static string Cifrado(byte modo, string cadena)
        {
            byte[] plaintext = null;
            string VecI = "20270430";
            byte Algoritmo = 3;
            string key = "C4rW4sh";

            if (modo == 1)
                plaintext = Encoding.ASCII.GetBytes(cadena);
            else if (modo == 2)
                plaintext = Convert.FromBase64String(cadena);

            byte[] keys = Encoding.ASCII.GetBytes(key);
            MemoryStream memdata = new MemoryStream();
            ICryptoTransform transforma = null;

            switch (Algoritmo)
            {
                case 1:
                    {
                        DESCryptoServiceProvider des = new DESCryptoServiceProvider(); // DES
                        des.Mode = CipherMode.CBC;
                        if (modo == 1)
                            transforma = des.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        else if (modo == 2)
                            transforma = des.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        break;
                    }

                case 2:
                    {
                        TripleDESCryptoServiceProvider des3 = new TripleDESCryptoServiceProvider(); // TripleDES
                        des3.Mode = CipherMode.CBC;
                        if (modo == 1)
                            transforma = des3.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        else if (modo == 2)
                            transforma = des3.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        break;
                    }

                case 3:
                    {
                        RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider(); // RC2 
                        rc2.Mode = CipherMode.CBC;
                        if (modo == 1)
                            transforma = rc2.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        else if (modo == 2)
                            transforma = rc2.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        break;
                    }

                case 4:
                    {
                        RijndaelManaged rj = new RijndaelManaged(); // Rijndael 
                        rj.Mode = CipherMode.CBC;
                        if (modo == 1)
                            transforma = rj.CreateEncryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        else if (modo == 2)
                            transforma = rj.CreateDecryptor(keys, Encoding.ASCII.GetBytes(VecI));
                        break;
                    }
            }

            CryptoStream encstream = new CryptoStream(memdata, transforma, CryptoStreamMode.Write);
            encstream.Write(plaintext, 0, plaintext.Length);

            encstream.FlushFinalBlock();
            encstream.Close();


            if (modo == 1)
                cadena = Convert.ToBase64String(memdata.ToArray());
            else if (modo == 2)
                cadena = Encoding.ASCII.GetString(memdata.ToArray());

            return cadena; // Aquí es Donde se Devuelve los Datos Cifrados
        }
    }
}
