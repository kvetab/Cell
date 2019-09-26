using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TheCell
{
    class Visualizer
    {
        Cell cell;
        Bitmap background;
        Bitmap view;    
        Graphics graphics;
        Graphics gr;
        Brush bcgrndBrush = new SolidBrush(Color.Gray);
        InputReader ir;
        System.Windows.Forms.PictureBox pb;

        Dictionary<Organelle, Dictionary<Organelle, Path>> paths = new Dictionary<Organelle, Dictionary<Organelle, Path>>();



        public Visualizer(Cell cell, Graphics g, System.Windows.Forms.PictureBox pb)
        {
            this.cell = cell;
            graphics = g;
            this.pb = pb;
            background = (Bitmap) Image.FromFile("Cell.png");
            view = (Bitmap)Image.FromFile("Cell.png");
            ir = new InputReader("input.txt", cell);
            paths = ir.GetInputFromFile();
            gr = Graphics.FromImage(view);
        }

        public void Show()
            //visualizes one step of the model - positions and sizes of all counters
        {           
            gr.DrawImage(background, 0, 0, view.Width, view.Height);

            /// tady se to zasekava pri zmene rychlosti. Proc???
            
            foreach (Vesicle vesicle in cell.GetVesicles())
            {
                DrawVesicle(vesicle);
            }
            foreach (Organelle organelle in cell.GetOrganelles())
            {
                foreach (Position counter in organelle.GetCounters())
                {
                    DrawCounter(counter, organelle.GetAmount(counter.GetResource()));
                }
            }
            graphics.DrawImage(view, 0, 0, pb.Width, pb.Height);
        }

        public void Redraw()
        {
            graphics.DrawImage(background, 0, 0, pb.Width, pb.Height);
        }

        public void SetBackgrnd(Bitmap image)
        {
            background = image;
        }
        public void SetPaths(Dictionary<Organelle, Dictionary<Organelle, Path>> paths)
        {
            this.paths = paths;
        }

        void Init()
        {
            //replaced by input from file
        }


        Color GetColor(metabolites met)
        {
            switch ((int)met)
            {
                case 0: return Color.Red; break;
                case 1: return Color.LightCoral; break;
                case 2: return Color.LightSteelBlue; break;
                case 3: return Color.MediumTurquoise; break;
                case 4: return Color.Navy; break;
                case 5: return Color.Violet; break;
                case 6: return Color.Gold; break;
                case 7: return Color.OliveDrab; break;
            }
            return Color.Gray;
        }      
                
        int GetRadius(int amount)
        {
            double radius = Math.Sqrt(amount) * 2;
            return (int)radius;
        }

        void DrawVesicle(Vesicle vesicle)
            //draws a circle representing a traveling vesicule
        {
            Path path;
            if (paths.ContainsKey(vesicle.GetOrigin()))
            {
                if (paths[vesicle.GetOrigin()].TryGetValue(vesicle.GetDestination(), out path))
                {
                    //adds a number of "jumps" to the start
                    int move = path.GetLength() - vesicle.GetDist();
                    int x = (int)((move * path.GetIntervalX() + path.GetStartX()) * (view.Width / 983.0));
                    int y = (int)((move * path.GetIntervalY() + path.GetStartY()) * (view.Height / 616.0));
                    int radius = GetRadius(vesicle.GetAmount());
                    x -= (2 * radius);
                    y -= (2 * radius);

                    using (SolidBrush brush = new SolidBrush(GetColor(vesicle.GetRes())))
                    {
                        gr.FillEllipse(brush, x , y, 4 * radius, 4 * radius);
                    }
                    

                }
            }

        }

        void DrawCounter(Position where, int amount)
            //draws a circle representing resources in an organelle
        {
            int radius = GetRadius(amount) / 2;
            int x = (int)((where.GetX()) * (view.Width / 983.0));
            int y = (int)((where.GetY()) * (view.Height / 616.0));
            using (SolidBrush brush = new SolidBrush(GetColor(where.GetResource())))
            {                
                gr.FillEllipse(brush, x - radius, y - radius, 2 * radius, 2 * radius);
            }
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                x -= 7;
                y -= 7;
                gr.DrawString(amount.ToString(), new Font("Arial", 14), brush, x, y);

            }

        }
    }
}
   
        
    


