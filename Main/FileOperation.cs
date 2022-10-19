#nullable enable
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using My_DirectoryNotFoundException = PhotosCategorier.DirectoryNotFoundException;

namespace PhotosCategorier;

public sealed partial class MainWindow
{
    public bool IsIncludeSubfolder
    {
        get => Properties.Settings.Default.IncludeSubfolder;
        set
        {
            var settings = Properties.Settings.Default;

            if (settings.IncludeSubfolder == value) return;
            settings.IncludeSubfolder = value;
            OnPropertyChanged();
        }
    }

    public bool IsDeleteToRecycleBin
    {
        get => Properties.Settings.Default.DeleteToRecycleBin;
        set
        {
            var settings = Properties.Settings.Default;

            if (settings.DeleteToRecycleBin == value) return;
            settings.DeleteToRecycleBin = value;
            OnPropertyChanged();
        }
    }

    public bool HasDeletedTip
    {
        get => Properties.Settings.Default.DeletedTip;
        set
        {
            var settings = Properties.Settings.Default;

            if (settings.DeletedTip == value) return;
            settings.DeletedTip = value;
            OnPropertyChanged();
        }
    }

    private string leftArrowContent = Properties.Resources.Left;

    public string LeftArrowContent
    {
        get => leftArrowContent;
        set
        {
            if (leftArrowContent == value) return;
            leftArrowContent = value;
            OnPropertyChanged();
        }
    }

    private string rightArrowContent = Properties.Resources.Right;

    public string RightArrowContent
    {
        get => rightArrowContent;
        set
        {
            if (rightArrowContent == value) return;
            rightArrowContent = value;
            OnPropertyChanged();
        }
    }

    private bool isEnd = true;

    public bool IsEnd
    {
        get => isEnd;
        set
        {
            if (isEnd == value) return;
            isEnd = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Open this file as a photo via Windows' Photos.
    /// <br/>
    /// If this photo isn't existed ,it throw <see cref="FileHasOccupiedOrBeenDeletedException"/>
    /// </summary>
    /// <exception cref="FileHasOccupiedOrBeenDeletedException"></exception>
    private void OpenThisPhoto()
    {
        var cur = photographs.Current;
        if (cur is null) return;
        var curFile = new FileInfo(cur.FilePath);
        if (curFile.Exists)
        {
            var path = curFile.FullName;
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
        else
        {
            throw new FileHasOccupiedOrBeenDeletedException();
        }
    }

    /// <summary>
    /// Open the folder in which this photo located via Windows' Explorer.
    /// <br/>
    /// If this photo isn't existed ,it throw <see cref="FileHasOccupiedOrBeenDeletedException"/> 
    /// <br/>
    /// If the folder isn't existed ,it throw <see cref="PhotosCategorier.DirectoryNotFoundException"/>
    /// </summary>
    /// <exception cref="FileHasOccupiedOrBeenDeletedException"></exception>
    /// <exception cref="PhotosCategorier.DirectoryNotFoundException"></exception>
    private void OpenFolderWherePhotoIs()
    {
        var cur = photographs.Current;
        if (cur is null) return;
        var curFile = new FileInfo(cur.FilePath);
        if (curFile.Exists)
        {
            var dir = curFile.Directory;
            if (dir is not null && dir.Exists)
            {
                System.Diagnostics.Process.Start("explorer.exe", dir.FullName);
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
        else
        {
            throw new FileHasOccupiedOrBeenDeletedException();
        }
    }

    /// <summary>
    /// Copy this photo into the Clipboard.
    /// </summary>
    /// <exception cref="FileHasOccupiedOrBeenDeletedException"></exception>
    private void CopyCurrent()
    {
        var cur = photographs.Current;
        if (cur is null) return;
        var curFile = new FileInfo(cur.FilePath);
        if (curFile.Exists)
        {
            var files = new StringCollection
            {
                curFile.FullName
            };
            Clipboard.SetFileDropList(files);
        }
        else
        {
            throw new FileHasOccupiedOrBeenDeletedException();
        }
    }

    /// <summary>
    /// select files to add.
    /// </summary>
    /// <returns>whether added any image</returns>
    /// <param name="title"></param>
    private void SelectFilesToAdd(string title)
    {
        var files = SelectFiles(title);
        if (files is null) return;
        AddImagesAndTryRefresh(files);
    }

    /// <summary>
    /// select folders to add.
    /// </summary>
    /// <returns>whether added any image</returns>
    /// <param name="title"></param>
    private void SelectFoldersToAdd(string title)
    {
        var directories = SelectFolders(title);
        if (directories is null) return;
        var li = new List<FileInfo>();
        foreach (var dir in directories)
        {
            li.AddRange(dir.GetFiles("*", SearchOption.AllDirectories));
        }
        AddImagesAndTryRefresh(li);
    }

    /// <summary>
    /// add images and try to refresh UI when any file was added.
    /// </summary>
    /// <param name="files">to add</param>
    private void AddImagesAndTryRefresh(IEnumerable<FileInfo> files)
    {
        var size = allImages.Count;
        allImages.AddRange(files);
        allImages = allImages.Distinct().ToList();
        if (size != allImages.Count)
        {
            RefreshUi();
        }
    }

    private void RefreshUi()
    {
        UpdateRemainingFileCounter();
        InitComponent();
        InitImage();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns>whether added any image</returns>
    private void DropFolderOrFile(IEnumerable array)
    {
        var images = new List<FileInfo>();
        foreach (var path in array.OfType<string>().Select(item => item))
        {
            if (Directory.Exists(path))
            {
                images.AddRange(Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                    .Select(item => new FileInfo(item)));
            }
            else if (File.Exists(path))
            {
                images.Add(new FileInfo(path));
            }
        }

        AddImagesAndTryRefresh(images);
    }

    private void SetLeftFolderWithSelection()
    {
        leftArrow = SelectFolder(Properties.Resources.SelectLeftFolder);
        SetLeftFolder();
    }

    private void SetLeftFolder()
    {
        if (leftArrow == null) return;
        LeftArrowContent = leftArrow.Name;
        ToLeft.IsEnabled = true;
        LeftArrowPointedToFolder.Visibility = Visibility.Visible;
    }

    private void SetRightFolderWithSelection()
    {
        rightArrow = SelectFolder(Properties.Resources.SelectRightFolder);
        SetRightFolder();
    }

    private void SetRightFolder()
    {
        if (rightArrow == null) return;
        RightArrowContent = rightArrow.Name;
        ToRight.IsEnabled = true;
        RightArrowPointedToFolder.Visibility = Visibility;
    }

    private static DirectoryInfo? SelectFolder(string caption)
    {
        using var dialog = new CommonOpenFileDialog(caption)
        {
            IsFolderPicker = true,
        };

        var mode = dialog.ShowDialog();

        return mode != CommonFileDialogResult.Ok
            ? null
            : new DirectoryInfo(dialog.FileName);
    }

    private static IEnumerable<FileInfo>? SelectFiles(string title)
    {
        using var dialog = new CommonOpenFileDialog(title)
        {
            IsFolderPicker = false,
            Multiselect = true
        };

        var mode = dialog.ShowDialog();

        return mode != CommonFileDialogResult.Ok
            ? null
            : (from file in dialog.FileNames select new FileInfo(file)).ToList();
    }

    private static List<DirectoryInfo>? SelectFolders(string title)
    {
        using var dialog = new CommonOpenFileDialog(title)
        {
            IsFolderPicker = true,
            Multiselect = true
        };

        var mode = dialog.ShowDialog();

        return mode != CommonFileDialogResult.Ok
            ? null
            : (from file in dialog.FileNames select new DirectoryInfo(file)).ToList();
    }
}