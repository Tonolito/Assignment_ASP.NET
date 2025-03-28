using Domain.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Controllers;

public class MembersController : Controller
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddMemberViewModel model)
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

        var result = await _memberService.AddMemberAsync(model);
        if (result)
        {
            return Ok(new { succes = true });
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
    public async Task<IActionResult> Edit(EditMemberViewModel model)
    {
        try
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

            var result = await _memberService.EditMemberAsync(model);
            if (result)
            {
                return Ok(new { succes = true });
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
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { success = false, message = "An error occurred while processing the request." });


        }

    }
}
