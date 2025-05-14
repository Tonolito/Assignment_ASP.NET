using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

    [HttpPut]
    [Route("edit")]
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
        return Ok(new { success = false, error = "Failed to update client." });
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
                image = result.Result.Image,
                clientName = result.Result.ClientName,
                email = result.Result.Email,
                location = result.Result.Location,
                phone = result.Result.Phone,
                
            });
        }

        return Json(new { success = false, error = "Client not found." });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        if (string.IsNullOrEmpty(id))
        {
            return BadRequest(new { success = false, message = "Client ID cannot be null or empty" });
        }

        var result = await _clientService.DeleteClientAsync(id);
        if (result.Succeeded)
        {
            return Ok(new { success = true, message = "Delete deleted successfully." });
        }
        if (!result.Succeeded)
        {
            Debug.WriteLine($"Failed to delete client: {result.Error}");
        }

        return BadRequest(new { success = false, message = result.Error });
    }

}
