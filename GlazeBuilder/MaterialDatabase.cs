using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GlazeChemistry;

namespace GlazeBuilder
{
    public class MaterialDatabase
    {
        public MaterialDatabase(string raw_chemical_file, string frit_file, string feldspar_file)
        {
            ChemicalDatabase = new ChemicalDatabase("PeriodicTableElements.json", "SimpleMolecules.json", "CompoundMolecules.json");
            Materials = new Dictionary<string, Material>();
            PopulateAllMaterials<RawChemical>(raw_chemical_file);
            //PopulateAllMaterials<Frit>(frit_file);
            //PopulateAllMaterials<Feldspar>(feldspar_file);
        }
        
        ChemicalDatabase ChemicalDatabase { get; set; }
        public Dictionary<string, Material> Materials { get; set; }

        private void PopulateAllMaterials<T>(string known_materials_filename) where T: Material
        {
            string all_materials_text = System.IO.File.ReadAllText(known_materials_filename);
            JContainer all_materials_json_object;

            if (typeof(T) == typeof(RawChemical))
            {
                all_materials_json_object = JArray.Parse(all_materials_text);

                foreach (JToken material in all_materials_json_object.Children())
                {
                    Materials.Add(material.ToString(), (T)Activator.CreateInstance(typeof(T), material, ChemicalDatabase));
                }
            }
            else
            {
                all_materials_json_object = JObject.Parse(all_materials_text);

                foreach (JProperty material in all_materials_json_object.Children())
                {
                    Materials.Add(material.Name, (T)Activator.CreateInstance(typeof(T), material, ChemicalDatabase));
                }
            }
        }
    }
}
