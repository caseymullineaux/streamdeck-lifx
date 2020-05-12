using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace au.com.mullineaux.lifx.Classes
{
    public class Light
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public LightGroup Group { get; set; }
    }
}
