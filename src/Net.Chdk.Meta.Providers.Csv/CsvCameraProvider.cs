using Net.Chdk.Meta.Model;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.Csv
{
    public abstract class CsvCameraProvider<TPlatform, TRevision, TSource>
        where TPlatform : PlatformData<TPlatform, TRevision, TSource>, new()
        where TRevision : RevisionData<TRevision, TSource>, new()
        where TSource : SourceData<TSource>, new()
    {
        protected IDictionary<string, TPlatform> GetCameras(Stream stream)
        {
            var cameras = new SortedDictionary<string, TPlatform>();
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

        private static void AddCamera(IDictionary<string, TPlatform> cameras, string platform, string revision, string source)
        {
            var revisionKey = GetRevisionKey(revision);
            var platformData = GetOrAddPlatform(cameras, platform);
            var revisionData = GetRevisionData(platform, revision, source);
            platformData.Revisions.Add(revisionKey, revisionData);
        }

        private static TPlatform GetOrAddPlatform(IDictionary<string, TPlatform> cameras, string platform)
        {
            TPlatform platformData;
            if (!cameras.TryGetValue(platform, out platformData))
            {
                platformData = GetPlatformData();
                cameras.Add(platform, platformData);
            }
            return platformData;
        }

        private static TPlatform GetPlatformData()
        {
            return new TPlatform
            {
                Revisions = new SortedDictionary<string, TRevision>()
            };
        }

        private static TRevision GetRevisionData(string platform, string revision, string source)
        {
            return new TRevision
            {
                Source = GetSourceData(platform, revision, source)
            };
        }

        private static TSource GetSourceData(string platform, string revision, string source)
        {
            return new TSource
            {
                Platform = platform,
                Revision = GetRevision(revision, source)
            };
        }

        private static string GetRevision(string revision, string source)
        {
            if (!string.IsNullOrEmpty(source))
                return source;
            return revision;
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
