﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class MembersResult : ServiceResult
    {
        public IEnumerable<Member>? Result { get; set; }
    }
}
