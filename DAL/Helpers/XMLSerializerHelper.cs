using Probea.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Probea.Helpers
{
    public static class XMLSerializerHelper
    {
        public static string PathData = @"C:\Users\Slawomir\Documents\Visual Studio 2013\Projects\Sonda\DAL\Data\Sondy.xml";
        static public void SerializeProbeToXML(List<Probe> probes)
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<Probe>));
            TextWriter textWriter = new StreamWriter(PathData);
            serializer.Serialize(textWriter, probes);
            textWriter.Close();
        }
        static public List<Probe> DeserializeProbeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Probe>));
            TextReader textReader = new StreamReader(PathData);
            List<Probe> Probes;
            Probes = (List<Probe>)deserializer.Deserialize(textReader);
            textReader.Close();
            return Probes;
        }
        static public void AddOrUpdate(Probe probe)
        {
            List<Probe> Probes = XMLSerializerHelper.DeserializeProbeFromXML();
            if (Probes.Any(x=> x.PublicationDate == probe.PublicationDate))
            {
                Probes.Remove(Probes.Single(x => x.PublicationDate == probe.PublicationDate));
                Probes.Add(probe);
            }
            else
            {
                Probes.Add(probe);
            }
            System.IO.File.Delete(PathData);
            SerializeProbeToXML(Probes);
        }
    }
}