using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlazeBuilder
{
    class PyrometricCone
    {
        public PyrometricCone(JProperty cone_json_property)
        {
            MakeCone(cone_json_property);
        }

        string Name { get; set; }
        Dictionary<string, int> SmallCones { get; set; }
        Dictionary<string, int> LargeCones { get; set; }
        Dictionary<string, int> PCECones { get; set; }

        private void MakeCone(JProperty cone_json_property)
        {
            Name = cone_json_property.Name;

            foreach (JProperty cone_json_subproperty in cone_json_property.Value.ToObject<JObject>().Children())
            {
                if (cone_json_subproperty.Name == "Large Cones")
                {
                    LargeCones = new Dictionary<string, int>();
                    foreach (JProperty temperature_property in cone_json_subproperty.Value.ToObject<JObject>().Children())
                    {
                        LargeCones.Add(temperature_property.Name, Convert.ToInt32(temperature_property.Value));
                    }
                }
                else if (cone_json_subproperty.Name == "Small Cones")
                {
                    SmallCones = new Dictionary<string, int>();
                    foreach (JProperty temperature_property in cone_json_subproperty.Value.ToObject<JObject>().Children())
                    {
                        SmallCones.Add(temperature_property.Name, Convert.ToInt32(temperature_property.Value));
                    }
                }
                else if (cone_json_subproperty.Name == "P.C.E. Cones")
                {
                    PCECones = new Dictionary<string, int>();
                    foreach (JProperty temperature_property in cone_json_subproperty.Value.ToObject<JObject>().Children())
                    {
                        PCECones.Add(temperature_property.Name, Convert.ToInt32(temperature_property.Value));
                    }
                }
            }
        }
    }
}
