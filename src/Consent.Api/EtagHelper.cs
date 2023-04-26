using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Consent.Api;

internal class EtagHelper
{
    public string Get(string resource, DateTime lastModified)
    {
        using var sha = SHA1.Create();
        var bytes = Encoding.UTF8.GetBytes(resource + lastModified);
        return ToHex(sha.ComputeHash(bytes)).ToLower();
    }

    public string Get(string resource, object value)
    {
        using var sha = SHA1.Create();
        var bytes = Encoding.UTF8.GetBytes(resource + JsonSerializer.Serialize(value));
        return ToHex(sha.ComputeHash(bytes)).ToLower();
    }

    private static string ToHex(byte[] hash)
    {
        return BitConverter.ToString(hash).Replace("-", "");
    }
}
