using System.Runtime.Serialization;

namespace SqlConnection
{
    [DataContract]
    public class Employee
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Bu { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string SuperiorId { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {Bu}, {Department}, {SuperiorId}";
        }
    }
}