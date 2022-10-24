using System;
using System.Collections.Generic;
using System.Text;

namespace NFTValuations.Domain.Models
{
    public sealed class ResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExternalUrl { get; set; }
        public string Media { get; set; }
        public List<PropertyValuePair> Properties { get; set; }

    }

    public sealed class PropertyValuePair
    {
        public string Category { get; set; }
        public string Property { get; set; }
    }
}
