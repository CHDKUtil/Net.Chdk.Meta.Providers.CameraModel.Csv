using Net.Chdk.Meta.Model.CameraModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    sealed class CsvCameraTreeProvider : CsvCameraProvider<TreeRevisionData>, ICameraTreeProvider
    {
        public IDictionary<string, TreePlatformData> GetCameraTree(Stream stream)
        {
            return GetCameras(stream)
                .ToDictionary(kvp => kvp.Key, kvp => GetPlatform(kvp.Value));
        }

        protected override TreeRevisionData GetData(string platform, string revision, string source)
        {
            return new TreeRevisionData
            {
                Source = GetSource(platform, revision, source)
            };
        }

        private static TreePlatformData GetPlatform(IDictionary<string, TreeRevisionData> revisions)
        {
            return new TreePlatformData
            {
                Revisions = revisions
            };
        }

        private static TreeSourceData GetSource(string platform, string revision, string source)
        {
            return new TreeSourceData
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
    }
}
