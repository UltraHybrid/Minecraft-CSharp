using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tmp.Mods.Parsers
{
    class ConfigParser
    {
        public static Dictionary<string, string> Parse(FileInfo configFile)
        {
            var result = new Dictionary<string, string>();
            foreach (var line in File.ReadLines(configFile.FullName, Encoding.UTF8))
            {
                var keyValuePair = line.Split(new[] { '=' }, 2);
                result[keyValuePair[0].Trim()] = keyValuePair[1].Trim();
            }
            return result;
        }
    }
}
