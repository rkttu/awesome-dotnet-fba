using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Message"] = "Minimal API와 MVC 컨트롤러가 함께 동작 중입니다!";
        return View();
    }

    public IActionResult About()
        => Content("이 페이지는 Home/About 액션에서 렌더링되었습니다.");
}
