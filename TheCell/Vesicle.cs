using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Vesicle
    {
        int amount;
        int distance;
        metabolites resource;
        Organelle origin;
        Organelle destination;

        public Vesicle(metabolites what, int dist, int amount, Organelle or, Organelle dest)
        {
            resource = what;
            distance = dist;
            this.amount = amount;
            origin = or;
            destination = dest;
        }

        public int GetDist()
        {
            return distance;
        }
        public int DecreaseDist()
        {
            distance--;
            return distance;
        }
        public int GetAmount()
        {
            return amount;
        }
        public metabolites GetRes()
        {
            return resource;
        }
        public Organelle GetOrigin()
        {
            return origin;
        }
        public Organelle GetDestination()
        {
            return destination;
        }
    }
}
