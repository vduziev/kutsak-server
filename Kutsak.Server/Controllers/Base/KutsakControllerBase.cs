using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Controllers.Base;

public abstract class KutsakControllerBase : ControllerBase
{
    public void Verify(string key, string value) {
        if (Request.Headers[key] != value && Request.Query[key] != value) {
            throw new UnauthorizedAccessException();
        }
    }
}
