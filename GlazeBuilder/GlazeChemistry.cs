using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlazeBuilder
{
    class GlazeChemistry
    {
        public GlazeChemistry(string elements_filename, string simple_molecules_filename, string compound_molecules_filename)
        {
            string elements_file = System.IO.File.ReadAllText(elements_filename);
            JObject elements_full_json = JObject.Parse(elements_file);

            PeriodicTable = new PeriodicTable(elements_full_json);
             
            string simple_molecules_file = System.IO.File.ReadAllText(simple_molecules_filename);
            JObject simple_molecules_json = JObject.Parse(simple_molecules_file);

            string compound_molecules_file = System.IO.File.ReadAllText(compound_molecules_filename);
            JObject compound_molecules_json = JObject.Parse(compound_molecules_file);

            MolecularDictionary = new MolecularDictionary(simple_molecules_json, compound_molecules_json, PeriodicTable);
        }

        PeriodicTable PeriodicTable { get; set; }
        MolecularDictionary MolecularDictionary { get; set; }

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

    class Element
    {
        public Element(JProperty element_json_property)
        {
            Name = element_json_property.Name;
            JObject element_json_object = element_json_property.Value.ToObject<JObject>();
            AtomicSymbol = element_json_object.GetValue("AtomicSymbol").ToString();
            AtomicNumber = Convert.ToInt32(element_json_object.GetValue("AtomicNumber"));
            AtomicWeight = Convert.ToDouble(element_json_object.GetValue("AtomicWeight"));
        }

        public string Name { get; set; }
        public string AtomicSymbol { get; set; }
        public int AtomicNumber { get; set; }
        public double AtomicWeight { get; set; }
    }

    class SimpleMolecule
    {
        public SimpleMolecule(JProperty simple_molecule_json_property, PeriodicTable periodic_table)
        {
            PeriodicTable = periodic_table;
            Formula = new List<Tuple<Element, int>>();
            MolecularWeight = 0.0;

            Name = simple_molecule_json_property.Name;
            JObject simple_molecule_object = simple_molecule_json_property.Value.ToObject<JObject>();
            BuildSimpleMolecule(simple_molecule_object);
        }

        public string Name { get; set; }
        public string FullFormula { get; set; }
        List<Tuple<Element, int>> Formula { get; set; }
        public double MolecularWeight { get; set; }

        private PeriodicTable PeriodicTable { get; set; }

        void BuildSimpleMolecule(JObject simple_molecule_object)
        {
            foreach (JProperty property in simple_molecule_object.Children())
            {
                if (property.Name == "Formula")
                {
                    FullFormula = property.Value.ToString();
                }
                else if (PeriodicTable.Contains(property.Name))
                {
                    Formula.Add(new Tuple<Element, int>(PeriodicTable.FindElement(property.Name), Convert.ToInt32(property.Value)));
                }
            }

            foreach (Tuple<Element, int> element in Formula)
            {
                MolecularWeight += element.Item1.AtomicWeight * element.Item2;
            }
        }
    }

    class CompoundMolecule
    {
        public CompoundMolecule(JProperty compound_molecule_json_property, PeriodicTable periodic_table, Dictionary<string, SimpleMolecule> simple_molecule_dictionary)
        {
            PeriodicTable = periodic_table;
            SimpleMolecules = simple_molecule_dictionary;

            SubMolecules = new Dictionary<string, Tuple<SimpleMolecule, int>>();
            AdditionalElements = new List<Tuple<Element, int>>();

            Name = compound_molecule_json_property.Name;
            JObject compound_molecule_object = compound_molecule_json_property.Value.ToObject<JObject>();
            BuildCompoundMolecule(compound_molecule_object);
        }

        public string Name { get; set; }
        string FullFormula { get; set; }
        Dictionary<string, Tuple<SimpleMolecule, int>> SubMolecules { get; set; }
        List<Tuple<Element, int>> AdditionalElements { get; set; }
        double MolecularWeight { get; set; }

        private PeriodicTable PeriodicTable { get; set; }
        Dictionary<string, SimpleMolecule> SimpleMolecules { get; set; }

        void BuildCompoundMolecule(JObject compound_molecule_object)
        {
            foreach (JProperty property in compound_molecule_object.Children())
            {
                if (property.Name == "Formula")
                {
                    FullFormula = property.Value.ToString();
                }
                else if (property.Name == "Submolecules")
                {
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
                    AdditionalElements.Add(new Tuple<Element, int>(PeriodicTable.FindElement(property.Name), Convert.ToInt32(property.Value)));
                }
            }

            foreach (Tuple<Element, int> element in AdditionalElements)
            {
                MolecularWeight += element.Item1.AtomicWeight * element.Item2;
            }

            foreach (Tuple<SimpleMolecule, int> molecule in SubMolecules.Values)
            {
                MolecularWeight += molecule.Item1.MolecularWeight * molecule.Item2;
            }
        }
    }

    class GenericMolecule
    {
        public GenericMolecule(CompoundMolecule compound_molecule)
        {
            CompoundMolecule = compound_molecule;
            SimpleMolecule = null;
        }

        public GenericMolecule(SimpleMolecule simple_molecule)
        {
            SimpleMolecule = simple_molecule;
            CompoundMolecule = null;
        }

        public CompoundMolecule CompoundMolecule { get; set; }
        public SimpleMolecule SimpleMolecule { get; set; }
    }

    class MolecularDictionary
    {
        public MolecularDictionary(JObject simple_molecules_json, JObject compound_molecules_json, PeriodicTable periodic_table)
        {
            SimpleMolecules = new Dictionary<string, SimpleMolecule>();
            CompoundMolecules = new Dictionary<string, CompoundMolecule>();

            foreach (JProperty simple_molecule in simple_molecules_json.Children())
            {
                SimpleMolecules.Add(simple_molecule.Name, new SimpleMolecule(simple_molecule, periodic_table));
            }

            foreach (JProperty compound_molecule in compound_molecules_json.Children())
            {
                CompoundMolecules.Add(compound_molecule.Name, new CompoundMolecule(compound_molecule, periodic_table, SimpleMolecules));
            }
        }

        Dictionary<string, SimpleMolecule> SimpleMolecules { get; set; }
        Dictionary<string, CompoundMolecule> CompoundMolecules { get; set; }

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

    class PeriodicTable
    {
        public PeriodicTable(JObject element_json_list)
        {
            Elements = new Dictionary<string, Element>();
            foreach (JProperty element in element_json_list.Children())
            {
                this.Add(element);
            }
        }

        Dictionary<string, Element> Elements { get; set; }

        public void Add(JProperty element_json)
        {

            Elements.Add(element_json.Name, new Element(element_json));
        }

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

        public Element FindElement(string element_name)
        {
            if (Elements.ContainsKey(element_name))
            {
                return Elements[element_name];
            }

            return null;
        }
    }
}
