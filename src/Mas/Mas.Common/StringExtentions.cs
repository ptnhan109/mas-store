using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Mas.Common
{
    public static class StringExtentions
    {
        public static string ToHashedString(this string input, string secret)
        {
            var Hashed = KeyDerivation.Pbkdf2(
                    password: input,
                    salt: Encoding.UTF8.GetBytes(secret),
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                    );
            return Convert.ToBase64String(Hashed);
        }

        public static bool IsCompare(this string input, string hashed, string secret)
        {
            return input.ToHashedString(secret).Equals(hashed);
        }

        public static string ToCurrencyFormat(this double input)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN"); 
            return input.ToString("#,###", cul.NumberFormat);
        }

        public static string GenerateCode(this string prefix,int length)
        {
            var random = new Random();
            var result = new StringBuilder(prefix.ToUpper());
            for(int i = 0; i < length; i++)
            {
                result.Append(random.Next(1, 9));
            }

            return result.ToString();
        }

    }
}
