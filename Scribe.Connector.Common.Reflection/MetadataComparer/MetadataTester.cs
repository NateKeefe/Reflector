namespace MetadataComparer
{
    using System.IO;
    using System.Linq;

    using Scribe.Core.ConnectorApi;

    public static class MetadataTester
    {

        public static Metadata GetMetadata(IConnector conn)
        {
            using (var mp = conn.GetMetadataProvider())
            {
                var actions = mp.RetrieveActionDefinitions().ToList();
                var objects = mp.RetrieveObjectDefinitions(true, true).ToList();
                return new Metadata(actions, objects);
            }
        }

        public static Metadata GetMetadata(string s)
        {
            return new Metadata(s);
        }

        public static string SaveMetadata(Metadata m, string fileName)
        {
            var s = Metadata.Serialize(m);
            File.WriteAllText(fileName, s);
            return s;
        }

        public static Metadata OpenMetadata(string fileName)
        {
            var s = File.ReadAllText(fileName);
            return Metadata.Deserialize(s);
        }
    }
}