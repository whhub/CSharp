using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace FeatureTest
{
    [AttributeUsage(AttributeTargets.All)]
    class CallTraceAttribute : ContextAttribute, IContributeObjectSink
    {
        private bool _ifLogTimeCost;

        public CallTraceAttribute(bool ifLogTimeCost = false) : base("log")
        {
            _ifLogTimeCost = ifLogTimeCost;
        }

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new CallTraceSink(nextSink, _ifLogTimeCost);
        }
    }
}
