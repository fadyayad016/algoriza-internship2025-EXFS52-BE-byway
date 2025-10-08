using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        // 1. نقطة نهاية عامة (لا تحتاج لتوكن)
        [HttpGet("public")]
        public IActionResult GetPublicData()
        {
            return Ok("This is PUBLIC data, anyone can see this.");
        }

        // 2. نقطة نهاية محمية (تحتاج لأي توكن صالح)
        [HttpGet("user")]
        [Authorize]
        public IActionResult GetAuthenticatedUserData()
        {
            // User.Identity.Name سيقوم بعرض الإيميل الموجود في التوكن
            return Ok($"Hello, {User.Identity.Name}! You are an AUTHENTICATED user.");
        }

        // 3. نقطة نهاية للأدمن فقط
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok($"Hello, {User.Identity.Name}! You are an ADMIN.");
        }
    }
}
