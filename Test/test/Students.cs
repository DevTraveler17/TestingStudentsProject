using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Students : User
    {
        int id_group;
       // Roles HerRole = Roles.Student;
        public int Id_Group
        {
            get
            {
                return id_group;
            }
            set
            {
                id_group = value;
            }
        }

    }
}
