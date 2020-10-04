using System;
using System.Collections;
using System.Collections.Generic;

namespace PhotosCategorier.Main
{
    public class PhotographsGenerator : IEnumerator<Photograph>
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
                if (IsEmpty)
                    return 0;
                return AllPhotographs.Count - CurIndex - 1;
            }
        }

        public Photograph Current
        {
            get
            {
                return AllPhotographs[CurIndex];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return AllPhotographs[CurIndex];
            }
        }

        public void Add(Photograph photo)
        {
            AllPhotographs.Add(photo);
        }
        public void AddRange(IEnumerable<Photograph> photos)
        {
            AllPhotographs.AddRange(photos);
        }

        public void Clear()
        {
            AllPhotographs = new List<Photograph>();
        }

        public void Set(List<Photograph> photos)
        {
            AllPhotographs = photos ?? throw new ArgumentNullException();
        }

        public void Dispose()
        {
            ;
        }

        public bool MoveNext()
        {
            if (HasNext)
            {
                CurIndex++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            CurIndex = 0;
        }
    }
}
