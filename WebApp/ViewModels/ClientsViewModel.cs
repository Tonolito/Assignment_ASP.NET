using Business.Models;
using Domain.Models;

namespace WebApp.ViewModels;

public class ClientsViewModel
{
    public IEnumerable<Client> Clients { get; set; } = [];

    public AddClientViewModel AddClient { get; set; } = null!;

    public EditClientViewModel EditClient { get; set; } = null!;

    //public static implicit operator ClientsViewModel(ClientResult result)
    //{

    //    // HJÄLP AV CHATGPT
    //    return new ClientsViewModel
    //    {
    //        Clients = result.Result == new List<ClientViewModel>()
    //    };
    //}
}

