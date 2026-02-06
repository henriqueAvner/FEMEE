using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace FEMEE.Infrastructure.Security
{
    public class PasswordHasher
    {
        private const int MinimumWorkFactor = 12;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), "Senha não pode ser nula ou vazia");


            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Erro ao gerar o hash da senha.", ex);
            }

        }

        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), "Senha não pode ser nula ou vazia");


            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentNullException(nameof(hash), "Hash não pode ser nulo ou vazio");

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Verifica se o hash precisa ser recalculado (ex.: work factor menor que o desejado).
        /// BCrypt.Net-Next não expõe GetWorkFactor; o cost é extraído do hash (ex.: $2a$10$... → 10).
        /// </summary>
        public static bool NeedsRehash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                return true;

            try
            {
                // Formato do hash BCrypt: $2a$XX$salt+hash (XX = work factor, 2 dígitos)
                var parts = hash.Split('$');
                if (parts.Length < 3 || parts[2].Length < 2)
                    return true;

                if (!int.TryParse(parts[2].AsSpan(0, 2), out var workFactor))
                    return true;

                return workFactor < MinimumWorkFactor;
            }
            catch
            {
                return true;
            }
        }
    }
}
