using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfRESTfulService
{
    public class UserList
    {
        private static readonly UserList _instance = new UserList();

        private UserList()
        {
        }

        public static UserList Instance {get { return _instance; }}
        
        public IList<Student>  Users {get { return _users; }}

        private IList<Student> _users = new List<Student>
        {
            new Student {Id = 1, Name = "zhangsan"},
            new Student {Id=2, Name = "lisi"},
            new Student {Id=3, Name = "wangwu"}
        }; 
    }
}