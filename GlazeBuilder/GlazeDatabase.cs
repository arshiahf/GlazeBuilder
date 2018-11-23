using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlazeBuilder
{
    class GlazeDatabase
    {
        public GlazeDatabase()
        {
            GlazeChemistry = new GlazeChemistry("PeriodicTableElements.json", "SimpleMolecules.json", "CompoundMolecules.json");
            PopulateCones("PyrometricCones.json");
        }

        Dictionary<string, PyrometricCone> Cones { get; set; }
        GlazeChemistry GlazeChemistry { get; set; }

        void PopulateCones(string pyrometric_cones_filename)
        {
            string all_cones_raw_json = System.IO.File.ReadAllText(pyrometric_cones_filename);
            JObject cones_json = JObject.Parse(all_cones_raw_json);

            foreach (JObject cone in cones_json.Children())
            {

            }
        }
    }

    class Glaze
    {
        public Glaze()
        {

        }

        // Double is the ratio compared to 100 of the material
        List<Tuple<double, Material>> Materials { get; set; }
        PyrometricCone Cone { get; set; }
        Color FiredColor { get; set; }
        bool Reduction { get; set; }
    }

    class PyrometricCone
    {
        PyrometricCone(string coneRaw)
        {
            JObject coneJson = JObject.Parse(coneRaw);
        }
    }
}
