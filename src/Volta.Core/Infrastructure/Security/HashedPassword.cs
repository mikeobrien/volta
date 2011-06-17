using System;
using System.Security.Cryptography;
using System.Text;

namespace Volta.Core.Infrastructure.Security
{
    public class HashedPassword
    {
        private readonly string _hash;
        private readonly string _salt;

        private HashedPassword(string salt, string hash)
        {
            _hash = hash;
            _salt = salt;
        }

        public static HashedPassword FromHash(string hash)
        {
            return new HashedPassword(hash.Substring(0, 32), hash);
        }

        public static HashedPassword Create(string password)
        {
            return Create(Guid.NewGuid().ToString("N"), password);
        }

        private static HashedPassword Create(string salt, string password)
        {
            var hash = salt + new SHA256Managed().ComputeHash(Encoding.Unicode.GetBytes(salt + password)).ToHex();
            return new HashedPassword(salt, hash);
        }

        public bool MatchesPassword(string password)
        {
            return Create(_salt, password).ToString() == _hash;
        }

        public override string ToString()
        {
            return _hash;
        }
    }
}