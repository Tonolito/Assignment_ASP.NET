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

    [HttpPost]
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
    [HttpPost]
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


            //var result = await _clientService.AddClientAsync(form);
            //if(result)
            //{
            //    return Ok(new { succes = true });
            //}
            //else
            //{
            //    return Problem("Unable to submit data");
            //}
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { success = false, message = "An error occurred while processing the request." });


        }

    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Edit(string id)
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

        var result = await _memberService.GetMemberByIdAsync(id);
        if (result.Succeeded)
        {
            var member = result.Result.FirstOrDefault();

            var memberViewModel = new MemberViewModel
            {
                Member = new Member()
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    Phone = member.Phone,
                    JobTitle = member.JobTitle,
                    Address = member.Address
                }
            };


            return Ok(new { success = true, member = memberViewModel});
        }

        return NotFound(new { success = false, error = "Member not found." });
    }
    

}
