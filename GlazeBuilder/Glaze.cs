using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlazeBuilder
{
    public class Glaze
    {
        public Glaze()
        {

        }

        // Double is the percentage of 1 of the material in the glaze
        List<Tuple<double, Material>> Materials { get; set; }
        PyrometricCone Cone { get; set; }
        Color FiredColor { get; set; }
        bool Reduction { get; set; }
    }
}
