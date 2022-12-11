using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Consent.Tests;

internal class HeaderTestHelper
{
    private readonly ControllerBase _controller;

    public (string name, string value)[]? RecordedHeaders { get; private set; } = null;

    public HeaderTestHelper(ControllerBase controller)
    {
        _controller = controller;
    }

    public string? GetRecordedHeader(string header)
    {
        return RecordedHeaders?.SingleOrDefault(h => h.name == header).value;
    }

    public void RecordLastHeaders()
    {
        var context = _controller.ControllerContext.HttpContext;
        RecordedHeaders = context.Response.Headers.Select(h => (h.Key, h.Value.ToString())).ToArray();
    }

    public void ClearHeaders()
    {
        var context = _controller.ControllerContext.HttpContext;
        context.Request.Headers.Clear();
        context.Response.Headers.Clear();
    }
}
