using System;
using System.Collections.Generic;
using System.Text;

namespace PostgresImageStore.Models
{
    public class Image
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }
    }
}
