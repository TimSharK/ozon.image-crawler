using System;
using System.Collections.Generic;
using System.Text;

namespace Console.ConfigModels
{
    public class AppSettings
    {
        public string Url { get; set; }

        public bool UseFileSystem { get; set; } = true;
    }
}
