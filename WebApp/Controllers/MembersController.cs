using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Business.Interfaces;
using Business.Services;
using Domain.Extentions;
using Domain.Models;

namespace WebApp.Controllers;

[Route("members")]
public class MembersController : Controller
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [Route("chat")]
    public IActionResult Chat()
    {
        return View();
    }

    [HttpPost]
    [Route("members/add")]

    public async Task<IActionResult> Add(AddMemberViewModel model)
    {
        AddMemberDto dto = model;
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
        if (result.Succeeded)
        {
            return Ok(new { success = true });
        }
      
        return Ok(new { success = true });

    }
    [HttpPost("edit/{id}")]

    public async Task<IActionResult> Edit(EditMemberViewModel model)
    {
        try
        {
            EditMemberDto dto = model;



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
            if (result.Succeeded)
            {
                return Ok(new { success = true });
            }


           
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { success = false, message = "An error occurred while processing the request." });


        }

    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        Console.WriteLine($"Received ID: {id}");  // Logga ID här för att se vad som skickas

        var result = await _memberService.GetMemberByIdAsync(id);
        Console.WriteLine($"Result Succeeded: {result.Succeeded}");
        Console.WriteLine($"Result Count: {result.Result?.Count()}");

        if (result.Succeeded)
        {
            var member = result.Result?.FirstOrDefault();

            var editMemberViewModel = new EditMemberViewModel
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                Phone = member.Phone,
                JobTitle = member.JobTitle
            };

            return Json(new { success = true, data = editMemberViewModel });
        }

        return Json(new { success = false, error = "Member not found." });
    }




}
