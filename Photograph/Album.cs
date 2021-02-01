using PhotosCategorier.Utils;
using System;
using System.IO;
using System.Linq;

namespace PhotosCategorier.Photo
{
    public class Album : IEquatable<Album>
    {
        public DirectoryInfo Directory
        {
            get; private set;
        }

        public Album(DirectoryInfo directory)
        {
            Directory = directory;
        }

        /// <summary>
        /// Geting all photos in the album
        /// </summary>
        /// <returns>All photos the album has.If it has no photos,it'll return null.</returns>
        public Photograph[] GetAllPhotographs()
        {
            var allFiles = Directory.GetFiles();

            var imageFiles = (from file in allFiles
                              where file.IsPhotograph()
                              select file)
                              .ToArray();

            if (imageFiles.Length > 0)
            {
                var photographs = (from file in imageFiles
                                   select new Photograph(file.FullName))
                                   .ToArray();
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
            return Directory.GetHashCode();
        }

        bool IEquatable<Album>.Equals(Album other)
        {
            return Directory.FullName == other.Directory.FullName;
        }
    }
}
