using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushieUser.Models
{
    public class SushieItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public int quantity { get; set; }
        public string photo { get; set; }
        public int category_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

}
