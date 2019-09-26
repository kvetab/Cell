using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    public enum metabolites { ATP, glucose, mRNA, N, O2, P, phospholipids, proteins, whoKnows };

    class Puddle : Organelle, IOrganelle
    {
        int requestsP = 0;
        int requestsN = 0;
        int capacity;
        List<WaitingRoom> beggarsN = new List<WaitingRoom>();
        List<WaitingRoom> beggarsP = new List<WaitingRoom>();
        //list of requests for resources produced by this organelle
        int distance = 3;

        Random random = new Random();

        public Puddle(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;
            contents[3] = capacity; //initial resources
            contents[5] = capacity * 3;
        }

        public override void Init()
        {

        }

        public override void Process()
        {
            //refills sources of N and P acording to settings with a small factor of randomness
            int rand = random.Next(8);
            contents[3] = parent.GetN() * capacity / 100 - rand;
            contents[5] = parent.GetP() * capacity * 3 / 100 + rand;
        }

        public override void RequestResources()
        {
            //nothing here
        }

        public override void DispatchResources()
        {
            double fractionN = 0;
            double fractionP = 0;
            if (requestsN != 0)
            {
                fractionN = (double)contents[(int)metabolites.N] / (double)requestsN;
            }
            if (requestsP != 0)
            {
                fractionP = (double)contents[(int)metabolites.P] / (double)requestsP;
            }
            foreach (WaitingRoom beggar in beggarsP)
            {
                int amount = 0;
                amount = (int)(beggar.GetAmount() * fractionP);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(metabolites.P, amount, distance, this);
                    contents[5] -= amount;
                }
            }
            foreach (WaitingRoom beggar in beggarsN)
            {
                int amount = 0;
                amount = (int)(beggar.GetAmount() * fractionN);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(metabolites.N, amount, distance, this);
                    contents[4] -= amount;
                }
            }
            beggars = new List<WaitingRoom>();
            requestsN = 0;
            requestsP = 0;
        }

        public new void IWant(metabolites what, int amount, Organelle org)
        //requests resources from puddle - unlike other organelles there are two types available
        {
            if (amount > 0)
            {
                if (what == metabolites.P)
                {
                    requestsP += amount;
                    beggarsP.Add(new WaitingRoom(amount, org, what));
                }
                if (what == metabolites.N)
                {
                    requestsN += amount;
                    beggarsN.Add(new WaitingRoom(amount, org, what));
                }            
            }
        }

    }
}
