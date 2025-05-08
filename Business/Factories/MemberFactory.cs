
using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;

public  class MemberFactory
{
    public static MemberEntity ToEntity(MemberSignUpDto dto)
    {
        return dto == null
            ? new MemberEntity()
            : new MemberEntity()
            {
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
            };
    }

    public static Member ToModel(MemberEntity entity)
    {
        return entity == null
            ? new Member()
            : new Member()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                JobTitle = entity.JobTitle,
                //Address = entity.Address,
            };
    }
}
