using PhotosCategorier.Utils;
using System.IO;
using System.Linq;

namespace PhotosCategorier.Photo
{
    public class Album
    {
        public DirectoryInfo Directory { get; private set; }

        public Album(DirectoryInfo directory)
        {
            Directory = directory;
        }

        /// <summary>
        /// Geting all photos in the album
        /// </summary>
        /// <returns>All photos the album has.If it has no photos,it would return null.</returns>
        public Photograph[] GetAllPhotographs()
        {
            var allFiles = Directory.GetFiles();

            var selectedFiles = from file in allFiles
                                where
           file.IsPhotograph() 
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
