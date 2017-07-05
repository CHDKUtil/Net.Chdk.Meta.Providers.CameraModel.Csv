using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.CameraModel.Csv
{
    sealed class CsvCameraListProvider : CsvCameraProvider<string>, ICameraListProvider
    {
        public IDictionary<string, IDictionary<string, string>> GetCameraList(Stream stream)
        {
            return GetCameras(stream);
        }

        protected override string GetData(string platform, string revision, string source)
        {
            return revision;
        }
    }
}
