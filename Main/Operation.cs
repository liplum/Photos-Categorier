﻿using System;
using PhotosCategorier.Main.Exceptions;
using PhotosCategorier.Photo;
using PhotosCategorier.Utils;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using PhotosCategorier.Server;
using DirectoryNotFoundException = System.IO.DirectoryNotFoundException;
using My_DirectoryNotFoundException = PhotosCategorier.Main.Exceptions.DirectoryNotFoundException;
using My_UnauthorizedAccessException = PhotosCategorier.Main.Exceptions.UnauthorizedAccessException;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace PhotosCategorier.Main
{
    public partial class MainWindow
    {
        private bool inited = false;
        public bool Inited
        {
            get => inited;
            set
            {
                if (inited != value)
                {
                    inited = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Refresh()
        {
            photographs.CleanNotExisted();
            var needRemove = new List<Album>();
            var needAdd = new List<Photograph>();
            foreach (var album in allClassifyFolder)
            {
                var ps = album.GetAllPhotographs();
                if (ps == null)
                {
                    needRemove.Add(album);
                }
                else
                {
                    needAdd.AddRange(ps);
                }
            }
            allClassifyFolder.RemoveAll(item => needRemove.Contains(item));
            photographs.AddRange(needAdd);
            IsEnd = photographs.IsEmpty;
            InitImage();
        }

        private void Clear()
        {
            photographs.Clear();
            allClassifyFolder.Clear();
            ResetComponent();
            UpdateRemainingFileCounter();
        }

        private void InitComponent()
        {
            IsEnd = false;
            Inited = true;
        }

        private void ResetComponent()
        {
            Inited = false;
            IsEnd = true;
            ClearCurImage();
            ResetPhotoInfo();
        }

        private enum Arrow
        {
            LeftArrow, RightArrow
        }

        /// <summary>
        /// Checking whether current photo is the last one
        /// </summary>
        /// <returns>If the current photo is the last one or it hasn't arrived yet,it'll return true.Otherwise,it had ended and return false.</returns>
        private bool CheckLastOne()
        {
            if (!photographs.HasNext)
            {
                ClearCurImage();
                MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
                IsEnd = true;
                return false;
            }
            return true;
        }

        private enum EmptyMessage
        {
            HasNoPhoto, NotSetClassify, NotHoldPhoto
        }

        /// <summary>
        /// Checking whether the photographs is empty or not . If it's empty , it'll pop a message box with what you want to display
        /// </summary>
        /// <param name="emptyMessage"></param>
        /// <returns>If photographs is empty , it'll return ture.</returns>
        private bool CheckEmptyWithMessage(EmptyMessage emptyMessage)
        {
            if (!photographs.IsEmpty) return false;
            switch (emptyMessage)
            {
                case EmptyMessage.HasNoPhoto:
                    MessageBox.Show(Properties.Resources.HasNoPhoto, Properties.Resources.Error);
                    break;
                case EmptyMessage.NotSetClassify:
                    MessageBox.Show(Properties.Resources.NotSetClassifyFolder, Properties.Resources.Error);
                    break;
                case EmptyMessage.NotHoldPhoto:
                    MessageBox.Show(Properties.Resources.NotHoldPhoto, Properties.Resources.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(emptyMessage), emptyMessage, null);
            }
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrow"></param>
        /// <exception cref="My_DirectoryNotFoundException"></exception>
        /// <exception cref="My_UnauthorizedAccessException"></exception>
        /// <exception cref="FileHasOccupiedOrBeenDeletedException"></exception>
        private void MoveThisTo(Arrow arrow)
        {
            if (CheckLastOne())
            {
                var photo = photographs.Current;
                try
                {
                    switch (arrow)
                    {
                        case Arrow.LeftArrow:
                            photo.FilePath.MoveTo(leftArrow);
                            break;
                        case Arrow.RightArrow:
                            photo.FilePath.MoveTo(rightArrow);
                            break;
                    }
                    NextImage();
                }
                catch (DirectoryNotFoundException)
                {
                    DirectoryInfo target = null;
                    switch (arrow)
                    {
                        case Arrow.LeftArrow:
                            target = leftArrow;
                            break;
                        case Arrow.RightArrow:
                            target = rightArrow;
                            break;
                    }
                    throw new My_DirectoryNotFoundException(target.FullName);
                    //MessageBox.Show($"{Properties.Resources.DirectoryNotFound}\n{target.FullName}", Properties.Resources.Error);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new My_UnauthorizedAccessException(photo.FilePath);
                    //MessageBox.Show($"{Properties.Resources.UnauthorizedAccess}\n{photo.FilePath}", Properties.Resources.Error);
                }
                catch (IOException)
                {
                    throw new FileHasOccupiedOrBeenDeletedException(photo.FilePath);
                    //MessageBox.Show($"{Properties.Resources.FileHasOccupiedOrHasDeleted}\n{photo.FilePath}", Properties.Resources.Error);
                }
            }
        }

        private void SkipThisPhoto()
        {
            if (CheckLastOne())
            {
                NextImage();
            }
        }

        private void DeleteThisPhoto()
        {
            CheckLastOne();
            if (!IsEnd)
            {
                var photo = photographs.Current;
                var file = photo.FilePath;
                if (!HasDeletedTip || MessageBox.Show($"{Properties.Resources.ConfirmDeletion}\n{file.GetLastName()}",
                    Properties.Resources.Warnning, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    file.Deleted();
                    NextImage();
                }
            }
        }



    }
}
