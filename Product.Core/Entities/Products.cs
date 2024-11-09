﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Entities
{
    public class Products : BasicEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ProductPicture { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
