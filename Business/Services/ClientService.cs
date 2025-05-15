using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extentions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, AppDbContext context) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly AppDbContext _context = context;

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
        Console.WriteLine(result == null ? "Client not found in repository" : $"Client found: {result.Result.Id}");

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
    //Kopierat från min memberservice
    public async Task<ClientResult> DeleteClientAsync(string id)
    {
        if (id == null)
        {
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Id can't be found." };
        }
        try
        {
            var clientEntity = await _context.Clients.FindAsync(id);

            if (clientEntity == null)
            {
                return new ClientResult { Succeeded = false, StatusCode = 404, Error = "Client not found." };
            }

            // Ta bort medlemmen
            _context.Clients.Remove(clientEntity);
            await _context.SaveChangesAsync();

            return new ClientResult { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<List<ClientSearchDto>> SearchClientAsync(string term)
    {
        if (string.IsNullOrEmpty(term))
            return new List<ClientSearchDto>();

        var clients = await _context.Clients
            .Where(x => x.ClientName.Contains(term) || x.Email.Contains(term))
            .Select(x => new ClientSearchDto
            {
                Id = x.Id,
                ClientName = x.ClientName,
                ImageUrl = x.Image,
            })
            .ToListAsync();

        return clients;

    }
}
