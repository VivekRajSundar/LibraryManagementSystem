using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Enums
{
    public enum AdminActivity
    {
        ViewAllUsers = MemberActivity.Logout + 1,
        AddBook,
    }
}
