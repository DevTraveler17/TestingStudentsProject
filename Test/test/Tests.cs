using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Tests
    {
        int id;
        string subject;
        string test_name;
        string link;
//        int id_test_autor;
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
        public string TestName
        {
            get
            {
                return test_name;
            }
            set
            {
                test_name = value;
            }
        }
        public string Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;
            }
        }
//        public int IdTestAutor
//        {
//            get
//            {
//  //              return id_test_autor;
//            }
//            set
//            {
////                id_test_autor = value;
//            }
//        }
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }
    }
}
