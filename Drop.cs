using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace WindowsFormsRain
{
    class Drop
    {
        private int width;
        private int height;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Drop_d { get; private set; }
        public Color DropColor { get; private set; }
        private static Random rand = null;
        private int dy;
        static public int DX { get; set; }
        private Thread t = null;
        private bool stop = false;

        public bool IsAlive
        {
            get { return t != null && t.IsAlive; }
        }
        public Drop(Rectangle r)
        {
            Update(r);
            if (rand == null) rand = new Random();
            Drop_d = rand.Next(15, 25);
            Y = 0;
            X = rand.Next(0, width);

            DropColor = Color.FromArgb(rand.Next(25, 255), 0, 0, rand.Next(200));
            do
            {
                dy = rand.Next(0, 5);
            } while (dy == 0);
        }

        public void Update(Rectangle r)
        {
            width = r.Width;
            height = r.Height;
        }

        private void Move()
        {
            while (!stop)
            {
                Thread.Sleep(30);
                Y += dy;
                if (DX != 0)
                {
                    X += DX;
                    if (X < 0)
                    {
                        X = width - 1;
                    }
                    if (X > width)
                    {
                        X = 1;
                    }
                }
            }
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
