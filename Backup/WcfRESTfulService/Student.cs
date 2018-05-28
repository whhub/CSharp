using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfRESTfulService
{
    [DataContract]
    public class Student
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}