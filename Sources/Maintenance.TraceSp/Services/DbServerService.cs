namespace Maintenance.TraceSp.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using Models;

    public interface IDbServerService
    {
        List<Server> GetDbServerList(string dbServerXmlPath);
    }

    public class DbServerService : IDbServerService
    {
        public List<Server> GetDbServerList(string dbServerXmlPath)
        {
            var Servers = new List<Server>();

            if (File.Exists(dbServerXmlPath))
            {
                var deserializer = new XmlSerializer(typeof(List<Server>), new XmlRootAttribute("DBServers"));

                using var stream = File.OpenRead(dbServerXmlPath);
                Servers = deserializer.Deserialize(stream) as List<Server>;
            }

            return Servers;
        }
    }
}
