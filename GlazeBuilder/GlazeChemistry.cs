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
             /*
            string simple_molecules_file = System.IO.File.ReadAllText(simple_molecules_filename);
            JObject simple_molecules_json = JObject.Parse(simple_molecules_file);

            string compound_molecules_file = System.IO.File.ReadAllText(compound_molecules_filename);
            JObject compound_molecules_json = JObject.Parse(compound_molecules_file);

            MolecularDictionary = new MolecularDictionary(simple_molecules_json, compound_molecules_json);
            */
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
        SimpleMolecule(JObject simple_molecule_json_object)
        {
            Name = simple_molecule_json_object.GetValue("Name").ToString();
        }

        string Name { get; set; }
        List<Tuple<Element, int>> Formula { get; set; }
        double MolecularWeight { get; set; }
    }

    class CompoundMolecule
    {
        CompoundMolecule(JObject compound_molecule_json_object)
        {
            
        }

        Dictionary<string, Tuple<SimpleMolecule, int>> SubMolecules { get; set; }
        Dictionary<string, Tuple<Element, int>> AdditionalElements { get; set; }
        string FullFormula { get; set; }
        int MolecularWeight { get; set; }
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
        public MolecularDictionary(JObject simple_molecules_json, JObject compound_molecules_json)
        {

        }

        Dictionary<string, CompoundMolecule> CompoundMolecules { get; set; }
        Dictionary<string, SimpleMolecule> SimpleMolecules { get; set; }

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
