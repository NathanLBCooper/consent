using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Consent.Tests;

internal class FakeLinkGenerator : LinkGenerator
{
    public string MakePath(RouteValueDictionary values)
    {
        return $"FakeLinkGenerator:{string.Join(',', values.ToArray().Select(kv => $"{kv.Key}:{kv.Value}"))}";
    }

    public override string GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values,
        RouteValueDictionary? ambientValues = null, PathString? pathBase = null,
        FragmentString fragment = new FragmentString(), LinkOptions? options = null)
    {
        return MakePath(values);
    }

    public override string GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values, PathString pathBase = new PathString(),
        FragmentString fragment = new FragmentString(), LinkOptions? options = null)
    {
        return MakePath(values);
    }

    public override string GetUriByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values,
        RouteValueDictionary? ambientValues = null, string? scheme = null, HostString? host = null,
        PathString? pathBase = null, FragmentString fragment = new FragmentString(), LinkOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public override string GetUriByAddress<TAddress>(TAddress address, RouteValueDictionary values, string? scheme, HostString host,
        PathString pathBase = new PathString(), FragmentString fragment = new FragmentString(), LinkOptions? options = null)
    {
        throw new NotImplementedException();
    }
}
