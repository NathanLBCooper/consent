namespace Consent.Api;

internal static class HttpHeaderNames
{
    /**
     * The name I need from HeaderNames, but as const strings so they can be used in attributes
     * <see cref="Microsoft.Net.Http.Headers.HeaderNames"/>
     */

    public const string IfNoneMatch = "If-None-Match";
    public const string IfMatch = "If-Match";
    public const string ETag = "ETag";
}
