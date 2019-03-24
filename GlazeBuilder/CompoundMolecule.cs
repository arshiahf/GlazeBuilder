using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GlazeChemistry
{
    // Class
    // CompoundMolecule defines molecules with sub-molecular structures that may themselves be simple molecules.
    // This and simple molecules were differentiated to track very specific simple molecules important to glaze calculation.
    class CompoundMolecule : Molecule
    {
        // Constructor
        // Requires a JSON property with the compound molecule's information, a Periodic Table, and a dictionary defining all known simple molecules.
        public CompoundMolecule(JProperty compound_molecule_json_property, ref PeriodicTable periodic_table, Dictionary<string, SimpleMolecule> simple_molecule_dictionary)
        {
            // Defines the local copies of the Period Table and the dictionary of simple molecules.
            SimpleMolecules = simple_molecule_dictionary;

            // Instantiates the SubMolecules, AdditionalElements, and MolecularWeight properties to avoid errors.
            SubMolecules = new Dictionary<string, Tuple<SimpleMolecule, int>>();
            AdditionalElements = new List<Tuple<Element, int>>();
            MolecularWeight = 0.0;

            // Pulls name from the JSON property itself. Also instantiating collection for any alternate names.
            Name = compound_molecule_json_property.Name;
            AlternateNames = new List<string>();

            // Casts the JSON property's value as a JSON object then builds the molecule from that object.
            JObject compound_molecule_object = compound_molecule_json_property.Value.ToObject<JObject>();
            BuildMolecule(compound_molecule_object, ref periodic_table);
        }

        // Properties
        // Properties inherent to a compound molecule.
        Dictionary<string, Tuple<SimpleMolecule, int>> SubMolecules { get; set; }
        List<Tuple<Element, int>> AdditionalElements { get; set; }

        // Properties
        // Local copies of the Periodic Table and a dictionary of all relevant simple molecules.
        private Dictionary<string, SimpleMolecule> SimpleMolecules { get; set; }

        // Function
        // Takes the JSON object with all the molecule's information and extracts the data into the molecule's properties.
        public override void BuildMolecule(JObject molecule_object, ref PeriodicTable periodic_table)
        {
            // Iterates through the JSON object's children and extracts each value into a molecular Property.
            foreach (JProperty property in molecule_object.Children())
            {
                if (property.Name == "Alternate Names")
                {
                    foreach (string name in property.Value.ToArray().Children())
                    {
                        AlternateNames.Add(name);
                    }
                }
                else if (property.Name == "Formula")
                {
                    // Extracts the full chemical formula in text string form.
                    FullFormula = property.Value.ToString();
                }
                else if (property.Name == "Submolecules")
                {
                    // Iterates through all submolecules, only adding if it can find it in the dictionary of simple molecules.
                    foreach (JProperty molecule in property.Value.ToObject<JObject>().Children())
                    {
                        if (SimpleMolecules.ContainsKey(molecule.Name))
                        {
                            SubMolecules.Add(molecule.Name, new Tuple<SimpleMolecule, int>(SimpleMolecules[molecule.Name], Convert.ToInt32(molecule.Value)));
                        }
                    }
                }
                else if (periodic_table.Contains(property.Name))
                {
                    // Adds any floating elements after checking for them in the Periodic Table.
                    AdditionalElements.Add(new Tuple<Element, int>(periodic_table.FindElement(property.Name), Convert.ToInt32(property.Value)));
                }
            }

            // Adds the atomic weight of each floating element multiplied by its relative abundance into the molecule's weight.
            foreach (Tuple<Element, int> element in AdditionalElements)
            {
                MolecularWeight += element.Item1.AtomicWeight * element.Item2;
            }

            // Adds the molecular weight of each submolecule multiplied by its relative abundance into the molecule's weight.
            foreach (Tuple<SimpleMolecule, int> molecule in SubMolecules.Values)
            {
                MolecularWeight += molecule.Item1.MolecularWeight * molecule.Item2;
            }
        }
    }
}
