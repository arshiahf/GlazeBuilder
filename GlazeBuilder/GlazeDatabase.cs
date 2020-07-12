﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GlazeChemistry;

namespace GlazeBuilder
{
    public class GlazeDatabase
    {
        public GlazeDatabase()
        {
            MaterialDatabase = new MaterialDatabase("MaterialsRawChemicals.json", "MaterialsCompoundChemicals.json");

            Cones = new Dictionary<string, PyrometricCone>();
            PopulateCones("PyrometricCones.json");

            UMFLimits = new Dictionary<string, UnityMolecularFormulaLimit>();
            InputLimits("UMFLimits.json");

            Glazes = new Dictionary<string, Glaze>();
        }

        public Dictionary<string, PyrometricCone> Cones { get; set; }
        public MaterialDatabase MaterialDatabase { get; set; }
        public Dictionary<string, Glaze> Glazes { get; set; }
        private Dictionary<string, UnityMolecularFormulaLimit> UMFLimits { get; set; }

        void PopulateCones(string pyrometric_cones_filename)
        {
            string all_cones_raw_json = System.IO.File.ReadAllText(pyrometric_cones_filename);
            JObject cones_json = JObject.Parse(all_cones_raw_json);

            foreach (JProperty cone_json_property in cones_json.Children())
            {
                Cones.Add(cone_json_property.Name, new PyrometricCone(cone_json_property));
            }
        }

        void InputLimits(string limits_filename)
        {
            string all_limits_text = System.IO.File.ReadAllText(limits_filename);
            JObject all_limits_json_object = JObject.Parse(all_limits_text);

            foreach (JProperty limit_json_subproperty in all_limits_json_object.Children())
            {
                UnityMolecularFormulaLimit temp_range = new UnityMolecularFormulaLimit();
                foreach (JProperty umf_property in limit_json_subproperty.Value.ToObject<JObject>().Children())
                {
                    if (umf_property.Name == "Cones")
                    {
                        foreach(JToken cone_token in umf_property.Value.ToArray<JToken>())
                        {
                            temp_range.Cones.Add(Cones[cone_token.ToString()]);
                        }
                    }
                    else if (umf_property.Name == "Surface")
                    {
                        foreach(JProperty surface_property in umf_property.Value.ToObject<JObject>().Children())
                        {
                            foreach (JProperty chemical_group_property in surface_property.Value.ToObject<JObject>().Children())
                            {
                                foreach (JProperty chemical_property in chemical_group_property.Value.ToObject<JObject>().Children())
                                {
                                    double temporary_minimum = 0;
                                    double temporary_maximum = 0;
                                    foreach (JProperty numerical_property in chemical_property.Value.ToObject<JObject>().Children())
                                    {
                                        if (numerical_property.Name == "Minimum")
                                        {
                                            temporary_minimum = Convert.ToDouble(numerical_property.Value);
                                        }
                                        else
                                        {
                                            temporary_maximum = Convert.ToDouble(numerical_property.Value);
                                        }
                                    }
                                    
                                    ((Dictionary<string, Tuple<double, double>>)((GlazeSurfaceLimits)temp_range[surface_property.Name])[chemical_group_property.Name]).Add(chemical_property.Name, new Tuple<double, double>(temporary_minimum, temporary_maximum));
                                }
                            }
                        }
                    }
                }
                UMFLimits.Add(limit_json_subproperty.Name, temp_range);
            }
        }

        void PopulateKnownGlazes(string known_glazes_filename)
        {
            string all_materials_text = System.IO.File.ReadAllText(known_glazes_filename);
            JObject all_materials_json_object = JObject.Parse(all_materials_text);
        }
    }

    public class UnityMolecularFormulaLimit
    {
        public UnityMolecularFormulaLimit()
        {
            Cones = new List<PyrometricCone>();
            Glossy = new GlazeSurfaceLimits();
            Satin = new GlazeSurfaceLimits();
            Matte = new GlazeSurfaceLimits();
        }

        public object this[string propertyName]
        {
            get
            {
                if (propertyName == "Glossy" || propertyName == "Satin" || propertyName == "Matte")
                {
                    return GetProperty(propertyName);
                }
                else
                {
                    return Cones;
                }
            }
        }

        public GlazeSurfaceLimits GetProperty(string property_name)
        {
            if (property_name == "Glossy")
            {
                return Glossy;
            }
            else if (property_name == "Satin")
            {
                return Satin;
            }
            else if (property_name == "Matte")
            {
                return Matte;
            }
            else
            {
                return null;
            }
        }

        public List<PyrometricCone> Cones;
        public GlazeSurfaceLimits Glossy;
        public GlazeSurfaceLimits Satin;
        public GlazeSurfaceLimits Matte;
    }

    public class GlazeSurfaceLimits
    {
        public GlazeSurfaceLimits()
        {
            Fluxes = new Dictionary<string, Tuple<double, double>>();
            FlowViscosity = new Dictionary<string, Tuple<double, double>>();
            GlassForming = new Dictionary<string, Tuple<double, double>>();
        }

        public object this[string propertyName]
        {
            get
            {
                return GetProperty(propertyName);
            }
        }

        public Dictionary<string, Tuple<double, double>> GetProperty(string property_name)
        {
            if (property_name == "Fluxes")
            {
                return Fluxes;
            }
            else if (property_name == "FlowViscosity")
            {
                return FlowViscosity;
            }
            else if (property_name == "GlassForming")
            {
                return GlassForming;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, Tuple<double, double>> Fluxes;
        public Dictionary<string, Tuple<double, double>> FlowViscosity;
        public Dictionary<string, Tuple<double, double>> GlassForming;
    }
}
