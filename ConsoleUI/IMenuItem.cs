﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IMenuItem
    {
        public string Name { get; }
        public Task ExecuteAsync();
    }
}
