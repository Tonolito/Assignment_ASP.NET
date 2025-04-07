using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class MemberAddressEntity
{
    public int Id { get; set; }

    [ForeignKey("Member")]
    public string UserId { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public MemberEntity Member { get; set; } = null!;
}
