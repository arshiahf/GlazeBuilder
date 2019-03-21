using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GlazeChemistry;

namespace GlazeBuilder
{
    public class GlazeDatabase
    {
        public GlazeDatabase()
        {
            ChemicalDatabase= new ChemicalDatabase("PeriodicTableElements.json", "SimpleMolecules.json", "CompoundMolecules.json");
            PopulateCones("PyrometricCones.json");
        }

        Dictionary<string, PyrometricCone> Cones { get; set; }
        ChemicalDatabase ChemicalDatabase { get; set; }

        void PopulateCones(string pyrometric_cones_filename)
        {
            Cones = new Dictionary<string, PyrometricCone>();
            string all_cones_raw_json = System.IO.File.ReadAllText(pyrometric_cones_filename);
            JObject cones_json = JObject.Parse(all_cones_raw_json);

            foreach (JProperty cone_json_property in cones_json.Children())
            {
                Cones.Add(cone_json_property.Name, new PyrometricCone(cone_json_property));
            }
        }
    }
}
