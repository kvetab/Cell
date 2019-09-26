using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TheCell
{
    class Model
    {
        Cell cell;
        Visualizer visualizer;
        Timer timer;
        int time = 300; //default interval
        bool running = false;
        

        public Model(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pb)
        {
            timer = new Timer(OneTick, null, Timeout.Infinite, time);
            cell = new Cell();
            visualizer = new Visualizer(cell, g, pb);
        }
        

        public void OneTick(object state)            
        {
            //one step of the simulation
            cell.Run();
            visualizer.Show();
        }
        public void SetSun(int level)
        {
            cell.SetSunshine(level);
        }
        public void SetUV(int level)
        {
            cell.SetUV(level);
        }
        public void SetN(int level)
        {
            cell.SetN(level);
        }
        public void SetP(int level)
        {
            cell.SetP(level);
        }
        
        public void Start()
        {
            timer.Change(20, time);
            running = true;
        }
        public void Pause()
        {
            timer.Change(Timeout.Infinite, time);
            running = false;
        }

        public void SetInterval(int interval)   
            //interval is the value of trackBar5 which is used to set speed, ranges from 0 to 10
        {
            time = 100 + (40 * interval);
            if (running)
            {
                timer.Change(20, time);
            }
            else
            {
                timer.Change(Timeout.Infinite, time);
            }            
        }
        

        public void Redraw()
        {
            visualizer.Redraw();
        }
        

    }
}
