﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Admin.Product
{
    public class ProductAttributeDto
    {
        public int? AttributeId { get; set; }
        public string? AttributeName { get; set; }
        public string? Value { get; set; }
    }
}