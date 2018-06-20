﻿using Nest;

namespace ActiveDirectoryConsole
{
    [ElasticsearchType(Name = "Group")]
    public class Group
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}