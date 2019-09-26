using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCell
{
    class InputReader
    {
        System.IO.StreamReader sr;
        Dictionary<Organelle, Dictionary<Organelle, Path>> dict;
        Cell cell;

        public InputReader(string filename, Cell cell)
        {
            sr = new System.IO.StreamReader(filename);
            this.cell = cell;
        }

        public Dictionary<Organelle, Dictionary<Organelle, Path>> GetInputFromFile()
        {
            //creates a dictionary of all organelles and paths between them 
            //and adds a list of counters to each organelle
            dict = new Dictionary<Organelle, Dictionary<Organelle, Path>>();
            string text = sr.ReadToEnd();
            string[] first = text.Split('/');
            ProcessPaths(first[1]);
            ProcessCounters(first[3]);
            return dict;
        }

        void ProcessPaths(string paths)
        {
            //saves paths in the dictionary of the starting organelle
            string[] lines = paths.Split(';');
            foreach (string line in lines)
            {
                string[] path = line.Trim().Split(',');
                Organelle org = GetOrganelleByName(path[0].Trim());
                if (dict.ContainsKey(org))
                {
                    Organelle end = GetOrganelleByName(path[1].Trim());
                    dict[org].Add(end, new Path
                        (org, end, int.Parse(path[2]), int.Parse(path[3]), int.Parse(path[4]), int.Parse(path[5]), int.Parse(path[6])));
                }
                else
                {
                    dict.Add(org, new Dictionary<Organelle, Path>());
                    Organelle end = GetOrganelleByName(path[1].Trim());
                    dict[org].Add(end, new Path
                        (org, end, int.Parse(path[2]), int.Parse(path[3]), int.Parse(path[4]), int.Parse(path[5]), int.Parse(path[6])));
                }
            }
        }

        void ProcessCounters(string counters)
        {
            string[] lines = counters.Split(';');            
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                List<Position> cntrs = new List<Position>();
                Organelle org = GetOrganelleByName(parts[0].Trim());
                string[] resources = parts[1].Split('-');
                foreach (string resource in resources)
                {
                    string[] pos = resource.Split(',');
                    cntrs.Add(new Position(GetMetByName(pos[0].Trim()), int.Parse(pos[1]), int.Parse(pos[2])));
                }
                org.SetCounterPos(cntrs);
            }
        }


        Organelle GetOrganelleByName(string name)
        {
            switch (name.ToLower())
            {
                case "chloroplast": return cell.chloroplast;
                case "er": return cell.er;
                case "mitochondrion": return cell.mitochondrion;
                case "membrane": return cell.membrane;
                case "nucleus": return cell.nucleus;
                case "puddle": return cell.puddle;
                case "ribosome": return cell.ribosome;
                case "vacuole": return cell.vacuole;            
            }
            return null;
        }

        metabolites GetMetByName(string name)
        {
            switch (name.ToLower())
            {
                case "atp": return metabolites.ATP;
                case "glucose": return metabolites.glucose;
                case "mrna": return metabolites.mRNA;
                case "n": return metabolites.N;
                case "o2": return metabolites.O2;
                case "p": return metabolites.P;
                case "phospholipids": return metabolites.phospholipids;
                case "proteins": return metabolites.proteins;
            }
            return metabolites.whoKnows;

        }
    }
}
