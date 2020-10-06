using PhotosCategorier.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PhotosCategorier.Photo
{
    public class PhotographsGenerator : IEnumerator<Photograph>, IEqualityComparer<Photograph>
    {
        public List<Photograph> AllPhotographs { get; private set; } = new List<Photograph>();

        public int CurIndex { get; private set; } = 0;

        public bool IsEmpty
        {
            get => AllPhotographs == null || AllPhotographs.Count == 0;
        }

        public int Count { get => AllPhotographs.Count; }

        public bool HasNext
        {
            get => CurIndex < AllPhotographs.Count - 1;
        }

        public int RemainingFiles
        {
            get
            {
                lock (this)
                {
                    if (IsEmpty)
                        return 0;
                    return AllPhotographs.Count - CurIndex - 1;
                }
            }
        }

        public Photograph Current
        {
            get
            {
                lock (this)
                {
                    return AllPhotographs[CurIndex];
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                lock (this)
                {
                    return AllPhotographs[CurIndex];
                }
            }
        }

        public void Add(Photograph photo)
        {
            lock (this)
            {
                if (photo != null)
                {
                    AllPhotographs.Add(photo);
                    CleanDuplicates();
                }
            }
        }
        public void AddRange(IEnumerable<Photograph> photos)
        {
            lock (this)
            {
                if (photos != null && photos.Count() > 0)
                {
                    AllPhotographs.AddRange(photos);
                    CleanDuplicates();
                }
            }
        }

        private void CleanDuplicates()
        {
            lock (this)
            {
                AllPhotographs = AllPhotographs.Distinct(this).ToList();
            }
        }

        public void CleanNotExisted()
        {
            lock (this)
            {
                AllPhotographs = (from item in AllPhotographs where item.FilePath.IsExisted(out _) select item).ToList();
            }
        }

        public void Clear()
        {
            lock (this)
            {
                AllPhotographs = new List<Photograph>();
            }
        }

        public Photograph Peek()
        {
            lock (this)
            {
                if (IsEmpty || !HasNext)
                {
                    return null;
                }
                else
                {
                    return AllPhotographs[CurIndex + 1];
                }
            }
        }
        public Photograph[] Peek(int nextCount)
        {
            lock (this)
            {
                int practicalCount = RemainingFiles > nextCount ? nextCount : RemainingFiles;
                var nextPhotos = new Photograph[practicalCount];
                for (int i = CurIndex, j = 0; j < practicalCount && i < Count; i++, j++)
                {
                    nextPhotos[j] = AllPhotographs[i];
                }
                return nextPhotos;
            }
        }
        public void Set([NotNull] List<Photograph> photos)
        {
            lock (this)
            {
                AllPhotographs = photos ?? throw new ArgumentNullException();
                CleanDuplicates();
            }
        }

        public void Dispose()
        {
            ;
        }

        public bool MoveNext()
        {
            lock (this)
            {
                if (HasNext)
                {
                    CurIndex++;
                    return true;
                }
                return false;
            }
        }

        public void Reset()
        {
            lock (this)
            {
                CurIndex = 0;
            }
        }

        public bool Equals([AllowNull] Photograph x, [AllowNull] Photograph y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (x is null || y is null)
                return false;

            return x.FilePath == y.FilePath;
        }

        public int GetHashCode([DisallowNull] Photograph obj)
        {
            return obj.FilePath.GetHashCode();
        }
    }
}
