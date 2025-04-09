using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TagRepository(AppDbContext context) : BaseRepository<TagEntity, Tag>(context), ITagRepository
    {

    }
}
