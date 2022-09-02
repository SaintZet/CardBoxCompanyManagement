using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CardBoxCompanyManagement
{
    public class Company
    {
        public Company(int companyID, string companyName, string category, string summary)
        {
            CompanyID = companyID;
            CompanyName = companyName;
            Category = category;
            Summary = summary;
        }

        [JsonProperty("bulstat")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int CompanyID { get; set; }

        [JsonProperty("title")]
        public string CompanyName { get; set; }

        [JsonProperty("category_id")]
        public string Category { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }
    }
}