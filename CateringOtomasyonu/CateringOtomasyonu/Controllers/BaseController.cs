using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected bool IsInRole(string role)
    {
        var currentRole = HttpContext.Session.GetString("Rol");
        Console.WriteLine($"🧠 IsInRole çağrıldı. Session Rol = {currentRole}");

        return currentRole?.ToLower() == role.ToLower(); // ✅ duyarsız kontrol
    }

    protected IActionResult DenyAccess()
    {
        return RedirectToAction("Giris", "Login");
    }
}
