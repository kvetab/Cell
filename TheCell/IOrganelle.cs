using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    interface IOrganelle
    {

        void UpdateAmounts();
        void Process();
        void RequestResources();
        void DispatchResources();

        int[] GetContents();
        int GetAmount(metabolites what);
        void AddResource(int amount, metabolites what);
    }
}
