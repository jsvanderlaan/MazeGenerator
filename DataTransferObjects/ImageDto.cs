using System.IO;

namespace DataTransferObjects
{
    public class ImageDto
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public Stream Data { get; set; }
    }
}
