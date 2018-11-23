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
        
        public GlazeChemistry GlazeChemistry { get; set; }
        private MainWindow MainWindow { get; set; }
        public Dictionary<string, Material> Materials { get; set; }

        private void PopulateAllMaterials(string filename)
        {
            string[] allMaterialsText = System.IO.File.ReadAllLines(filename);

            foreach (string material in allMaterialsText)
            {
                Materials.Add(material, new Material(GlazeChemistry.LookupMolecule(material)));
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
        public Material(GenericMolecule molecule)
        {
            if (molecule.CompoundMolecule != null)
            {
                CompoundMolecule = molecule.CompoundMolecule;
                SimpleMolecule = null;
            }
            else if (molecule.SimpleMolecule != null)
            {
                SimpleMolecule = molecule.SimpleMolecule;
                CompoundMolecule = null;
            }
            else
            {
                SimpleMolecule = null;
                CompoundMolecule = null;
            }
        }

        CompoundMolecule CompoundMolecule { get; set; }
        SimpleMolecule SimpleMolecule { get; set; }
    }
}
