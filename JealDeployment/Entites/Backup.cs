﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment.Entites
{
    public class Backup
    {
        public string DesinationFolder { get; set; }
        public DuplicateNameingRule DuplicateNameingRule { get; set; }
    }
}