using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class WaitingRoom
    {
        Organelle organelle;
        int amount;
        metabolites resource;

        public WaitingRoom(int amount, Organelle org, metabolites what)
        {
            this.amount = amount;
            organelle = org;
            resource = what;
        }

        public Organelle GetOrg()
        {
            return organelle;
        }

        public int GetAmount()
        {
            return amount;
        }
        public metabolites GetRes()
        {
            return resource;
        }
    }
}
