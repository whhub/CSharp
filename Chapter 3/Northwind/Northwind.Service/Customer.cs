using System.Runtime.Serialization;

namespace Northwind.Service
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public string CustomerID { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string ContractName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Phone { get; set; }
    }
}