using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace MarkdownDistrictAspNetCore.Controllers
{
    public class HelpController : Controller
    {
        string MarkdownPath;
        public HelpController(IHostEnvironment env)
        {
            MarkdownPath = Path.Combine(env.ContentRootPath, "HelpDoc");
        }

        public IActionResult Index(string path)
        {
            // Path validation
            path = path ?? string.Empty;
            if (!Regex.IsMatch(path, "^[A-Za-z/]*$"))
                return Content($"Ivalid Path - {path}");
            var mdPath = Path.Combine(MarkdownPath, path.Replace("/", "\\"));
            if (!System.IO.File.Exists(mdPath + ".md"))
            {
                mdPath = mdPath.TrimEnd('/') + "/index";
                if (!System.IO.File.Exists(mdPath + ".md"))
                    return NotFound();
                else
                {
                    if (!string.IsNullOrEmpty(path)) 
                        path = path.TrimEnd('/') + "/";
                    return Redirect($"~/Help/{path}index");
                }
            }
            ViewBag.HtmlContent = Markdig.Markdown.ToHtml(System.IO.File.ReadAllText(mdPath + ".md"));
            return View();
        }
    }
}
