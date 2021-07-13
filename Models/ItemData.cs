﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class ItemData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
    }
}
