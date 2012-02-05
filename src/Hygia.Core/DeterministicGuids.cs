namespace Hygia
{
    using System;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;

    public static class DeterministicGuids
    {
        [DebuggerNonUserCode]
        public static Guid ToGuid(this string str)
        {
            //use MD5 hash to get a 16-byte hash of the string
            var provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(str);
            byte[] hashBytes = provider.ComputeHash(inputBytes);
            //generate a guid from the hash:
            return new Guid(hashBytes);

        }
    }
}