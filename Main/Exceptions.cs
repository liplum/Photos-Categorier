using System;

namespace PhotosCategorier.Main.Exceptions
{
    /// <summary>
    /// No more photo.
    /// </summary>
    [Serializable]
    public class HasNoPhotoException : Exception
    {
        public HasNoPhotoException()
        {
        }
        public HasNoPhotoException(string message) : base(message) { }
        public HasNoPhotoException(string message, Exception inner) : base(message, inner) { }
        protected HasNoPhotoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Selected folder has no photo Or the format is not supported
    /// </summary>
    [Serializable]
    public class NotHoldPhotoException : Exception
    {
        public NotHoldPhotoException()
        {
        }
        public NotHoldPhotoException(string message) : base(message) { }
        public NotHoldPhotoException(string message, Exception inner) : base(message, inner) { }
        protected NotHoldPhotoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// No album to categorize was set!
    /// </summary>
    [Serializable]
    public class NotSetClassifyException : Exception
    {
        public NotSetClassifyException()
        {
        }
        public NotSetClassifyException(string message) : base(message) { }
        public NotSetClassifyException(string message, Exception inner) : base(message, inner) { }
        protected NotSetClassifyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Target folder isn't existed or invalid
    /// </summary>
    [Serializable]
    public class DirectoryNotFoundException : Exception
    {
        public DirectoryNotFoundException()
        {
        }
        public DirectoryNotFoundException(string message) : base(message) { }
        public DirectoryNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected DirectoryNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// File has occupied or deleted
    /// </summary>
    [Serializable]
    public class FileHasOccupiedOrBeenDeletedException : Exception
    {
        public FileHasOccupiedOrBeenDeletedException()
        {
        }
        public FileHasOccupiedOrBeenDeletedException(string message) : base(message) { }
        public FileHasOccupiedOrBeenDeletedException(string message, Exception inner) : base(message, inner) { }
        protected FileHasOccupiedOrBeenDeletedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Can't opreate,Possibly because you has no access or the file is read-only
    /// </summary>
    [Serializable]
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException()
        {
        }
        public UnauthorizedAccessException(string message) : base(message) { }
        public UnauthorizedAccessException(string message, Exception inner) : base(message, inner) { }
        protected UnauthorizedAccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
