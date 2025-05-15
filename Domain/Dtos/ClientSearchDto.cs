using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos;

public class ClientSearchDto
{
    public string Id { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string? ImageUrl { get; set; }
}
