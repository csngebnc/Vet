﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Models
{
    public class PhotoClass
    {
        public IFormFile ProfilePhoto { get; set; }
    }
}
