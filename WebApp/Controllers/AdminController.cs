using Domain.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
[Route("admin")]
public class AdminController : Controller
{
    private readonly IMemberService _memberService;

    public AdminController(IMemberService memberService)
    {
        _memberService = memberService;
    }


    //[Authorize(Roles = "admin")]
    [Route("members")]
    public async Task<IActionResult> Members()
    {
        var members = await _memberService.GetAllMembersAsync();

        var viewModel = new MembersViewModel
        {
            Members = members.Select(member => new MemberViewModel
            {
                Member = member,
                EditMemberViewModel = new EditMemberViewModel
                {
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    Phone = member.Phone,
                    JobTitle = member.JobTitle,
                    //Address = member.Address
                }
            }),
            AddMemberViewModel = new()
        };

        return View(viewModel);
    }


    //[Authorize(Roles = "admin")]
    [Route("clients")]
    public IActionResult Clients()
    {
        return View();
    }

    
}
