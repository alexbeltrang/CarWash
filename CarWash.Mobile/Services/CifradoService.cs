using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace CarWash.Mobile.Services
{
public static class CifradoService
{
    private static readonly byte[] Key = Encoding.ASCII.GetBytes("C4rW4sh");
    private static readonly byte[] IV = Encoding.ASCII.GetBytes("20270430");

    public static string Cifrar(string texto)
    {
        byte[] plaintext = Encoding.ASCII.GetBytes(texto);

        var engine = new RC2Engine();
        var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(engine));
        var keyParam = new ParametersWithIV(new RC2Parameters(Key), IV);

        cipher.Init(true, keyParam);

        byte[] output = new byte[cipher.GetOutputSize(plaintext.Length)];
        int len = cipher.ProcessBytes(plaintext, 0, plaintext.Length, output, 0);
        cipher.DoFinal(output, len);

        return Convert.ToBase64String(output);
    }
}
}


