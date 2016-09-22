using System;
using System.Threading;

namespace CSharp
{
    class MyHeater
    {
        public event EventHandler Boiled;


        private Thread _heatThread;
        public void Begin()
        {
            _heatTime = 5;
            _heatThread = new Thread(Heat);
            _heatThread.Start();
            Console.WriteLine("加热器已经开启");
        }

        private int _heatTime;
        private void Heat()
        {
            while (true)
            {
                Console.WriteLine("加热还需要{0}秒", _heatTime);

                if (_heatTime == 0)
                {
                    RaiseBoiledEvent();
                    return;
                }
                _heatTime--;
                Thread.Sleep(1000);
            }
        }

        private void RaiseBoiledEvent()
        {
            if (Boiled == null)
            {
                Console.WriteLine("加热完成事件未被订阅");
            }
            else
            {
                Boiled(this, new EventArgs());
            }
        }
    }
}
