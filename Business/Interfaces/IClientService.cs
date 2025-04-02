using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ClientResult> AddClientAsync(AddClientDto dto);
        Task<ClientResult> EditClientAsync(EditClientDto dto);
        Task<ClientResult> GetAllClientsAsync();
    }
}