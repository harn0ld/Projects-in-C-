// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Użycie: program.exe [nazwa_pliku_a] [nazwa_pliku_b] [SHA256 | SHA512 | MD5]");
            return;
        }

        string inputFile = args[0];
        string hashFile = args[1];
        string algorithm = args[2];

        if (!File.Exists(inputFile))
        {
            Console.WriteLine("Plik wejściowy nie istnieje.");
            return;
        }

        if (!IsValidAlgorithm(algorithm))
        {
            Console.WriteLine("Niepoprawny algorytm. Wybierz: SHA256, SHA512 lub MD5.");
            return;
        }

        try
        {
            if (!File.Exists(hashFile))
            {
                Console.WriteLine("Obliczanie hasha...");
                File.WriteAllBytes(hashFile, CalculateHash(inputFile, algorithm));
                Console.WriteLine("Hash został obliczony i zapisany.");
            }
            else
            {
                Console.WriteLine("Weryfikowanie hasha...");
                bool isValid = VerifyHash(inputFile, hashFile, algorithm);
                Console.WriteLine(isValid ? "Hash jest zgodny." : "Hash nie jest zgodny.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        }
    }

    static bool IsValidAlgorithm(string algorithm)
    {
        return algorithm.Equals("SHA256", StringComparison.OrdinalIgnoreCase) ||
               algorithm.Equals("SHA512", StringComparison.OrdinalIgnoreCase) ||
               algorithm.Equals("MD5", StringComparison.OrdinalIgnoreCase);
    }

    static byte[] CalculateHash(string inputFile, string algorithm)
    {
        using (var hashAlgorithm = GetHashAlgorithm(algorithm))
        {
            return hashAlgorithm.ComputeHash(File.ReadAllBytes(inputFile));
        }
    }

    static bool VerifyHash(string inputFile, string hashFile, string algorithm)
    {
        byte[] computedHash = CalculateHash(inputFile, algorithm);
        byte[] storedHash = File.ReadAllBytes(hashFile);
        return CompareHashes(computedHash, storedHash);
    }

    static HashAlgorithm GetHashAlgorithm(string algorithm)
    {
        switch (algorithm.ToUpper())
        {
            case "SHA256":
                return SHA256.Create();
            case "SHA512":
                return SHA512.Create();
            case "MD5":
                return MD5.Create();
            default:
                throw new ArgumentException("Niepoprawny algorytm.");
        }
    }

    static bool CompareHashes(byte[] hash1, byte[] hash2)
    {
        if (hash1.Length != hash2.Length)
        {
            return false;
        }
        for (int i = 0; i < hash1.Length; i++)
        {
            if (hash1[i] != hash2[i])
            {
                return false;
            }
        }
        return true;
    }
}

