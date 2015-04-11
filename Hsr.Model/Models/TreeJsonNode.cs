#region

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace Hsr.Model.Models
{
    public class TreeJsonNode
    {
        public string id { get; set; }

        public string text { get; set; }

        public string pid { get; set; }

        public string icon { get; set; }

        public StateType state { get; set; }

        [JsonIgnore]
        public string Tag { get; set; }
        public List<TreeJsonNode> children { get; set; }
    }

    public class StateType
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
        [JsonProperty(PropertyName ="checked")]
        public bool Checked { get; set; }
    }
}