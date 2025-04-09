using Data.Entities;
using Domain.Models;

namespace Data.Interfaces
{
    public interface ITagRepository : IBaseRepository<TagEntity, Tag>
    {
    }
}