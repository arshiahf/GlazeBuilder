using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Chemistry;

namespace GlazeBuilder
{
    class MaterialDatabase
    {
        public MaterialDatabase(string matFile, MainWindow window)
        {
            Elements = new Dictionary<string, Element>();
            Materials = new List<Material>();
            this.MainWin = window;
            populateAllElements("PeriodicTableElements.csv");
            this.populatePeriodicTable(this.Elements);
            populateAllMaterials(matFile);
        }

        private MainWindow MainWin { get; set; }
        private Dictionary<string, Element> Elements { get; set; }
        public List<Material> Materials { get; set; }

        private void populateAllElements(string filename)
        {
            string[] allElementsText = System.IO.File.ReadAllLines(filename);

            foreach (string element in allElementsText)
            {
                var parts = element.Split(',');
                Elements.Add(parts[0], new Element(parts[1], Convert.ToInt32(parts[2]), Convert.ToDouble(parts[3])));
            }
        }

        private void populatePeriodicTable(Dictionary<String, Element> dict)
        {
            foreach (KeyValuePair<string, Element> kvp in dict)
            {
                PeriodicTable.Add(kvp.Value);
            }
        }

        private void populateAllMaterials(string filename)
        {
            string[] allMaterialsText = System.IO.File.ReadAllLines(filename);

            foreach (string formula in allMaterialsText)
            {
                var parts = formula.Split(',');
                Console.WriteLine("Material: {0}, Formula: {1}", parts[0], parts[1]);
                Materials.Add(new Material(parts[0], new ChemicalFormula(parts[1])));
            }

            MainWin.MaterialsList.ItemsSource = Materials;
            MainWin.MaterialsList.DisplayMemberPath = "MatName";
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
        public Material(string matName, ChemicalFormula matFormula)
        {
            MatName = matName;
            MatFormula = matFormula;
        }

        public Material() { }

        public string MatName { get; set; }
        ChemicalFormula MatFormula { get; set; }
    }
}
