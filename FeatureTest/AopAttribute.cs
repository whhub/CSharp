using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace FeatureTest
{
    class AopAttribute : ProxyAttribute
    {
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            var proxy = new AopProxy(serverType);
            return null;
        }
    }

    internal class AopProxy : RealProxy
    {
        public AopProxy(Type serverType)
        {
            throw new NotImplementedException();
        }

        public override IMessage Invoke(IMessage msg)
        {
            return null;
        }
    }
}
