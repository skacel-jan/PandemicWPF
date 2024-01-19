using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Game.Pandemic.GameLogic.Board
{
    public class XmlWorldMapFactory : IWorldMapFactory
    {
        private XmlNodeList _cityNodes;
        private XmlNamespaceManager _namespaceManager;
        private List<City> _cities;

        public XmlWorldMapFactory()
        {
            ParseXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\CitiesConfiguration.xml"));
        }

        private void ParseXml(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            if (doc.DocumentElement.Attributes["xmlns"] != null)
            {
                string xmlns = doc.DocumentElement.Attributes["xmlns"].Value;
                _namespaceManager = new XmlNamespaceManager(doc.NameTable);

                _namespaceManager.AddNamespace("schema", xmlns);

                _cityNodes = doc.SelectNodes("/schema:world/schema:cities/schema:city", _namespaceManager);
            }
            else
            {
                _cityNodes = doc.SelectNodes("/world/cities/city");
            }

            var cities = new List<City>(_cityNodes.Count);
            foreach (XmlNode cityNode in _cityNodes)
            {
                string name = cityNode.Attributes["name"].Value;
                DiseaseColor color = (DiseaseColor)Enum.Parse(typeof(DiseaseColor), cityNode.Attributes["color"].Value, true);
                cities.Add(new City(name, color));
            }

            _cities = cities;
        }

        public WorldMap CreateWorldMap(IDictionary<DiseaseColor, Disease> diseases)
        {
            var mapCities = new Dictionary<string, MapCity>(_cities.ToDictionary(x => x.Name, x => new MapCity(x, diseases)));

            foreach (XmlNode cityNode in _cityNodes)
            {
                var neighbors = cityNode.SelectNodes(".//schema:neighbor", _namespaceManager);
                foreach (XmlNode neighbor in neighbors)
                {
                    mapCities[cityNode.Attributes["name"].Value].AddConnectedCity(mapCities[neighbor.InnerText]);
                }
            }

            return new WorldMap(mapCities.Values);
        }
    }
}