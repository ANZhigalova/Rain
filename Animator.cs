using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace WindowsFormsRain
{
    class Animator
    {
        private Graphics mainG;
        private int width, height;
        private List<Drop> drops = new List<Drop>();
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;
        private object obj = new object();
        public Animator(Graphics g, Rectangle r)
        {
            Update(g, r);
        }

        public void Update(Graphics g, Rectangle r)
        {
            mainG = g;
            width = r.Width;
            height = r.Height;
            Monitor.Enter(obj);
            bg = BufferedGraphicsManager.Current.Allocate(mainG, new Rectangle(0, 0, width, height));
            Monitor.Exit(obj);
            Monitor.Enter(drops);
            foreach (var b in drops)
            {
                b.Update(r);
            }
            Monitor.Exit(drops);
        }

        private void Animate()
        {
            while (!stop)
            {
                Monitor.Enter(obj);
                Graphics g = bg.Graphics;
                Monitor.Exit(obj);
                g.Clear(Color.White);
                Monitor.Enter(drops);
                int cnt = drops.Count;
                for(int i = 0; i<cnt; i++)
                {
                    if (!drops[i].IsAlive) drops.Remove(drops[i]);
                    i--;
                    cnt--;
                }
                foreach (var b in drops)
                {
                    Brush br = new SolidBrush(b.DropColor);
                    Point[] points = {
                        new Point(b.X, b.Y - 2 * b.Drop_d),
                        new Point(b.X - b.Drop_d, b.Y),
                        new Point(b.X, b.Y + b.Drop_d),
                        new Point(b.X+b.Drop_d, b.Y),
                        //new Point(b.X, b.Y - 2 * (b.Drop_d))
                    };
                    /*try
                    {*/
                        g.FillClosedCurve(br, points);
                    /*}catch(Exception w) { }*/
                }
                Monitor.Exit(drops);
                Monitor.Enter(obj);
                try
                {
                    bg.Render();
                }
                catch (Exception e) { }
                Monitor.Exit(obj);
                Thread.Sleep(30);
            }
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
            }
            var rect = new Rectangle(0, 0, width, height);
            Drop b  = new Drop(rect);
            b.Start();
            Monitor.Enter(drops);
            drops.Add(b);
            Monitor.Exit(drops);
        }
 
        public void Stop()
        {
            stop = true;
            Monitor.Enter(drops);
            foreach(var d in drops)
            {
                d.Stop();
            }
            drops.Clear();
            Monitor.Exit(drops);
        }
    }
}
