using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    sealed class CsvCameraListProvider : ICameraListProvider
    {
        public IDictionary<string, IDictionary<string, string>> GetCameraList(Stream stream)
        {
            var cameraList = new SortedDictionary<string, IDictionary<string, string>>();
            using (var reader = new StreamReader(stream))
            {
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(',');
                    AddCamera(cameraList, split[0], split[1]);
                }
            }
            return cameraList;
        }

        private static void AddCamera(IDictionary<string, IDictionary<string, string>> cameraList, string platform, string revision)
        {
            var revisionKey = GetRevisionKey(revision);
            var revisions = GetOrAddRevisions(cameraList, platform);
            revisions.Add(revisionKey, revision);
        }

        private static IDictionary<string, string> GetOrAddRevisions(IDictionary<string, IDictionary<string, string>> cameraList, string platform)
        {
            IDictionary<string, string> revisions;
            if (!cameraList.TryGetValue(platform, out revisions))
            {
                revisions = new SortedDictionary<string, string>();
                cameraList.Add(platform, revisions);
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
