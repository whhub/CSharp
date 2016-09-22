using System;
using System.Collections.Generic;
using System.Threading;

namespace CSharp
{

    #region My Program

    class Program
    {
        static void Main()
        {
            TakesAWhileDelegate dl = TakesAWhile;

            var ar = dl.BeginInvoke(1, 3000, null, null);

            while (!ar.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(50);

            }

            var result = dl.EndInvoke(ar);

            Console.WriteLine("result: {0}", result);
        }

        static int TakesAWhile(int data, int ms)
        {
            Console.WriteLine("TakesAWhile started");
            Thread.Sleep(ms);
            Console.WriteLine("TakesAWhile completed");
            return ++data;
        }

        public delegate int TakesAWhileDelegate(int data, int ms);
    }




    #endregion


    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        var test = new Heater();
    //        #region 若多次添加同一个事件处理函数时，触发时处理函数是否也会多次触发？
    //        //test.OnBoiled += TestOnBoiled;
    //        //test.OnBoiled += TestOnBoiled;
    //        //test.Begin();

    //        #endregion


    //        #region 若添加了一个事件处理函数，却执行了两次或多次”取消事件“，是否会报错？
    //        //test.OnBoiled += TestOnBoiled;
    //        //test.OnBoiled += TestOnBoiled;
    //        //test.OnBoiled -= TestOnBoiled;
    //        //test.Begin();
    //        #endregion


    //        #region 如何认定两个事件处理函数是一样的？ 如果是匿名函数呢？
    //        //test.OnBoiled += (s, e) => Console.WriteLine("加热完成事件被调用");
    //        //test.OnBoiled -= (s, e) => Console.WriteLine("加热完成事件被调用");
    //        //test.Begin();
    //        #endregion


    //        #region 如果不手动删除事件函数，系统会帮我们回收吗？(更新引用)
    //        //test.OnBoiled += (s, e) => Console.WriteLine("加热完成事件被调用");
    //        //test = new Heater();
    //        //test.Begin();
    //        #endregion


    //        #region 如果不手动删除事件函数，系统会帮我们回收吗？(没有更新引用)
    //        //test.OnBoiled += (s, e) => Console.WriteLine("加热完成事件被调用");
    //        //var heaters = new List<Heater>() { test, test };
    //        //heaters.Clear();
    //        //test.Begin();
    //        //test = null;
    //        //GC.Collect();  //不论你加不加这句话，事件都会被执行
    //        #endregion


    //        #region 在多线程环境下，挂接事件时和对象创建所在的线程不同，那事件处理函数中的代码将在哪个线程中执行？(采用新加线程的做法)
    //        //test.OnBoiled += (s, e) =>
    //        //    {
    //        //        var newThread = new Thread(
    //        //            new ThreadStart(
    //        //                () =>
    //        //                {
    //        //                    Thread.Sleep(2000); //模拟长时间操作
    //        //                    Console.WriteLine("总算把热好的水加到了暖瓶里");
    //        //                }));
    //        //        newThread.Start();

    //        //    };

    //        //test.Begin();

    //        #endregion


    //        #region 在多线程环境下，挂接事件时和对象创建所在的线程不同，那事件处理函数中的代码将在哪个线程中执行？(采用线程池的做法)
    //        //var mainThread = Thread.CurrentThread;
    //        //test.OnBoiled += (s, e) =>
    //        //    {
    //        //        ThreadPool.QueueUserWorkItem((d) =>
    //        //            {
    //        //                Thread.Sleep(2000); //模拟长时间操作
    //        //                Console.WriteLine("总算把热好的水加到了暖瓶里");
    //        //                if (Thread.CurrentThread != mainThread)
    //        //                {
    //        //                    Console.WriteLine("两者执行的是不同的线程");
    //        //                }
    //        //                else
    //        //                {
    //        //                    Console.WriteLine("两者执行的是相同的线程");
    //        //                }

    //        //            });
    //        //    };

    //        //test.Begin();
    //        #endregion


    //        #region 在多线程环境下，挂接事件时和对象创建所在的线程不同，那事件处理函数中的代码将在哪个线程中执行？(构造函数在另一个线程)
    //        //var mainThread = Thread.CurrentThread;

    //        //ThreadPool.QueueUserWorkItem((d) =>
    //        //    {

    //        //        test.OnBoiled += (s, e) =>
    //        //            {
    //        //                if (Thread.CurrentThread == mainThread)
    //        //                    Console.WriteLine(Thread.CurrentThread != mainThread ? "两者执行的是不同的线程" : "两者执行的是相同的线程");
    //        //            };
    //        //    });

    //        //test.Begin();
    //        #endregion


    //        #region 在多线程环境下，挂接事件时和对象创建所在的线程不同，那事件处理函数中的代码将在哪个线程中执行？(订阅函数在另一个线程)
    //        //var mainThread = Thread.CurrentThread;
    //        //ThreadPool.QueueUserWorkItem((d) =>
    //        //{
    //        //    var bThread = Thread.CurrentThread;
    //        //    test.OnBoiled += (s, e) =>
    //        //    {
    //        //        if (Thread.CurrentThread == mainThread)
    //        //            Console.WriteLine("事件在主线程中执行");
    //        //        else if (bThread == Thread.CurrentThread)
    //        //        {
    //        //            Console.WriteLine("事件在订阅事件的线程B中执行");
    //        //        }
    //        //        else
    //        //        {
    //        //            Console.WriteLine("事件在第三个线程中执行");
    //        //        }
    //        //    };
    //        //});

    //        //test.Begin();
    //        #endregion

    //    }

    //    static void TestOnBoiled(object sender, EventArgs e)
    //    {
    //        Console.WriteLine("加热完成事件被调用");
    //    }
    //}

    ///// <summary>
    ///// 热水器
    ///// </summary>
    //public class Heater
    //{
    //    public event EventHandler OnBoiled;
    //    private void RasieBoiledEvent()
    //    {
    //        if (OnBoiled == null)
    //        {
    //            Console.WriteLine("加热完成处理订阅事件为空");
    //        }
    //        else
    //        {
    //            OnBoiled(this, new EventArgs());
    //        }
    //    }
    //    private Thread heatThread;
    //    public void Begin()
    //    {
    //        heatTime = 5;
    //        heatThread = new Thread(new ThreadStart(Heat));
    //        heatThread.Start();
    //        Console.WriteLine("加热器已经开启", heatTime);

    //    }
    //    private int heatTime;
    //    private void Heat()
    //    {
    //        while (true)
    //        {
    //            Console.WriteLine("加热还需{0}秒", heatTime);

    //            if (heatTime == 0)
    //            {
    //                RasieBoiledEvent();
    //                return;
    //            }
    //            heatTime--;
    //            Thread.Sleep(1000);

    //        }
    //    }
    //}
}
