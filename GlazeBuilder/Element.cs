using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlazeChemistry
{
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
}
