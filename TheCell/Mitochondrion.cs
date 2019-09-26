using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Mitochondrion: Organelle, IOrganelle
    {
        int capacity;

        public Mitochondrion(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;
            contents[1] = capacity * 1; //initial resources
            contents[4] = capacity * 6;
        }

        public override void Init()
        {
            distances.Add(parent.nucleus, 10);
            distances.Add(parent.ribosome, 6);
            distances.Add(parent.er, 4);
            distances.Add(parent.vacuole, 10);
        }

        public override void RequestResources()
        {
            double howMuch = (capacity * 2.0) - contents[(int)metabolites.glucose];
            howMuch = Math.Max(0, howMuch);
            //can request max to fill up to 200% its capacity
            parent.chloroplast.IWant(metabolites.glucose, (int)howMuch, this);
            howMuch = (capacity * 12.0) - contents[(int)metabolites.O2];
            howMuch = Math.Max(0, howMuch);
            parent.chloroplast.IWant(metabolites.O2, (int)howMuch, this);
        }

       
        public override void Process()
        {
            //glucose + 6 oxygen -> 38 ATP (+ water, CO2)
            int speed = Math.Min(contents[1], contents[4]/6);   //limitation of speed by amount of resources
            speed = Math.Min(speed, capacity);  //limitation by production capacity
            contents[1] -= speed;   //used up glucose
            contents[4] -= 6 * speed;   //used up oxygen
            contents[0] += 38 * speed;  //created ATP
            if (contents[0] > parent.MAX)
            {
                //if there is too much of a resource, it is sent to the vacuole
                int distanceV;
                distances.TryGetValue(parent.vacuole, out distanceV);
                parent.vacuole.SendResources(metabolites.ATP, contents[0]-parent.MAX, distanceV, this);
                contents[0] = parent.MAX;
            }

        }

        public override void DispatchResources()
        {
            double fraction = 0;
            if (requests != 0)
            {
                fraction = (double)contents[(int)metabolites.ATP] / (double)requests;
            }
            foreach (WaitingRoom beggar in beggars)
            {
                distances.TryGetValue(beggar.GetOrg(), out int distance);
                int amount = (int)(beggar.GetAmount() * fraction);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(metabolites.ATP, amount, distance, this);
                    contents[(int)metabolites.ATP] -= amount;
                }
            }
            if (fraction < 0.35)
            {
                foreach (WaitingRoom beggar in beggars)
                {
                    parent.vacuole.IWant(metabolites.ATP, (int)(beggar.GetAmount() * 0.5), beggar.GetOrg());
                }
            }
            beggars = new List<WaitingRoom>();
            requests = 0;
        }
    }
}