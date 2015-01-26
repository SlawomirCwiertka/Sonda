using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Probea.Models
{
    [DataContract]
    public class Answer
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
