using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Vacuole: Organelle
    {
        new int[] requests = new int[8];    //all resources can be requested
        int capacity;

        public Vacuole(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;
        }

        public override void Init()
        {
            distances.Add(parent.nucleus, 4);
            distances.Add(parent.ribosome, 6);
            distances.Add(parent.er, 7);
            distances.Add(parent.chloroplast, 8);
            distances.Add(parent.mitochondrion, 10);
            distances.Add(parent.membrane, 4);
        }
        
        public override void UpdateAmounts()
        {
            List<Vesicle> dump = new List<Vesicle>();
            for (int i = 0; i < OnTheWay.Count; i++)
            {
                OnTheWay[i].DecreaseDist();
                if (OnTheWay[i].GetDist() < 1)
                {
                    AddResource(OnTheWay[i].GetAmount(), OnTheWay[i].GetRes());
                    dump.Add(OnTheWay[i]);
                    if (contents[(int)OnTheWay[i].GetRes()] > parent.MAX)
                    //in case of overflow discards excess
                    {                        
                        contents[(int)OnTheWay[i].GetRes()] = parent.MAX;
                    }
                }
            }
            foreach (Vesicle vesicle in dump)
            {
                OnTheWay.Remove(vesicle);
            }
            dump = new List<Vesicle>();
        }

        public override void Process()
        {
            //nothing here
        }

        public override void RequestResources()
        {
            //nothing here
        }

        public new void IWant(metabolites what, int amount, Organelle org)
        {
            //all resources can be requested
            requests[(int)what] += amount;
            beggars.Add(new WaitingRoom(amount, org, what));
        }

        public override void DispatchResources()
        {

            foreach (WaitingRoom beggar in beggars)
            {
                metabolites type = beggar.GetRes();
                double fraction = (double)contents[(int)type] / (double)requests[(int)type];
                distances.TryGetValue(beggar.GetOrg(), out int distance);
                int amount = (int)(beggar.GetAmount() * fraction * 0.7);
                if (amount > 0)
                {
                    beggar.GetOrg().SendResources(type, amount, distance, this);
                    contents[(int)type] -= amount;
                    requests[(int)type] -= beggar.GetAmount();
                }
            }
            beggars = new List<WaitingRoom>();
        }
    }
}
