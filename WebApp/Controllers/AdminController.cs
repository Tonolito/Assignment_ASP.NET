using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Business.Interfaces;
using Domain.Models;
using Business.Models;

namespace WebApp.Controllers;

[Authorize(Roles = "Administrator")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly IMemberService _memberService;
    private readonly IClientService _clientService;

    public AdminController(IMemberService memberService, IClientService clientService)
    {
        _memberService = memberService;
        _clientService = clientService;
    }


    [Authorize(Roles = "Administrator")]
    [Route("members")]
    public async Task<IActionResult> Members()
    {
        var members = await _memberService.GetMembersAsnyc();

        var viewModel = new MembersViewModel
        {
            Members = members.Result?.Select(member => new MemberViewModel
            {
                Member = member, 
                EditMemberViewModel = new()
            }) ?? [],
            AddMemberViewModel = new()
        };


        return View(viewModel);
    }


    [Authorize(Roles = "Administrator")]
    [Route("clients")]
    public async Task<IActionResult> Clients()
    {
        var clients = await _clientService.GetAllClientsAsync();

        var viewModel = new ClientsViewModel
        {
            Clients = clients.Result?.Select(x => new Client
            {
                Id = x.Id,
                Image = x.Image,
                ClientName = x.ClientName,
                Email = x.Email,
                Location = x.Location,
                Phone = x.Phone,
            }) ?? [],
            AddClient = new(),
            EditClient = new()

        };

        return View(viewModel);
    }

    
}

