using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Chloroplast: Organelle, IOrganelle
    {
        int requestsO = 0;
        int requestsG = 0;  //different from base class - needs two kinds of requests
        int capacity;
        List<WaitingRoom> beggarsG = new List<WaitingRoom>();
        List<WaitingRoom> beggarsO = new List<WaitingRoom>();
        //list of requests for resources produced by this organelle      
        
                
        
        public Chloroplast(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;
        }
        public override void Init()
        {
            distances.Add(parent.mitochondrion, 5);
            distances.Add(parent.ribosome, 5);
            distances.Add(parent.er, 6);
            distances.Add(parent.vacuole, 9);
        }

        public override void Process()
        {
            //creates O2 and glucose in a ratio of 6:1
            double speedFirst = (double)capacity * (double)parent.GetSunshine() / 100;    //limitation of speed by amount of sunlight only
            int speed = (int)speedFirst;
            
            contents[1] += speed / 3;  //created glucose
            contents[4] += (speed / 3) * 6;  //created oxygen
            if (contents[1] > parent.MAX)
            {
                //if there is too much of glucose, it is sent to the vacuole
                int distanceV;
                distances.TryGetValue(parent.vacuole, out distanceV);
                parent.vacuole.SendResources(metabolites.glucose, contents[1] - parent.MAX, distanceV, this);
                contents[1] = parent.MAX;
            }
            if (contents[4] > parent.MAX)
            {
                //if there is too much of oxygen, it is sent to the vacuole
                int distanceV;
                distances.TryGetValue(parent.vacuole, out distanceV);
                parent.vacuole.SendResources(metabolites.O2, contents[4] - parent.MAX, distanceV, this);
                contents[4] = parent.MAX;
            }
        }

        public override void RequestResources()
        {
            //nothing here - the chloroplast needs only freely available things
        }

        public override void DispatchResources()
            //sends requested resources to organelles - unlike elswhere there are two possible types
        {
            double fractionO = 0;
            double fractionG = 0;
            //available fraction of both requested resources
            if (requestsO != 0)
            {
                fractionO = (double)contents[(int)metabolites.O2] / (double)requestsO;
            }
            if (requestsG != 0)
            {
                fractionG = (double)contents[(int)metabolites.glucose] / (double)requestsG;
            }
            foreach (WaitingRoom beggar in beggarsG)
            {
                distances.TryGetValue(beggar.GetOrg(), out int distance);
                int amount = 0;
                amount = (int)(beggar.GetAmount() * fractionG);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(metabolites.glucose, amount, distance, this);
                    contents[1] -= amount;
                }
            }
            foreach (WaitingRoom beggar in beggarsO)
            {
                distances.TryGetValue(beggar.GetOrg(), out int distance);
                int amount = 0;
                amount = (int)(beggar.GetAmount() * fractionO);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(metabolites.O2, amount, distance, this);
                    contents[4] -= amount;
                }
            }
            //if there is too little available, a request is sent to the vacuole:
            if (fractionG < 0.35)
            {
                foreach (WaitingRoom beggar in beggarsG)
                {
                    parent.vacuole.IWant(metabolites.glucose, (int)(beggar.GetAmount() * 0.5), beggar.GetOrg());
                }
            }
            if (fractionO < 0.35)
            {
                foreach (WaitingRoom beggar in beggarsO)
                {
                    parent.vacuole.IWant(metabolites.O2, (int)(beggar.GetAmount() * 0.5), beggar.GetOrg());
                }
            }
            beggarsO = new List<WaitingRoom>();
            beggarsG = new List<WaitingRoom>();
            requestsG = 0;
            requestsO = 0; //reset for next step
        }

        public new void IWant(metabolites what, int amount, Organelle org)
            //requests resources from chloroplast - unlike other organelles there are two types available
        {
            if (what == metabolites.O2)
            {
                requestsO += amount;
                beggarsO.Add(new WaitingRoom(amount, org, what));
            }
            if (what == metabolites.glucose)
            {
                requestsG += amount;
                beggarsG.Add(new WaitingRoom(amount, org, what));
            }
            ;
        }
    }
}
