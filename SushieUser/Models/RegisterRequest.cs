using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushieUser.Models
{
    public class RegisterRequest
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Birth { get; set; } // Можно использовать DateTime, если нужно
        public string Telephone { get; set; }
        public string Email { get; set; }
    }
}
