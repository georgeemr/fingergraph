using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace FutronicDrv
{
    class Program
    {
        static Device d = new Device();

        static void Wait( int n )
        {
            int x = 0;
            for (int i = 0; i < n; i++)
                x += i;
            if (x == -1)
                Console.WriteLine("epic fail"); 
        }
        
        static void Window()
        {
            Console.WriteLine("Thread-Window : started");
            while (d.Connected)
            {
                Thread.Sleep(100);
                if (d.IsFinger())
                    Console.WriteLine("I see a finger! What shall I do?");
            }
            Console.WriteLine("Thread-Window : finished (device is disconnected)");
        }


        static void Main(string[] args)
        {
            Thread t = new Thread(Window);
            Device d = new Device();

            if (!d.Init())
            {
                Console.WriteLine("fail to connect");
                return;
            }
            Console.WriteLine("Device is connected");
            t.Start();
            Console.WriteLine("1");
            d.ExportBitMap().Save("D:\\finger.bmp");
            Console.WriteLine("2");
            
            int n = (int)1e6, i = n / 2 , di = (int)1e4;
            while (!Console.KeyAvailable)
            {
                d.SetDiodesStatus(true, true);
                Wait(i);
                d.SetDiodesStatus(false, false);
                Wait(n - i);

                i += di;
                if (i < n / 8 || i > 3 * n / 4)
                {
                    di *= -1;
                    i += di;
                    Console.WriteLine(i + " " + n);
                }
            }
            d.SetDiodesStatus(false, false);
            d.Dispose();
            Console.WriteLine("3");
        }
    }
}
