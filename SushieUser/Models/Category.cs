using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushieUser.Models
{
    public class Category
    {
        public long Id { get; set; } // bigint
        public string Name { get; set; } // varchar(255)
        public DateTime CreatedAt { get; set; } // timestamp
        public DateTime UpdatedAt { get; set; } // timestamp
    }
}
