using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class StatusRepository(DataContext context) : BaseRepository<StatusEntity, Status>(context), IStatusRepository
{
}
