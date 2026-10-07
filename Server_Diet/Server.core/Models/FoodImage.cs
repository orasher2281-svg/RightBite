using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Core.Models
    {
        public class FoodImage
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public byte[] Data { get; set; } = Array.Empty<byte>();
            public string ContentType { get; set; } = "image/jpeg";
        }
    }

