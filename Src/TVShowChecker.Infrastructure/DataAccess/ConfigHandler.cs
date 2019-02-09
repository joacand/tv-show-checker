using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TVShowChecker.Core.Interfaces;

namespace TVShowChecker.Infrastructure.DataAccess
{
    public class ConfigHandler : IConfigHandler
    {
        private static readonly string subTvConfigFile = @"SubscribedTV.xml";

        public List<string> ReadSubscribedTvShowsFromConfig()
        {
            if (File.Exists(subTvConfigFile))
            {
                using (var sr = new StreamReader(subTvConfigFile))
                {
                    var serializer = new XmlSerializer(typeof(List<string>));
                    return serializer.Deserialize(sr) as List<string>;
                }
            }
            return new List<string>();
        }

        public void SaveTvShowsToConfig(List<string> tvShows)
        {
            using (StreamWriter sw = new StreamWriter(subTvConfigFile))
            {
                var serializer = new XmlSerializer(tvShows.GetType());
                serializer.Serialize(sw, tvShows);
                sw.Flush();
            }
        }
    }
}
