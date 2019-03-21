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
    class Material
    {
        public Material(JProperty material_json_property)
        {
            Name = material_json_property.Name;
        }

        string Name { get; set; }
        List<CompoundMolecule> CompoundMolecules { get; set; }
        List<SimpleMolecule> SimpleMolecules { get; set; }
    }
}
