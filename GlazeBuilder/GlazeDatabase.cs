using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chemistry;

namespace GlazeBuilder
{
    class GlazeDatabase
    {
        public GlazeDatabase()
        {
            populateCones("PyrometricCones.csv");
        }

        Dictionary<string, Int32[]> Cones { get; set; }
        // Cone array reads Temp C @ 60C/hr, Temp F @ 108F/hr, Temp C @ 150C/hr, Temp F @ 270F/hr
        // Cone temps read left to right off of the Large Cones section on p.89 of Val Cushing's handbook

        void populateCones(string filename)
        {
            string[] allConesText = System.IO.File.ReadAllLines(filename);

            foreach (string element in allConesText)
            {
                var parts = element.Split(',');
                int[] conetemps = new int[parts.Length - 1];
                for (int i = 1; i < parts.Length; i++)
                {
                    conetemps[i - 1] = Convert.ToInt32(parts[i]);
                }
                Cones.Add(parts[0], conetemps);
            }
        }
    }

    class Glaze
    {
        public Glaze()
        {

        }

        // Double is the ratio compared to 100 of the material
        List<Tuple<double, ChemicalFormula>> Materials { get; set; }
        Tuple<string, Int32[]> Cone { get; set; }
        Color FiredColor { get; set; }
        bool Reduction { get; set; }
    }
}
