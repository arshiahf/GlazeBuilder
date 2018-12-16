using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GlazeChemistry;

namespace GlazeBuilder
{
    class MaterialDatabase
    {
        public MaterialDatabase(string matFile, MainWindow window)
        {
            Materials = new Dictionary<string, Material>();
            this.MainWindow = window;
            PopulateAllMaterials(matFile);
        }
        
        public ChemicalDatabase ChemicalDatabase { get; set; }
        private MainWindow MainWindow { get; set; }
        public Dictionary<string, Material> Materials { get; set; }

        private void PopulateAllMaterials(string filename)
        {
            string all_materials_text = System.IO.File.ReadAllText(filename);
            JObject all_materials_json_object = JObject.Parse(all_materials_text);

            foreach (JProperty material in all_materials_json_object.Children())
            {
                Materials.Add(material.Name, new Material(material));
            }

            MainWindow.MaterialsList.ItemsSource = Materials;
            MainWindow.MaterialsList.DisplayMemberPath = "Name";
        }

        /*
        private void completeKey(ref string partial)
        {
            List<string> options = new List<string>();

            foreach (Material mat in Materials)
            {
                if (mat.MatName.ToLower().StartsWith(partial.ToLower()))
                {
                    options.Add();
                }
            }

            if (options.Count == 1)
            {
                partial = options[0];
            }
            else if (options.Count > 1)
            {
                MainWin.OverflowMaterials.ItemsSource = options;
                MainWin.InputMaterial.Text = null;
                MainWin.OverflowMaterials.Visibility = Visibility.Visible;
                MainWin.InputMaterial.Visibility = Visibility.Hidden;
            }
            else
            {

            }
        }

        public void findMaterial(string material)
        {
            completeKey(ref material);
            foreach (Material mat in Materials)
            {
                if (material == mat.)
                {
                    MainWin.InputMaterial.Text = null;
                }
                else
                {
                    ErrorPopup pop = new ErrorPopup("ERROR: " + material + " not found in materials database.\n" +
                        "Please select valid material from list or enter new material.\n");
                }
            }
        }
        */
    }

    class Material
    {
        public Material(JProperty material_json_property)
        {
            Name = material_json_property.Name;
        }

        string Name { get; set; }
        List<CompoundMolecule> CompoundMolecules { get; set; }
        List<SimpleMolecule> SimpleMolecules { get; set; }
    }
}
