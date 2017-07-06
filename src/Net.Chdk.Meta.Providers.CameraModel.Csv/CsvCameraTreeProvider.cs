using Net.Chdk.Meta.Model.CameraTree;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    sealed class CsvCameraTreeProvider : CsvCameraProvider<TreePlatformData, TreeRevisionData, TreeSourceData>, ICameraTreeProvider
    {
        public IDictionary<string, TreePlatformData> GetCameraTree(Stream stream)
        {
            return GetCameras(stream);
        }
    }
}
