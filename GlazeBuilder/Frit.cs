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
    class Frit : Material
    {
        public Frit(JProperty material_properties, ChemicalDatabase chemical_database) : base(material_properties, chemical_database)
        {
            BuildMaterial(material_properties, ref chemical_database);
        }

        public Dictionary<string, Tuple<Molecule, double>> MolecularList { get; set; }

        public override void BuildMaterial(JProperty material_object, ref ChemicalDatabase chemical_database)
        {

        }
    }
}
