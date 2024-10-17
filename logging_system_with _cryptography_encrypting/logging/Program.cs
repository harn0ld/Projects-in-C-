// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;  
using System.Text;   
using System;
using System.IO;
class Program{
    static void Main(string[] args){
        int commandType;
        if (!int.TryParse(args[0], out commandType))
        {
            Console.WriteLine("Niepoprawny typ polecenia. Podaj 0, 1 lub 2");
            return;
        }

        switch (commandType)
        {
            case 0:
                gneratekeys();
                break;
            case 1:
                if (args.Length != 3)
                {
                    Console.WriteLine("Podaj nazwę plików (a) oraz (b)");
                    return;
                }
                EncryptText(args[1], args[2]);
                break;
            case 2:
                if (args.Length != 3)
                {
                    Console.WriteLine("Podaj nazwę plików (a) oraz (b)");
                    return;
                }
                DecryptData(args[1], args[2]);
                break;
            default:
                Console.WriteLine("Niepoprawny typ polecenia. Podaj 0, 1 lub 2");
                break;
        }
    }


    static void gneratekeys(){
        using (var rsa = new RSACryptoServiceProvider(2048)) {   
            var publicKey = rsa.ToXmlString(false);
            File.WriteAllText("publicKey.xml", publicKey);


            var privateKey = rsa.ToXmlString(true);
            File.WriteAllText("privateKey.xml", privateKey);
        }
    }
 static void EncryptText(string tekst,string nazwaPliku)  
    {  
        string privateKeyText = File.ReadAllText("publicKey.xml");
        UnicodeEncoding byteConverter = new UnicodeEncoding();  
        byte[] daneDoZaszyfrowania = byteConverter.GetBytes(tekst);  

        byte[] zaszyfrowaneDane;   
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())  
        {  

            rsa.FromXmlString(privateKeyText);  

            zaszyfrowaneDane = rsa.Encrypt(daneDoZaszyfrowania, false);   
        }  
        File.WriteAllBytes(nazwaPliku, zaszyfrowaneDane);  

        Console.WriteLine("Dane zostały zaszyfrowane");   
    }  
    static string DecryptData(string privateKey,string fileName)  
    {  

        byte[] daneDoOdszyfrowania = File.ReadAllBytes(fileName);  


        byte[] odszyfrowaneDane;  
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())  
        {  

            rsa.FromXmlString(privateKey);  
            odszyfrowaneDane = rsa.Decrypt(daneDoOdszyfrowania, false);   
        }  

        UnicodeEncoding byteConverter = new UnicodeEncoding();  
        return byteConverter.GetString(odszyfrowaneDane);   
    }  
    static String skrotSHA256(String napis)
        {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = SHA256.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(napis));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }

    static String skrotSHA512(String napis)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = SHA512.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(napis));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }

    static String skrotMD5(String napis)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = MD5.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(napis));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }
            
                
        
}

