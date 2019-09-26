using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Ribosome : Organelle, IOrganelle
    {
        int capacity;

        public Ribosome(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;
            contents[2] = capacity * 2; //initial resources
            contents[1] = capacity * 10;
            contents[3] = capacity * 10;
        }

        public override void Init()
        {
            distances.Add(parent.vacuole, 6);
        }

        public override void Process()
        {
            //5 glucose + mRNA + 5 N -> protein
            int speed = Math.Min(contents[1] / 5, contents[2]);   //limitation of speed by amount of resources
            speed = Math.Min(speed, contents[3] / 5);
            speed = Math.Min(speed, capacity);  //limitation by production capacity
            contents[1] -= speed * 5;   //used up glucose
            contents[2] -= speed;   //used up mRNA
            contents[3] -= 5 * speed;  //used up nitrogen
            contents[7] += speed;   //created proteins            
            if (contents[7] > parent.MAX)
            {
                //if there is too much of a resource, it is sent to the vacuole
                int distanceV;
                distances.TryGetValue(parent.vacuole, out distanceV);
                parent.vacuole.SendResources(metabolites.proteins, contents[7] - parent.MAX, distanceV, this);
                contents[7] = parent.MAX;
            }
        }

        public override void RequestResources()
        {
            double howMuch = (capacity * 10.0) - contents[(int)metabolites.glucose];   //can request max to fill up to 200% its capacity
            howMuch = Math.Max(0, howMuch);
            parent.chloroplast.IWant(metabolites.glucose, (int)howMuch, this);
            howMuch = (capacity * 2.0) - contents[(int)metabolites.mRNA];
            howMuch = Math.Max(0, howMuch);
            parent.nucleus.IWant(metabolites.mRNA, (int)howMuch, this);
            howMuch = (capacity * 10.0) - contents[(int)metabolites.N];
            howMuch = Math.Max(0, howMuch);
            parent.puddle.IWant(metabolites.N, (int)howMuch, this);
        }

        public override void DispatchResources()
        {
            distances.TryGetValue(parent.vacuole, out int dist);
            parent.vacuole.SendResources(metabolites.proteins, contents[7], dist, this);
            contents[7] = 0;
            //sends all products to the vacuole
        }
    }
}
