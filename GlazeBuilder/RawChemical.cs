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
    class RawChemical : Material
    {
        public RawChemical(JToken material_properties, ChemicalDatabase chemical_database) : base(material_properties, chemical_database)
        {
            BuildMaterial(material_properties, ref chemical_database);
        }

        Tuple<Molecule, double> Compound { get; set; }

        public override void BuildMaterial(JToken material_object, ref ChemicalDatabase chemical_database)
        {
            Name = material_object.ToString();
            Compound = Tuple.Create<Molecule, double>(chemical_database.LookupMolecule(material_object.ToString()), 1);
        }
    }
}
