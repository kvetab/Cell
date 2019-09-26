using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Cell
    {
        public Puddle puddle;
        public Mitochondrion mitochondrion;
        public Chloroplast chloroplast;
        public Ribosome ribosome;
        public Nucleus nucleus;
        public ER er;
        public Vacuole vacuole;
        public Membrane membrane;
        //all contained organelles

        List<Organelle> organelles = new List<Organelle>();
        List<Vesicle> vesicles = new List<Vesicle>();

        int sunshine;
        int UV;
        int N;
        int P;
        public int MAX = 1500;
        //maximum amount of one resource in an organelle

        public Cell()
        {
            sunshine = 50;
            UV = 50;
            N = 50;
            P = 50;
            chloroplast = new Chloroplast(20, this);
            er = new ER(5, this);
            membrane = new Membrane(10, this);
            mitochondrion = new Mitochondrion(12, this);
            nucleus = new Nucleus(5, this);
            puddle = new Puddle(18, this);
            ribosome = new Ribosome(3, this);
            vacuole = new Vacuole(6, this);
            //initial settings

            organelles.Add(er);
            organelles.Add(mitochondrion);
            organelles.Add(membrane);
            organelles.Add(chloroplast);
            organelles.Add(nucleus);
            organelles.Add(puddle);
            organelles.Add(ribosome);
            organelles.Add(vacuole);

            foreach (Organelle org in organelles)
            {
                org.Init();
            }
            
        }

        public void Run()
            //one step of the simulation
        {
            vesicles = new List<Vesicle>(); 
            foreach (Organelle org in organelles)
            {
                org.UpdateAmounts();
            }
            foreach (Organelle org in organelles)
            {
                org.Process();
            }
            foreach (Organelle org in organelles)
            {
                org.RequestResources();
            }
            foreach (Organelle org in organelles)
            {
                org.DispatchResources();
            }
        }
        

        public int GetSunshine()
        {
            return sunshine;
        }
        public int GetUV()
        {
            return UV;
        }
        public int GetP()
        {
            return P;
        }
        public int GetN()
        {
            return N;
        }

        public void SetSunshine(int newLevel)
        {
            sunshine = newLevel;
        }
        public void SetUV(int newLevel)
        {
            UV = newLevel;
        }
        public void SetN(int newLevel)
        {
            N = newLevel;
        }
        public void SetP(int newLevel)
        {
            P = newLevel;
        }
        public void AddVesicles(List<Vesicle> list)
        {
            vesicles.AddRange(list);
        }
        public List<Vesicle> GetVesicles()
        {
            return vesicles;
        }
        public List<Organelle> GetOrganelles()
        {
            return organelles;
        }
    }
}
