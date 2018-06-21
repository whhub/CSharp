using Nest;

namespace EmployeePermissionInfoToElasticsearch
{
    [ElasticsearchType(Name = "Group")]
    public class Group
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DistinguishedName { get; set; }
    }
}