using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace FutronicDrv
{
    class Program
    {
static void Wait( int n )
{
  int x = 0;
  for (int i = 0; i < n; i++)
    x += i;
  if (x == -1)
    Console.WriteLine("epic fail"); 
}

        static void Main(string[] args)
        {
            Device d = new Device();

            if (!d.Init())
            {
                Console.WriteLine("fail to connect");
                return;
            }
            Console.WriteLine("Device is connected");
            Console.WriteLine("1");
            //d.ExportBitMap().Save("D:\\finger.bmp");
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
