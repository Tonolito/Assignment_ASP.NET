﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models;

public class ProjectsResult<T> : ServiceResult
{

    public T? Result { get; set; }

}
