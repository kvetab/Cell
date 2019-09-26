using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class Membrane: Organelle
    {
        new int contents;   //contains only one metabolite
        int capacity;   

        public Membrane(int capacity, Cell parent)
        {
            this.capacity = capacity;
            this.parent = parent;
            this.contents = capacity;
        }

        public override void Init()
        {
            //no initialization needed
        }

        public new int GetAmount(metabolites what)
        {
            if (what == metabolites.phospholipids)
            {
                return contents;
            }
            else return 0;
        }

        public override void Process()
        {
            //phospholipids are gradually destroyed by UV
            double rate = (parent.GetUV() * 0.7 / 100.0);
            contents = (int)(contents * (1-rate));            
        }

        public override void RequestResources()
        {
            int amount = (int)((capacity * 1.3) - contents);
            amount = Math.Max(0, amount);
            parent.er.IWant(metabolites.phospholipids, amount, this);
        }

        public override void DispatchResources()
        {
            //nothing here
        }
    }
}
