using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GlazeChemistry
{
    // Full dictionary of all simple and compound elements relevant to glaze chemistry, as defined by input files.
    class MolecularDictionary
    {
        // Constructor
        // Takes in a JSON object with all simple molecules, a JSON object with all compound molecules, and a Periodic Table of the Elements.
        public MolecularDictionary(JObject simple_molecules_json, JObject compound_molecules_json, PeriodicTable periodic_table)
        {
            // Instantiates the simple and compound molecule dictionaries to prevent future errors.
            SimpleMolecules = new Dictionary<string, SimpleMolecule>();
            CompoundMolecules = new Dictionary<string, CompoundMolecule>();

            // Iterates through all molecules in the simple molecules JSON object and adds each one to the simple molecules dictionary.
            foreach (JProperty simple_molecule in simple_molecules_json.Children())
            {
                SimpleMolecules.Add(simple_molecule.Name, new SimpleMolecule(simple_molecule, ref periodic_table));
            }

            // Iterates through all molecules in the compound molecules JSON object and adds each one to the compound molecules dictionary.
            foreach (JProperty compound_molecule in compound_molecules_json.Children())
            {
                CompoundMolecules.Add(compound_molecule.Name, new CompoundMolecule(compound_molecule, ref periodic_table, SimpleMolecules));
            }
        }

        // Properties
        // Dictionaries containing all the simple and compound molecules.
        Dictionary<string, SimpleMolecule> SimpleMolecules { get; set; }
        Dictionary<string, CompoundMolecule> CompoundMolecules { get; set; }

        // Function
        // Checks if either dictionary contains a given molecule.
        // Exact case and spelling is required.
        public bool Contains(string molecule_name)
        {
            if (CompoundMolecules.ContainsKey(molecule_name) || SimpleMolecules.ContainsKey(molecule_name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Function
        // Checks if either dictionary contains a molecule.
        // If either does, it returns that molecule.
        // Exact case and spelling is required.
        public Molecule Get(string molecule_name)
        {
            if (CompoundMolecules.ContainsKey(molecule_name))
            {
                return CompoundMolecules[molecule_name];
            }
            else if (SimpleMolecules.ContainsKey(molecule_name))
            {
                return SimpleMolecules[molecule_name];
            }
            else
            {
                return null;
            }
        }
    }
}
