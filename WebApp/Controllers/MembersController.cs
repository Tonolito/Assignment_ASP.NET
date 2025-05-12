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

    [HttpGet]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
       

        var result = await _memberService.GetMemberByIdAsync(id);
       

        if (result.Succeeded && result.Result != null)
        {

            return Json(new
            {
                id = result.Result.Id,
                firstName = result.Result.FirstName,
                lastName = result.Result.LastName,
                email = result.Result.Email,
                phoneNumber = result.Result.PhoneNumber,
                jobTitle = result.Result.JobTitle,
            });
           
        }

        return Json(new { success = false, error = "Member not found." });
    }





    // TAGS

    [HttpGet("search-members")]
    public async Task<JsonResult> SearchMember(string term)
    {
        var users = await _memberService.SearchMemberAsync(term);
        return Json(users);
    }



}
