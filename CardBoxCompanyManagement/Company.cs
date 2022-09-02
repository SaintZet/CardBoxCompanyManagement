using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CardBoxCompanyManagement
{
    public class Company
    {
        public Company(int ID, string Name, string category, string summary)
        {
            this.ID = ID;
            this.Name = Name;
            Category = category;
            Summary = summary;
        }

        [JsonProperty("bulstat")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int ID { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("category_id")]
        public string Category { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("image")]
        public Uri? Image { get; set; } = null;
    }
}