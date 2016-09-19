using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace FeatureTest
{
    //决定是否捕获异常，是否计时，是否打log， 是否做参数检测，是否做返回值检测
    class CallTraceSink : IMessageSink  
    {
        private bool _ifTraceCostTime;

        //在构造器中初始化下一个接收器
        public CallTraceSink(IMessageSink next, bool ifTraceCostTime)
        {
            NextSink = next;
            _ifTraceCostTime = ifTraceCostTime;
        }

        #region Implemented Interface

        //实现IMessageSink的接口方法，当消息传递的时候，该方法被调用
        //性能呢？
        public IMessage SyncProcessMessage(IMessage msg)
        {
            //拦截消息，做前处理
            Preprocess(msg);    //参数Log、校验

                 var retMsg = _ifTraceCostTime ? Process(msg) : NextSink.SyncProcessMessage(msg);

            //调用返回时进行拦截，并进行后处理
            Postprocess(msg, retMsg); //返回Log(返回值、异常） 

            return retMsg;
        }

        //IMessageSink接口方法，用于异步处理，我们不实现异步处理，所以简单返回null,
        //不管是同步还是异步，这个方法都需要定义
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }

        public IMessageSink NextSink { get; private set; }

        #endregion Implemented Interface


        private IMessage Process(IMessage msg)
        {
            var sw = new Stopwatch();
            sw.Start();

            //传递消息给下一个接收器
            IMessage retMsg = NextSink.SyncProcessMessage(msg);

            sw.Stop();
            Console.WriteLine(string.Format("Time Cost (ms) : {0}", sw.ElapsedMilliseconds));
            return retMsg;
        }

        private void Preprocess(IMessage msg)
        {
            var call = msg as IMethodCallMessage;
            if(call == null)
            {
                Console.WriteLine("Can't get CallMessage in PreProcess");
                return;
            }

            var argumentList = call.Args.Select(a => string.Format("{0}:{1}", a.GetType().Name, a.ToString()));
            var argumentsInfo = string.Join(",", argumentList);

            //var argumentInfoBuilder = new StringBuilder();
            //foreach (var arg in call.Args)
            //{
            //    argumentInfoBuilder.AppendFormat("{0}:{1},", arg.GetType(), arg.ToString());
            //}
            //if (argumentInfoBuilder.Length > 1) argumentInfoBuilder.Remove(argumentInfoBuilder.Length - 1, 1);
            var type = call.TypeName.Split(',').First().Split('.').Last();


            Console.WriteLine(string.Format("[Begin]{0}.{1}({2})", type, call.MethodName, argumentsInfo));

        }

        private void Postprocess(IMessage msg, IMessage retMsg)
        {
            var mi = retMsg as IMethodReturnMessage;
            if (mi == null)
            {
                Console.WriteLine("Can't get ReturnMessage in PostProcess");
                return;
            }

            var outArgumentsInfo = string.Empty;
            if (mi.OutArgCount > 0)
            {
                var outArgumentList = mi.OutArgs.Select(a => string.Format("{0}:{1}", a.GetType(), a.ToString()));
                outArgumentsInfo = string.Format("[Out]({0})", string.Join(",", outArgumentList)); 
            }
            var type = mi.TypeName.Split(',').First().Split('.').Last();
            Console.WriteLine(string.Format("[End]{0}.{1}[Ret]{2}{3}", type, mi.MethodName, mi.ReturnValue, outArgumentsInfo));

            var e = mi.Exception;
            if (e != null)
            {
                Console.WriteLine(string.Format("[Exception]{0}", e.Message));
            }
        }
    }
}
