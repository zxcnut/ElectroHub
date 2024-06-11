using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroHub
{
    internal class UserManager
    {
        public static User CurrentUser { get; private set; }

        public static void SetCurrentUser(string username, string role)
        {
            CurrentUser = new User(username, role);
        }
    }
}
