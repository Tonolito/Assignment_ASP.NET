using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.ViewComponents
{
    public class ProfileImageViewComponent(IMemberService memberService) : ViewComponent
    {
        private readonly IMemberService _memberService = memberService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var username = User.Identity?.Name;
            var imageUrl = await _memberService.GetMemberImageAsync(username!);

            var model = new HeaderViewModel
            {
                ProfileImage = imageUrl
            };

            return View(model);

        }
    }
}
