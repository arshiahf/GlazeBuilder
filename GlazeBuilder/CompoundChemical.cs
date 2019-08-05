using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GlazeChemistry;

namespace GlazeBuilder
{
    class CompoundChemical : Material
    {
        public CompoundChemical(JProperty material_properties, ChemicalDatabase chemical_database)
        {
            Compound = new Dictionary<string, Tuple<Molecule, double>>();
            AlternateNames = new List<string>();
            BuildMaterial(material_properties, ref chemical_database);
        }

        public Dictionary<string, Tuple<Molecule, double>> Compound { get; set; }
        public List<string> AlternateNames { get; set; }

        public override void BuildMaterial(JProperty material_object, ref ChemicalDatabase chemical_database)
        {
            Name = material_object.Name;
            JObject material_details = material_object.Value.ToObject<JObject>();
            foreach (JProperty material_property in material_details.Children())
            {
                if (material_property.Name == "Alternate Names")
                {
                    foreach (string alternate_name in material_property.Value.ToArray())
                    {
                        AlternateNames.Add(alternate_name);
                    }
                }
                else if (material_property.Name != "Alternate Names")
                {
                    foreach (JProperty material_component in material_property.Value.ToObject<JObject>().Children())
                    {
                        Compound.Add(material_component.Name, Tuple.Create<Molecule, double>(chemical_database.LookupMolecule(material_component.Name), material_component.Value.ToObject<Double>()));
                    }
                }
            }
        }
    }
}
