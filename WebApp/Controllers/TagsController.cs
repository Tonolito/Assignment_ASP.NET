using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp.Controllers;

public class TagsController(ITagService tagService) : Controller
{
    private readonly ITagService _tagService = tagService;

    [HttpGet]
    public async Task<IActionResult> SearchTags(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Json(new List<object>());
        }

        var tags = await _tagService.SearchTagsAsync(term);
        return Json(tags);
    }
}
