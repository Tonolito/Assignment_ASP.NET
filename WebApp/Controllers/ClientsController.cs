using Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class ClientsController : Controller
{
    //private IClientService _clientService;


    [HttpPost]
    public IActionResult AddClient(AddClientDto dt)
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

    [HttpPost]
    public IActionResult EditClient(EditClientDto dto)
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

        // SKICKA DATA TILL VÅR SERVICE


        //var result = await _clientService.UpdateClientAsync(form);
        //if (result)
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
