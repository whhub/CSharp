using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace FeatureTest
{
    class PerformanceAttribute : ContextAttribute, IContributeObjectSink
    {
        public PerformanceAttribute() : base("performance")
        {
        }

        #region Implement Interface

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            throw new NotImplementedException();
        }

        #endregion Implement Interface

    }
}
