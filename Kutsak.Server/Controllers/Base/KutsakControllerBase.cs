using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Controllers.Base;

public abstract class KutsakControllerBase : ControllerBase
{
    public void Verify(string header, string value) {
        if (Request.Headers[header] != value && Request.Query[header] != value) {
            throw new UnauthorizedAccessException();
        }
    }
}
