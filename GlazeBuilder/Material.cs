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
    public abstract class Material
    {
        /*
        public Material(JToken material_properties, ChemicalDatabase chemical_database)
        {
            BuildMaterial(material_properties, ref chemical_database);
        }
        public Material(JProperty material_properties, ChemicalDatabase chemical_database)
        {
            BuildMaterial(material_properties, ref chemical_database);
        }
        */

        public string Name { get; set; }


        virtual public void BuildMaterial(JToken material_object, ref ChemicalDatabase chemical_database) { }
        virtual public void BuildMaterial(JProperty material_object, ref ChemicalDatabase chemical_database) { }
    }
}
