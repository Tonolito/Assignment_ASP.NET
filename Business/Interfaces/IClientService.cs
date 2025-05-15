using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ClientResult> AddClientAsync(AddClientDto dto);
        Task<ClientResult> DeleteClientAsync(string id);
        Task<ClientResult> EditClientAsync(EditClientDto dto);
        Task<ClientsResult> GetAllClientsAsync();
        Task<ClientResult> GetClientByIdAsync(string id);
        Task<List<ClientSearchDto>> SearchClientAsync(string term);
    }
}