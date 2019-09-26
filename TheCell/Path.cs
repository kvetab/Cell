using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Path
    {
        int length;
        int intervalX;
        int intervalY;  //length of intervals in perpendicular directions
        int startX;
        int startY; //starting point
        int endX;
        int endY;   //ending point

        Organelle start;
        Organelle end;

        public Path(Organelle start, Organelle end, int x, int y, int xx, int yy, int len)
        {
            this.start = start;
            this.end = end;
            this.startX = x;
            startY = y;
            endX = xx;
            endY = yy;
            length = len;
            intervalX = (xx - x) / length;
            intervalY = (yy - y) / length;

        }

        public int GetStartX()
        {
            return startX;
        }
        public int GetStartY()
        {
            return startY;
        }
        public int GetEndX()
        {
            return endX;
        }
        public int GetEndY()
        {
            return endY;
        }
        public int GetLength()
        {
            return length;
        }
        public int GetIntervalX()
        {
            return intervalX;
        }
        public int GetIntervalY()
        {
            return intervalY;
        }
        public Organelle GetStart()
        {
            return start;
        }
        public Organelle GetEnd()
        {
            return end;
        }
    }
}
