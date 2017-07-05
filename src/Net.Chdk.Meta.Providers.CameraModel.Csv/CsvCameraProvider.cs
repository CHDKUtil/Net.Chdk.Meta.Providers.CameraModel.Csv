using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    abstract class CsvCameraProvider<TRevision>
    {
        protected IDictionary<string, IDictionary<string, TRevision>> GetCameras(Stream stream)
        {
            var cameras = new SortedDictionary<string, IDictionary<string, TRevision>>();
            using (var reader = new StreamReader(stream))
            {
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(',');
                    AddCamera(cameras, split[0], split[1], split[3]);
                }
            }
            return cameras;
        }

        private void AddCamera(IDictionary<string, IDictionary<string, TRevision>> cameras, string platform, string revision, string source)
        {
            var revisionKey = GetRevisionKey(revision);
            var revisions = GetOrAddRevisions(cameras, platform);
            var data = GetData(platform, revision, source);
            revisions.Add(revisionKey, data);
        }

        protected abstract TRevision GetData(string platform, string revision, string source);

        private static IDictionary<string, TRevision> GetOrAddRevisions(IDictionary<string, IDictionary<string, TRevision>> cameras, string platform)
        {
            IDictionary<string, TRevision> revisions;
            if (!cameras.TryGetValue(platform, out revisions))
            {
                revisions = new SortedDictionary<string, TRevision>();
                cameras.Add(platform, revisions);
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
