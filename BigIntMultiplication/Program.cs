using System;

namespace MultiplicacaoBigInt
{
    class Program
    {
        static string SomarStrings(string num1, string num2)
        {
            int carry = 0;
            int tamanhoMaximo = Math.Max(num1.Length, num2.Length);
            char[] resultado = new char[tamanhoMaximo + 1];

            for (int i = 0; i < tamanhoMaximo || carry > 0; i++)
            {
                int digito1 = i < num1.Length ? num1[num1.Length - 1 - i] - '0' : 0;
                int digito2 = i < num2.Length ? num2[num2.Length - 1 - i] - '0' : 0;

                int soma = digito1 + digito2 + carry;
                carry = soma / 10;
                resultado[resultado.Length - 1 - i] = (char)(soma % 10 + '0');
            }

            return new string(resultado).TrimStart('0');
        }

        static string MultiplicarStrings(string num1, string num2)
        {
            if (num1 == "0" || num2 == "0")
                return "0";

            string resultado = "0";

            for (int i = num1.Length - 1; i >= 0; i--)
            {
                int carry = 0;
                int[] produtos = new int[num1.Length + num2.Length];

                for (int j = num2.Length - 1; j >= 0; j--)
                {
                    int produto = (num1[i] - '0') * (num2[j] - '0') + carry;
                    carry = produto / 10;
                    produtos[i + j + 1] = produto % 10;
                }

                produtos[i] = carry;

                string resultadoParcial = new string(produtos.Select(digito => (char)(digito + '0')).ToArray());
                resultadoParcial = resultadoParcial.TrimStart('0');
                if (resultadoParcial == "")
                    resultadoParcial = "0";

                resultado = SomarStrings(resultado, resultadoParcial + new string('0', num1.Length - 1 - i));
            }

            return resultado;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Digite o primeiro número:");
            string num1 = Console.ReadLine();

            Console.WriteLine("Digite o segundo número:");
            string num2 = Console.ReadLine();

            string resultado = MultiplicarStrings(num1, num2);
            Console.WriteLine($"O resultado da multiplicação é: {resultado}");
        }
    }
}
