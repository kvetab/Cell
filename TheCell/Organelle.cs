using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    abstract class Organelle: IOrganelle
    {
        protected Cell parent;
        protected int[] contents = new int[8];  //the metabolites in an organelle
        protected int requests = 0;     //the amount of product requested by other organelles
        protected List<Vesicle> OnTheWay = new List<Vesicle>();
        protected List<WaitingRoom> beggars = new List<WaitingRoom>();
            //list of requests for resources produced by this organelle

        protected Dictionary<Organelle, int> distances = new Dictionary<Organelle, int>();
        protected List<Position> counters = new List<Position>();   //positions of counters, that show amounts of resources


        abstract public void Process();
            //specific for each subclass

        public int[] GetContents()
        {
            return contents;
        }
        public int GetAmount(metabolites what)
        {
            return contents[(int)what];
        }
        public int GetAmount(int i)
        {
            return contents[i];
        }

        public void SetCounterPos(List<Position> pos)
        {
            counters = pos;
        }
    

        virtual public void UpdateAmounts()
            //adds arriving vesicules to contents, decreases distances of vesicules travelling to this organelle
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
                        //in case of overflow sends excess to vacuole
                    {
                        int distanceV;
                        distances.TryGetValue(parent.vacuole, out distanceV);
                        parent.vacuole.SendResources
                            (OnTheWay[i].GetRes(), contents[(int)OnTheWay[i].GetRes()] - parent.MAX, distanceV, this);
                        contents[(int)OnTheWay[i].GetRes()] = parent.MAX;
                    }
                }
            }
            foreach (Vesicle vesicle in dump)
            {
                OnTheWay.Remove(vesicle);
            }
            dump = new List<Vesicle>();
            parent.AddVesicles(OnTheWay);
        }
        //public int GetProduct(int amount, metabolites what)
        //{
        //    if (contents[(int)what] >= amount)
        //    {
        //        contents[(int)what] -= amount;
        //        return amount;
        //    }
        //    else
        //    {
        //        int available = contents[(int)what];
        //        contents[(int)what] = 0;
        //        return available;
        //    }
        //}

        public void AddResource(int amount, metabolites what)
            //adds a resource to the contents array
        {
            contents[(int)what] += amount;
        }

        public void IWant(metabolites what, int amount, Organelle org)
        //requests resources from this organelle (later processed by DispatchResources)
        {
            if (amount > 0)
            {
                requests += amount;
                beggars.Add(new WaitingRoom(amount, org, what));
            }
        }

        public void SendResources(metabolites what, int amount, int distance, Organelle who)
            //allows resources to be sent to this organelle in a vesicule
        {            
            OnTheWay.Add(new Vesicle(what, distance, amount, who, this));
        }

        abstract public void DispatchResources();
        abstract public void RequestResources();
        abstract public void Init();
            //specific for each subclass

        public List<Position> GetCounters()
        {
            return counters;
        }

    }
}
