using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace PhotosCategorier;

public class Image
{
    public FileInfo File;

    public Image(FileInfo file)
    {
        File = file;
    }
}

public class ImageList
{
    public readonly IReadOnlyList<Image> Images;

    public int Index { get; private set; } = 0;

    public ImageList()
    {
        Images = Array.Empty<Image>();
    }
    public ImageList(IList<Image> images)
    {
        Images = new ReadOnlyCollection<Image>(images);
    }

    public ImageList(IReadOnlyList<Image> images)
    {
        Images = images;
    }

    public int Count => Images.Count;
    public int Rest => Count - Index;
    

    public static ImageList By(IEnumerable<FileInfo> files)
    {
        return new ImageList((IList<Image>)files.Select(fi => new Image(fi)).ToList());
    }
}