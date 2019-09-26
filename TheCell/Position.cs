using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Position
    {
        int X;
        int Y;
        metabolites resource;

        public Position(metabolites res, int x, int y)
        {
            resource = res;
            X = x;
            Y = y;
        }

        public int GetX()
        {
            return X;
        }
        public int GetY()
        {
            return Y;
        }
        public metabolites GetResource()
        {
            return resource;
        }
    }
}
