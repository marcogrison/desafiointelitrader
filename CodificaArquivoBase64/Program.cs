using System;
using System.IO;

namespace Base64EncoderDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao codificador/decodificador Base64!");
            Console.WriteLine("Digite o caminho completo do arquivo que deseja codificar ou decodificar:");
            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                Console.WriteLine("Escolha a opção:\n1. Codificar para Base64\n2. Decodificar de Base64");
                int option = Convert.ToInt32(Console.ReadLine());

                if (option == 1)
                {
                    EncodeBase64(filePath);
                }
                else if (option == 2)
                {
                    DecodeBase64(filePath);
                }
                else
                {
                    Console.WriteLine("Opção inválida!");
                }
            }
            else
            {
                Console.WriteLine("Arquivo não encontrado. Certifique-se de que o caminho está correto.");
            }
        }

        private static void EncodeBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            string base64String = ConvertToBase64String(fileBytes);
            string newFilePath = Path.ChangeExtension(filePath, ".base64");
            File.WriteAllText(newFilePath, base64String);
            Console.WriteLine($"Arquivo codificado em Base64 salvo em: {newFilePath}");
        }

        private static void DecodeBase64(string filePath)
        {
            string base64String = File.ReadAllText(filePath);
            byte[] fileBytes = ConvertFromBase64String(base64String);
            string newFilePath = Path.ChangeExtension(filePath, ".decoded");
            File.WriteAllBytes(newFilePath, fileBytes);
            Console.WriteLine($"Arquivo decodificado salvo em: {newFilePath}");
        }

        private static string ConvertToBase64String(byte[] inputBytes)
        {
            const string base64Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            string base64String = "";
            int paddingCount = inputBytes.Length % 3;

            for (int i = 0; i < inputBytes.Length; i += 3)
            {
                int byte1 = inputBytes[i];
                int byte2 = (i + 1 < inputBytes.Length) ? inputBytes[i + 1] : 0;
                int byte3 = (i + 2 < inputBytes.Length) ? inputBytes[i + 2] : 0;

                int index1 = byte1 >> 2;
                int index2 = ((byte1 & 0x03) << 4) | (byte2 >> 4);
                int index3 = ((byte2 & 0x0F) << 2) | (byte3 >> 6);
                int index4 = byte3 & 0x3F;

                base64String += base64Characters[index1];
                base64String += base64Characters[index2];
                base64String += base64Characters[index3];
                base64String += base64Characters[index4];
            }

            switch (paddingCount)
            {
                case 1:
                    base64String = base64String.Remove(base64String.Length - 1) + "=";
                    break;
                case 2:
                    base64String = base64String.Remove(base64String.Length - 2) + "==";
                    break;
            }

            return base64String;
        }

        private static byte[] ConvertFromBase64String(string base64String)
        {
            const string base64Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

            string paddedBase64String = base64String.PadRight(base64String.Length + (4 - base64String.Length % 4) % 4, '=');
            byte[] outputBytes = new byte[paddedBase64String.Length * 3 / 4];

            for (int i = 0, j = 0; i < paddedBase64String.Length; i += 4, j += 3)
            {
                int index1 = base64Characters.IndexOf(paddedBase64String[i]);
                int index2 = base64Characters.IndexOf(paddedBase64String[i + 1]);
                int index3 = base64Characters.IndexOf(paddedBase64String[i + 2]);
                int index4 = base64Characters.IndexOf(paddedBase64String[i + 3]);

                int byte1 = (index1 << 2) | (index2 >> 4);
                int byte2 = ((index2 & 0x0F) << 4) | (index3 >> 2);
                int byte3 = ((index3 & 0x03) << 6) | index4;

                outputBytes[j] = (byte)byte1;
                if (index3 < 64)
                {
                    outputBytes[j + 1] = (byte)byte2;
                    if (index4 < 64)
                    {
                        outputBytes[j + 2] = (byte)byte3;
                    }
                }
            }

            return outputBytes;
        }
    }
}