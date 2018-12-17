using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GlazeChemistry
{
    // SimpleMolecule defines a molecule that contains no sub-molecular structures larger than a single element.
    class SimpleMolecule : Molecule
    {
        // Constructor
        // Takes in a JSON property containing the info of the molecule and a Periodic Table of Elements to define that molecule's atomic structure.
        public SimpleMolecule(JProperty simple_molecule_json_property, PeriodicTable periodic_table)
        {
            // Defines its internal Periodic Table with the input Periodic Table.
            PeriodicTable = periodic_table;

            // Instantiates its Formula and MolecularWeight properties so they can be used later without causing an error.
            Formula = new List<Tuple<Element, int>>();
            MolecularWeight = 0.0;

            // Pulls the name directly from the JSON property. Also instantiates variable for any alternate names.
            Name = simple_molecule_json_property.Name;
            AlternateNames = new List<string>();

            // Casts the value of the JSON property to a JSON object and uses that new JSON object to build the molecule.
            JObject simple_molecule_object = simple_molecule_json_property.Value.ToObject<JObject>();
            BuildMolecule(simple_molecule_object);
        }

        // Properties
        // Properties inherent to a simple molecule.
        List<Tuple<Element, int>> Formula { get; set; }

        // Function
        // Takes in a JSON object to build a simple molecule from its assorted values.
        public override void BuildMolecule(JObject molecule_object)
        {
            // Iterates through the JSON objects properties to populate the information of the molecule.
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
                else if (PeriodicTable.Contains(property.Name))
                {
                    // Searches the Periodic Table for each element, and if it finds it adds it and its relative abundance to the chemical formula.
                    Formula.Add(new Tuple<Element, int>(PeriodicTable.FindElement(property.Name), Convert.ToInt32(property.Value)));
                }
            }

            // Iterates through all elements and adds in their molecular weight multiplied by its relative abundance.
            foreach (Tuple<Element, int> element in Formula)
            {
                MolecularWeight += element.Item1.AtomicWeight * element.Item2;
            }
        }
    }
}
