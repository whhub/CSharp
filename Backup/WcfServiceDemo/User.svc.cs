using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceDemo
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "User" in code, svc and config file together.
    public class User : IUser
    {
        public string ShowName(string name)
        {
            string wcfName = string.Format("WCF 服务，显示姓名：{0}", name);
            return wcfName;
        }
    }
}
