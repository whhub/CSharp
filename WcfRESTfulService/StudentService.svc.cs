using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfRESTfulService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StudentService" in code, svc and config file together.
    public class StudentService : IStudentService
    {

        public Student GetStudentById(string id)
        {
            return UserList.Instance.Users.FirstOrDefault(u => u.Id == int.Parse(id));
        }

        public IList<Student> GetStudentList()
        {
            return UserList.Instance.Users;
        }

        public string GetString()
        {
            return "foo";
        }

        public IEnumerable<string> GetStringList()
        {
            return new List<string> {"foo1", "foo2"};
        }
    }
}
