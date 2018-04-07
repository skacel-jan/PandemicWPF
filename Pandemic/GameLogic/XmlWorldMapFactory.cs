using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Pandemic
{
    public class XmlWorldMapFactory : IWorldMapFactory
    {
        public XmlWorldMapFactory(IDictionary<DiseaseColor, Disease> diseases)
        { 
            Diseases = diseases ?? throw new ArgumentNullException(nameof(diseases));

            ParseXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\CitiesConfiguration.xml"));
        }

        private void ParseXml(string path)
        {           
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList cityNodes;
            XmlNamespaceManager namespaceManager = null;
            if (doc.DocumentElement.Attributes["xmlns"] != null)
            {
                string xmlns = doc.DocumentElement.Attributes["xmlns"].Value;
                namespaceManager = new XmlNamespaceManager(doc.NameTable);

                namespaceManager.AddNamespace("schema", xmlns);

                cityNodes = doc.SelectNodes("/schema:world/schema:cities/schema:city", namespaceManager);
            }
            else
            {
                cityNodes = doc.SelectNodes("/world/cities/city");
            }

            Cities = new List<City>(cityNodes.Count);
            foreach (XmlNode cityNode in cityNodes)
            {
                string name = cityNode.Attributes["name"].Value;
                DiseaseColor color = (DiseaseColor)Enum.Parse(typeof(DiseaseColor), cityNode.Attributes["color"].Value, true);
                Cities.Add(new City(name, color));
            }

            MapCities = new Dictionary<string, MapCity>(Cities.ToDictionary(x => x.Name, x => new MapCity(x, Diseases)));

            foreach (XmlNode cityNode in cityNodes)
            {
                var neighbors = cityNode.SelectNodes(".//schema:neighbor", namespaceManager);
                foreach (XmlNode neighbor in neighbors)
                {
                    MapCities[cityNode.Attributes["name"].Value].AddConnectedCity(MapCities[neighbor.InnerText]);
                }
            }

            WorldMap = new WorldMap(MapCities);
        }

        public IList<City> Cities { get; private set; }

        public WorldMap WorldMap { get; private set; }

        public IDictionary<string, MapCity> MapCities { get; private set; }

        public IDictionary<DiseaseColor, Disease> Diseases { get; }
    }
}