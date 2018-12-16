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

    class Glaze
    {
        public Glaze()
        {

        }

        // Double is the percentage of 1 of the material in the glaze
        List<Tuple<double, Material>> Materials { get; set; }
        PyrometricCone Cone { get; set; }
        Color FiredColor { get; set; }
        bool Reduction { get; set; }
    }

    class PyrometricCone
    {
        public PyrometricCone(JProperty cone_json_property)
        {
            MakeCone(cone_json_property);
        }

        string Name { get; set; }
        Dictionary<string, int> SmallCones { get; set; }
        Dictionary<string, int> LargeCones { get; set; }
        Dictionary<string, int> PCECones { get; set; }

        private void MakeCone(JProperty cone_json_property)
        {
            Name = cone_json_property.Name;
            
            foreach (JProperty cone_json_subproperty in cone_json_property.Value.ToObject<JObject>().Children())
            {
                if (cone_json_subproperty.Name == "Large Cones")
                {
                    LargeCones = new Dictionary<string, int>();
                    foreach (JProperty temperature_property in cone_json_subproperty.Value.ToObject<JObject>().Children())
                    {
                        LargeCones.Add(temperature_property.Name, Convert.ToInt32(temperature_property.Value));
                    }
                }
                else if (cone_json_subproperty.Name == "Small Cones")
                {
                    SmallCones = new Dictionary<string, int>();
                    foreach (JProperty temperature_property in cone_json_subproperty.Value.ToObject<JObject>().Children())
                    {
                        SmallCones.Add(temperature_property.Name, Convert.ToInt32(temperature_property.Value));
                    }
                }
                else if (cone_json_subproperty.Name == "P.C.E. Cones")
                {
                    PCECones = new Dictionary<string, int>();
                    foreach (JProperty temperature_property in cone_json_subproperty.Value.ToObject<JObject>().Children())
                    {
                        PCECones.Add(temperature_property.Name, Convert.ToInt32(temperature_property.Value));
                    }
                }
            }
        }
    }
}
