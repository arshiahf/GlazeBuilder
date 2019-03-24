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
            MaterialDatabase = new MaterialDatabase("MaterialsRawChemicals.json", "MaterialsFrits.json", "MaterialsFeldspars.json");

            Cones = new Dictionary<string, PyrometricCone>();
            PopulateCones("PyrometricCones.json");

            Glazes = new Dictionary<string, Glaze>();
        }

        public Dictionary<string, PyrometricCone> Cones { get; set; }
        public MaterialDatabase MaterialDatabase { get; set; }
        public Dictionary<string, Glaze> Glazes { get; set; }

        void PopulateCones(string pyrometric_cones_filename)
        {
            string all_cones_raw_json = System.IO.File.ReadAllText(pyrometric_cones_filename);
            JObject cones_json = JObject.Parse(all_cones_raw_json);

            foreach (JProperty cone_json_property in cones_json.Children())
            {
                Cones.Add(cone_json_property.Name, new PyrometricCone(cone_json_property));
            }
        }

        void PopulateKnownGlazes(string known_glazes_filename)
        {
            string all_materials_text = System.IO.File.ReadAllText(known_glazes_filename);
            JObject all_materials_json_object = JObject.Parse(all_materials_text);
        }


    }
}
