using Net.Chdk.Meta.Model.CameraList;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    sealed class CsvCameraListProvider : CsvCameraProvider<ListPlatformData, ListRevisionData, ListSourceData>, ICameraListProvider
    {
        public IDictionary<string, ListPlatformData> GetCameraList(Stream stream)
        {
            return GetCameras(stream);
        }
    }
}
