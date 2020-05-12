using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace au.com.mullineaux.lifx.Classes
{
    public class Light : LightBase
    {
        public LightGroup Group { get; set; }

    }

    public class LightBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
    }

}

