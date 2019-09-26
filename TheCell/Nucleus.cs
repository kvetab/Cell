using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Nucleus: Organelle, IOrganelle
    {
        int capacity;

        public Nucleus(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;            
            contents[0] = capacity * 20; //initial resources
            contents[4] = capacity * 8;
            contents[5] = capacity * 20;
        }

        public override void Init()
        {
            distances.Add(parent.ribosome, 4);
            distances.Add(parent.vacuole, 3);
            distances.Add(parent.chloroplast, 8);
        }

        public override void Process()
        {
            //10 ATP + 4 O2 + 10 phosphorus -> mRNA
            int speed = Math.Min(contents[0]/10, contents[4] / 4);   //limitation of speed by amount of resources
            speed = Math.Min(speed, contents[5] / 10);
            speed = Math.Min(speed, capacity);  //limitation by production capacity
            contents[0] -= speed * 10;   //used up ATP
            contents[4] -= 4 * speed;   //used up oxygen
            contents[5] -= 10 * speed;  //used up phosphorus
            contents[2] += speed;   //created mRNA            
            if (contents[2] > parent.MAX)
            {
                //if there is too much of a resource, it is sent to the vacuole
                int distanceV;
                distances.TryGetValue(parent.vacuole, out distanceV);
                parent.vacuole.SendResources(metabolites.ATP, contents[2] - parent.MAX, distanceV, this);
                contents[2] = parent.MAX;
            }
        }

        public override void RequestResources()
        {
            double howMuch = (capacity * 20.0) - contents[(int)metabolites.ATP];   //can request max to fill up to 200% its capacity
            howMuch = Math.Max(0, howMuch);
            parent.mitochondrion.IWant(metabolites.ATP, (int)howMuch, this);
            howMuch = (capacity * 8.0) - contents[(int)metabolites.O2];
            howMuch = Math.Max(0, howMuch);
            parent.chloroplast.IWant(metabolites.O2, (int)howMuch, this);
            howMuch = (capacity * 10.0) - contents[(int)metabolites.P]; //lower than elsewhere to avoid hoarding
            howMuch = Math.Max(0, howMuch);
            parent.puddle.IWant(metabolites.P, (int)howMuch, this);
        }

        public override void DispatchResources()
        {
            double fraction = 0;
            if (requests != 0)
            {
                fraction = (double)contents[(int)metabolites.mRNA] / (double)requests;
            }
            foreach (WaitingRoom beggar in beggars)
            {
                distances.TryGetValue(beggar.GetOrg(), out int distance);
                int amount = (int)(beggar.GetAmount() * fraction);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(metabolites.mRNA, amount, distance, this);
                    contents[(int)metabolites.mRNA] -= amount;
                }
            }
            if (fraction < 0.35)
            {
                foreach (WaitingRoom beggar in beggars)
                {
                    parent.vacuole.IWant(metabolites.mRNA, (int)(beggar.GetAmount() * 0.5), beggar.GetOrg());
                }
            }
            beggars = new List<WaitingRoom>();
            requests = 0;
        }
    }
}
