using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*
    GlazeChemistry.cs

    The purpose of this .cs is to contain all items for glaze calculation that are directly chemistry related in one concise area.
    The properties allotted to each item will be expanded as need arises.
    The JSON files linked here are intended to be PeriodicTableElements.json, SimpleMolecules.json, and CompoundMolecules.json.
    This .cs was not intended to contain the materials or other glaze specific properties. Only the root elements and the molecules they create.
*/

namespace GlazeBuilder
{
    // Class
    // GlazeChemistry manages all the purely chemical functionality of the GlazeBuilder namespace.
    // It controls the elements, molecules, and atomic/molecular weights of each component piece.
    class GlazeChemistry
    {
        // Constructor
        // Requires, in order, a file for the elements, a file for the simple molecules, and a file for the compound molecules, all in JSON.
        public GlazeChemistry(string elements_filename, string simple_molecules_filename, string compound_molecules_filename)
        {
            // Extracts the text for the elements into a string then parses that string into a JSON object.
            string elements_file = System.IO.File.ReadAllText(elements_filename);
            JObject elements_full_json = JObject.Parse(elements_file);

            // Populates the Periodic Table of Elements using the elements JSON object.
            PeriodicTable = new PeriodicTable(elements_full_json);
             
            // Extracts the text for the simple molecules into a string then parses that string into a JSON object.
            string simple_molecules_file = System.IO.File.ReadAllText(simple_molecules_filename);
            JObject simple_molecules_json = JObject.Parse(simple_molecules_file);

            // Extracts the text for the compound molecules into a string then parses that string into a JSON object.
            string compound_molecules_file = System.IO.File.ReadAllText(compound_molecules_filename);
            JObject compound_molecules_json = JObject.Parse(compound_molecules_file);

            // Populates the molecular dictionary with the simple molecule JSON, the compound molecule JSON, and the Periodic Table of Elements.
            MolecularDictionary = new MolecularDictionary(simple_molecules_json, compound_molecules_json, PeriodicTable);
        }

        // Properties
        // Declaration for the Periodic Table of Elements and Molecular Dictionary properties inherent to the GlazeChemistry class.
        PeriodicTable PeriodicTable { get; set; }
        MolecularDictionary MolecularDictionary { get; set; }

        // Function
        // Looks up any given molecule. Returns a generic container with the molecule in it.
        public GenericMolecule LookupMolecule(string molecule_name)
        {
            if (MolecularDictionary.Contains(molecule_name))
            {
                return MolecularDictionary.Get(molecule_name);
            }
            else
            {
                return null;
            }
        }
    }

    // Class
    // Element defines a basic element for the Periodic Table of Elements to use.
    class Element
    {
        // Constructor
        // Requires a JSON property with the details of the element inside.
        public Element(JProperty element_json_property)
        {
            // Pulls the name from the name of the JSON property itself.
            Name = element_json_property.Name;

            // Gets the value of the JSON property and casts it as a JSON Object.
            JObject element_json_object = element_json_property.Value.ToObject<JObject>();

            // Gets assorted values from the JSON object and assigns them to the properties of the element.
            AtomicSymbol = element_json_object.GetValue("AtomicSymbol").ToString();
            AtomicNumber = Convert.ToInt32(element_json_object.GetValue("AtomicNumber"));
            AtomicWeight = Convert.ToDouble(element_json_object.GetValue("AtomicWeight"));
        }

        // Properties
        // Inherent properties of any given element.
        public string Name { get; set; }
        public string AtomicSymbol { get; set; }
        public int AtomicNumber { get; set; }
        public double AtomicWeight { get; set; }
    }

    // Class
    // SimpleMolecule defines a molecule that contains no sub-molecular structures larger than a single element.
    class SimpleMolecule
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

            // Pulls the name directly from the JSON property.
            Name = simple_molecule_json_property.Name;

            // Casts the value of the JSON property to a JSON object and uses that new JSON object to build the molecule.
            JObject simple_molecule_object = simple_molecule_json_property.Value.ToObject<JObject>();
            BuildSimpleMolecule(simple_molecule_object);
        }

        // Properties
        // Properties inherent to a simple molecule.
        public string Name { get; set; }
        public string FullFormula { get; set; }
        List<Tuple<Element, int>> Formula { get; set; }
        public double MolecularWeight { get; set; }

        // Property
        // Internal Periodic Table of Elements used when assembling the molecule.
        private PeriodicTable PeriodicTable { get; set; }

        // Function
        // Takes in a JSON object to build a simple molecule from its assorted values.
        void BuildSimpleMolecule(JObject simple_molecule_object)
        {
            // Iterates through the JSON objects properties to populate the information of the molecule.
            foreach (JProperty property in simple_molecule_object.Children())
            {
                if (property.Name == "Formula")
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

    // Class
    // CompoundMolecule defines molecules with sub-molecular structures that may themselves be simple molecules.
    // This and simple molecules were differentiated to track very specific simple molecules important to glaze calculation.
    class CompoundMolecule
    {
        // Constructor
        // Requires a JSON property with the compound molecule's information, a Periodic Table, and a dictionary defining all known simple molecules.
        public CompoundMolecule(JProperty compound_molecule_json_property, PeriodicTable periodic_table, Dictionary<string, SimpleMolecule> simple_molecule_dictionary)
        {
            // Defines the local copies of the Period Table and the dictionary of simple molecules.
            PeriodicTable = periodic_table;
            SimpleMolecules = simple_molecule_dictionary;

            // Instantiates the SubMolecules and AdditionalElements properties to avoid errors.
            SubMolecules = new Dictionary<string, Tuple<SimpleMolecule, int>>();
            AdditionalElements = new List<Tuple<Element, int>>();

            // Pulls name from the JSON property itself.
            Name = compound_molecule_json_property.Name;

            // Casts the JSON property's value as a JSON object then builds the molecule from that object.
            JObject compound_molecule_object = compound_molecule_json_property.Value.ToObject<JObject>();
            BuildCompoundMolecule(compound_molecule_object);
        }

        // Properties
        // Properties inherent to a compound molecule.
        public string Name { get; set; }
        string FullFormula { get; set; }
        Dictionary<string, Tuple<SimpleMolecule, int>> SubMolecules { get; set; }
        List<Tuple<Element, int>> AdditionalElements { get; set; }
        double MolecularWeight { get; set; }

        // Properties
        // Local copies of the Periodic Table and a dictionary of all relevant simple molecules.
        private PeriodicTable PeriodicTable { get; set; }
        private Dictionary<string, SimpleMolecule> SimpleMolecules { get; set; }

        // Function
        // Takes the JSON object with all the molecule's information and extracts the data into the molecule's properties.
        void BuildCompoundMolecule(JObject compound_molecule_object)
        {
            // Iterates through the JSON object's children and extracts each value into a molecular Property.
            foreach (JProperty property in compound_molecule_object.Children())
            {
                if (property.Name == "Formula")
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
                else if (PeriodicTable.Contains(property.Name))
                {
                    // Adds any floating elements after checking for them in the Periodic Table.
                    AdditionalElements.Add(new Tuple<Element, int>(PeriodicTable.FindElement(property.Name), Convert.ToInt32(property.Value)));
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

    // Class
    // Generic container for molecules.
    // Allows a query to not need to specify if it's looking for a compound or a simple molecule in a search.
    class GenericMolecule
    {
        // Constructor
        // Turns the GenericMolecule into a compound molecule and declares the simple molecule Property to be null.
        public GenericMolecule(CompoundMolecule compound_molecule)
        {
            CompoundMolecule = compound_molecule;
            SimpleMolecule = null;
        }

        // Constructor
        // Turns the GenericMolecule into a simple molecule and declares the compound molecule Property to be null.
        public GenericMolecule(SimpleMolecule simple_molecule)
        {
            SimpleMolecule = simple_molecule;
            CompoundMolecule = null;
        }

        // Properties
        // Items determining if this GenericMolecule is a simple or compound molecule.
        public CompoundMolecule CompoundMolecule { get; set; }
        public SimpleMolecule SimpleMolecule { get; set; }
    }

    // Class
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
                SimpleMolecules.Add(simple_molecule.Name, new SimpleMolecule(simple_molecule, periodic_table));
            }

            // Iterates through all molecules in the compound molecules JSON object and adds each one to the compound molecules dictionary.
            foreach (JProperty compound_molecule in compound_molecules_json.Children())
            {
                CompoundMolecules.Add(compound_molecule.Name, new CompoundMolecule(compound_molecule, periodic_table, SimpleMolecules));
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
        // If either does, it returns a genericized version of that molecule.
        // Exact case and spelling is required.
        public GenericMolecule Get(string molecule_name)
        {
            if (CompoundMolecules.ContainsKey(molecule_name))
            {
                return new GenericMolecule(CompoundMolecules[molecule_name]);
            }
            else if (SimpleMolecules.ContainsKey(molecule_name))
            {
                return new GenericMolecule(SimpleMolecules[molecule_name]);
            }
            else
            {
                return null;
            }
        }
    }

    // Class
    // A Periodic Table of the Elements.
    class PeriodicTable
    {
        // Constructor
        // Requires a JSON object with all elements.
        public PeriodicTable(JObject element_json_list)
        {
            // Instantiates its element dictionary to avoid errors.
            Elements = new Dictionary<string, Element>();

            // Iterates through all elements in the JSON object and adds each one to the dictionary.
            foreach (JProperty element in element_json_list.Children())
            {
                this.Add(element);
            }
        }

        // Property
        // Dictionary of all elements in this Periodic Table.
        Dictionary<string, Element> Elements { get; set; }

        // Function
        // Adds a given element to the dictionary.
        public void Add(JProperty element_json)
        {

            Elements.Add(element_json.Name, new Element(element_json));
        }

        // Function
        // Checks if the dictionary contains a given element.
        // Exact case and spelling are required.
        public bool Contains(string element_name)
        {
            if (Elements.ContainsKey(element_name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Function
        // Checks if the dictionary contains an element.
        // If it does, it returns the element.
        public Element FindElement(string element_name)
        {
            if (Elements.ContainsKey(element_name))
            {
                return Elements[element_name];
            }
            else
            {
                return null;
            }
        }
    }
}
