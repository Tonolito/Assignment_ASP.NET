using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Route("clients")]
public class ClientsController : Controller
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost]
    [Route("clients/Add")]
    public async Task<IActionResult> Add(AddClientViewModel model)
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

        var result = await _clientService.AddClientAsync(model);
        if(result.Succeeded)
        {
            return Ok(new { success = true });

        }

        return Ok(new { success = true });

    }

    [HttpPost]
    [Route("clients/Edit")]

    public async Task<IActionResult> Edit(EditClientViewModel model)
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

        var result = await _clientService.EditClientAsync(model);
        if (result.Succeeded)
        {
            return Ok(new { success = true });

        }
        return Ok(new { succes = true });

    }

    [HttpGet]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        Console.WriteLine($"Edit Client - ID received: {id}");

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        var result = await _clientService.GetClientByIdAsync(id);

        if (result.Succeeded && result.Result != null)
        {
            return Json(new
            {
                id = result.Result.Id,
                clientName = result.Result.ClientName,
                email = result.Result.Email,
                location = result.Result.Location,
                phone = result.Result.Phone,
                
            });
        }

        return Json(new { success = false, error = "Client not found." });
    }

}
