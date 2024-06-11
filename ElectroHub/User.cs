using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroHub
{
    internal class User
    {
        public string Username { get; set; }
        public string Role { get; set; }

        public User(string username, string role)
        {
            this.Username = username;
            this.Role = role;
        }
    }
}
