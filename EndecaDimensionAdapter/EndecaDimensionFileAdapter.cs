using Castle.Core.Logging;
using System.IO;
using System.Xml.Serialization;

namespace EndecaDimensionAdapter
{
    public class EndecaDimensionFileAdapter : IEndecaDimensionFileAdapter
    {
        public ILogger Log { get; set; }

        public IEndecaDimensionXml GetEndecaDimensionsFromFile(string file)
        {
            const string logLocation = "EndecaDimensionFileAdapter:GetEndecaDimensionsFromFile";
            if (!IsFileValid(file, logLocation)) return null;

            using (var fileStream = new FileStream(file, FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof (EndecaDimensionXml));
                var deserializedXml = (EndecaDimensionXml) xmlSerializer.Deserialize(fileStream);
                Log.InfoFormat("{0} - Deserialized file {1}!", logLocation, file);
                return deserializedXml;
            }
        }

        public void WriteOutEndecaDimensionFile(string file, IEndecaDimensionXml dimensionFileXml)
        {
            const string logLocation = "EndecaDimensionFileAdapter:WriteOutEndecaDimensionFile";
            if (!IsFileValid(file, logLocation)) return;
            if (dimensionFileXml == null)
            {
                Log.ErrorFormat("{0} - Dimension file XML is null!", logLocation);
                return;
            }

            using (var sw = new StreamWriter(file))
            {
                var xs = new XmlSerializer(typeof (EndecaDimensionXml));
                xs.Serialize(sw, dimensionFileXml);
                Log.InfoFormat("{0} - File {1} serialized to file!");
            }
        }

        private bool IsFileValid(string file, string logLocation)
        {
            if (string.IsNullOrEmpty(file))
            {
                Log.ErrorFormat("{0} - File name not supplied to EndecaDimensionFileAdapter!", logLocation);
                return false;
            }
            else if (!File.Exists(file))
            {
                Log.ErrorFormat("{0} - File {1} does not exist in the filesystem! Check the file location and try again!", logLocation, file);
                return false;
            }

            return true;
        }
    }
}
