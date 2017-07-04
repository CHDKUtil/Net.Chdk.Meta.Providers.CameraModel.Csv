using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    sealed class CsvCameraListProvider : ICameraListProvider
    {
        public IDictionary<string, IDictionary<string, string>> GetCameraList(Stream stream)
        {
            var models = new SortedDictionary<string, IDictionary<string, string>>();
            using (var reader = new StreamReader(stream))
            {
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(',');
                    var revision = split[1];
                    var revisionKey = GetRevisionKey(revision);
                    var revisions = GetOrAddRevisions(split, models);
                    revisions.Add(revisionKey, revision);
                }
            }
            return models;
        }

        private static IDictionary<string, string> GetOrAddRevisions(string[] split, IDictionary<string, IDictionary<string, string>> models)
        {
            var model = split[0];
            IDictionary<string, string> revisions;
            if (!models.TryGetValue(model, out revisions))
            {
                revisions = new SortedDictionary<string, string>();
                models.Add(model, revisions);
            }
            return revisions;
        }

        private static string GetRevisionKey(string revisionStr)
        {
            var revision = GetFirmwareRevision(revisionStr);
            return $"0x{revision:x}";
        }

        private static uint GetFirmwareRevision(string revision)
        {
            if (revision == null)
                return 0;
            return
                (uint)((revision[0] - 0x30) << 24) +
                (uint)((revision[1] - 0x30) << 20) +
                (uint)((revision[2] - 0x30) << 16) +
                (uint)((revision[3] - 0x60) << 8);
        }
    }
}
