using Probea.Helpers;
using Probea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SondaWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public Probe GetProbeFromDay()
        {
            Probe Probe = XMLSerializerHelper.DeserializeProbeFromXML()
                .First(x => x.PublicationDate == DateTime.Now.ToShortDateString());
            return Probe;
        }
        public void PostProbe(Probe probe)
        {
            XMLSerializerHelper.AddOrUpdate(probe);
        }
    }
}
