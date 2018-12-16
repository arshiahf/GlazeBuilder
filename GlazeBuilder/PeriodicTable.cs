using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlazeChemistry
{
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
