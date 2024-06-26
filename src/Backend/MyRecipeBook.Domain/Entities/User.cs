﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte Active { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
