using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GlazeChemistry
{
    // Generic molecule.
    // Acts as a framework for other molecules, and allows functions to call their overtype rather than the specific subtypes.
    public abstract class Molecule
    {
        // Properties
        // Properties inherent to a molecule.
        public string Name { get; set; }
        public List<string> AlternateNames { get; set; }
        public string FullFormula { get; set; }
        public double MolecularWeight { get; set; }

        // BuildMolecule template
        // Creates a required function to build a molecule
        abstract public void BuildMolecule(JObject molecule_object, ref PeriodicTable periodic_table);
    }
}
