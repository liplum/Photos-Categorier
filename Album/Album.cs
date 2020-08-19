using System.IO;
using System.Linq;

namespace PhotosCategorier.Album
{
    public class Album
    {
        public DirectoryInfo Directory { get; private set; }

        public Album(DirectoryInfo directory)
        {
            Directory = directory;
        }

        public Photograph[] GetAllPhotographs()
        {
            var allFiles = Directory.GetFiles();

            var selectedFiles = from file in allFiles
                                where
           file.Name.EndsWith(".png") || file.Name.EndsWith(".jpg") || file.Name.EndsWith(".gif")
           || file.Name.EndsWith(".jpeg")
                                select file;

            var imageFiles = selectedFiles.ToArray();

            int filesCount = imageFiles.Length;


            if (filesCount > 0)
            {
                var photographs = new Photograph[filesCount];

                for (int i = 0; i < filesCount; i++)
                {
                    photographs[i] = new Photograph(imageFiles[i].FullName);
                }
                return photographs;
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj is Album album)
            {
                return Directory.FullName == album.Directory.FullName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
