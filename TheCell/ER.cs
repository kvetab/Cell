using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class ER: Organelle, IOrganelle
    {        
        int capacity;

        public ER(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;
            contents[0] = capacity * 20; //initial resources
            contents[1] = capacity * 2;
            contents[5] = capacity * 20;
        }
        public override void Init()
        {
            distances.Add(parent.membrane, 5);
            distances.Add(parent.vacuole, 7);
        }

        public override void Process()
        {
            //10 ATP + glucose + 10 phosphorus -> phospholipid

            int speed = Math.Min(contents[0] / 10, contents[1]);   //limitation of speed by amount of resources
            speed = Math.Min(speed, contents[5] / 10);
            speed = Math.Min(speed, capacity);  //limitation by production capacity
            contents[0] -= speed * 10;   //used up ATP
            contents[1] -= speed;   //used up glucose
            contents[5] -= 10 * speed;  //used up phosphorus
            contents[6] += speed;   //created phospholipids           
            if (contents[6] > parent.MAX)
            {
                //if there is too much of a resource, it is sent to the vacuole
                int distanceV;
                distances.TryGetValue(parent.vacuole, out distanceV);
                parent.vacuole.SendResources(metabolites.phospholipids, contents[6] - parent.MAX, distanceV, this);
                contents[6] = parent.MAX;
            }
        }

        public override void RequestResources()
        {
            double howMuch = (capacity * 20.0) - contents[(int)metabolites.ATP];   //can request max to fill up to 200% its capacity
            howMuch = Math.Max(0, howMuch);
            parent.mitochondrion.IWant(metabolites.ATP, (int)howMuch, this);
            howMuch = (capacity * 4.0) - contents[(int)metabolites.glucose];   //higher request than elsewhere
            howMuch = Math.Max(0, howMuch);
            parent.chloroplast.IWant(metabolites.glucose, (int)howMuch, this);
            howMuch = (capacity * 10.0) - contents[(int)metabolites.P]; //lower
            howMuch = Math.Max(0, howMuch);
            parent.puddle.IWant(metabolites.P, (int)howMuch, this);
        }

        public override void DispatchResources()
        {
            double fraction = 0;
            if (requests != 0)
            {
                fraction = (double)contents[(int)metabolites.phospholipids] / (double)requests;
                //fraction that is available of requested resources
            }
            foreach (WaitingRoom beggar in beggars)
            {
                //sends availabe amounts using SendResources
                distances.TryGetValue(beggar.GetOrg(), out int distance);
                int amount = Math.Min(beggar.GetAmount(), contents[(int)beggar.GetRes()]);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(metabolites.phospholipids, amount, distance, this);
                    contents[(int)metabolites.phospholipids] -= amount;
                }
            }
            if (fraction < 0.35)
            {
                //if the available amount is too small, a request is sent to the vacuole
                foreach (WaitingRoom beggar in beggars)
                {
                    parent.vacuole.IWant(metabolites.mRNA, (int)(beggar.GetAmount() * 0.5), beggar.GetOrg());
                }
            }
            beggars = new List<WaitingRoom>();  //reset for next step
            requests = 0;
        }
    }
}
