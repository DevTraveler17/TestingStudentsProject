using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public enum Roles
    {
        Teacher,
        Student,
        TestAutor
    }
    public class User
    {
        protected int id;
        protected string login;
        protected string name;
        protected string second_name;
        protected string third_name;
        protected string RoleS;
        public Roles role;
        
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string SecondName
        {
            get
            {
                return second_name;
            }
            set
            {
                second_name = value;
            }
        }
        public string ThirdName
        {
            get
            {
                return third_name;
            }
            set
            {
                third_name = value;
            }
        }
        public string Role
        {
            get
            {
                return RoleS;
            }
            set
            {
                RoleS = value;
            }
        }
    }
}
