using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extentions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    //CREATE
    public async Task<ClientResult> AddClientAsync(AddClientDto dto)
    {
        string imageUrl = string.Empty;
        if (dto.ClientImage != null && dto.ClientImage.Length > 0)
        {

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ClientImage.FileName)}";


            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");


            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);


            var filePath = Path.Combine(folderPath, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ClientImage.CopyToAsync(stream);
            }

            imageUrl = $"/images/avatars/{fileName}";
        }
        else
        {
            imageUrl = $"/images/avatars/templateavatar.svg";
        }
        var entity = dto.MapTo<ClientEntity>();
        entity.Image = imageUrl;

        var result = await _clientRepository.AddAsync(entity);

        if (result.Succeeded)
        {
            return result.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 201, }
            : new ClientResult { Succeeded = false, StatusCode = 201, Error = "Client created but something went wrong" };
        }
        return new ClientResult { Succeeded = false, StatusCode = 500, Error = "Unable to add client." };

    }
    //READ
    public async Task<ClientsResult> GetAllClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();

        return result.MapTo<ClientsResult>();

    }
    public async Task<ClientResult> GetClientByIdAsync(string id)
    {
        var result = await _clientRepository.GetAsync(x => x.Id == id);

        return result.MapTo<ClientResult>();

    }

    //UPDATE
    public async Task<ClientResult> EditClientAsync(EditClientDto dto)
    {
        var entity = dto.MapTo<ClientEntity>();

        var result = await _clientRepository.UpdateAsync(entity);

        if (result.Succeeded)
        {
            return result.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 201, }
            : new ClientResult { Succeeded = false, StatusCode = 201, Error = "Client updated but something went wrong" };
        }
        return new ClientResult { Succeeded = false, StatusCode = 500, Error = "Unable to update client." };

    }
    //DELETE
    public async Task<ClientResult> DeleteClientAsync(string id)
    {
        if (id == null)
        {
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Id can't be found." };
        }

        try
        {
       
            var clientResult = await _clientRepository.GetAsync(x => x.Id == id);
            var clientEntity = clientResult.Result!.MapTo<ClientEntity>();

            var result = await _clientRepository.DeleteAsync(clientEntity);

            return result.Succeeded
                ? new ClientResult { Succeeded = true, StatusCode = 200 }
                : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }


}
