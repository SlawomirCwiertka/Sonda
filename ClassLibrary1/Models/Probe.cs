using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Probea.Models
{
    [DataContract]
    public class Probe
    {
        [DataMember]
        public string PublicationDate { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public List<Answer> Answers { get; set; }
    }
}