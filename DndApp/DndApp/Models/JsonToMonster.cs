using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DndApp.Models
{
    public class JsonToMonster
    {
        // counterpart to array results from JSON (the only part we're interested in)
        public List<Result> results { get; set; }

        // objects from array results
        public class Result
        {
            [JsonProperty(PropertyName = "index")]
            public string MonsterId { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }


            public override string ToString()
            {
                return $"{this.Name}";
            }
        }
    }
}
