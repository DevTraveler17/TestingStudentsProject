using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class LogCompleteTest
    {
        int id;
        string login;
        string test;
        int percent_rightComplete;
        int rightQuestions;
        bool is_complete;
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
        public int Right
        {
            get
            {
                return rightQuestions;
            } 
            set
            {
                rightQuestions = value;
            }
        }
        public int Percent
        {
            get
            {
                return percent_rightComplete;
            }
            set
            {
                percent_rightComplete = value;
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
        public string Test
        {
            get
            {
                return test;
            }
            set
            {
                test = value;
            }
        }
        public bool IsComplete
        {
            get
            {
                return is_complete;
            }
            set
            {
                is_complete = value;
            }

        }
    }
}
