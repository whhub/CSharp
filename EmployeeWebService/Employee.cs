using System.Runtime.Serialization;

namespace EmployeeWebService
{
    [DataContract]
    public class Employee
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Bu { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string SuperiorId { get; set; }
    }
}