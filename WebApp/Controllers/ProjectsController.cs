using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectsController : Controller
{
    [Route("")]
    public IActionResult Projects()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(AddProjectDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        //var result = await _clientService.AddClientAsync(form);
        //if(result)
        //{
        //    return Ok(new { succes = true });
        //}
        //else
        //{
        //    return Problem("Unable to submit data");
        //}
        return Ok(new { succes = true });

    }
}
