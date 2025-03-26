using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class MembersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(AddMemberForm form)
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
