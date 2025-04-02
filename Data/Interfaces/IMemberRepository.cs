using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IMemberRepository : IBaseRepository<MemberEntity, Member>
{

}
