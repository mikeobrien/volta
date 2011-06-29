using System.Linq;

namespace Volta.Core.Infrastructure.Framework
{
    public static class ByteArrayExtensions
    {
        public static string ToHex(this byte[] bytes)
        {
            return bytes.Select(x => x.ToString("X2")).Aggregate((a, i) => a + i).ToLower();
        }
    }
}
