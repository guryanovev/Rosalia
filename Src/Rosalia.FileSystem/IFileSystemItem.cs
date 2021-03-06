﻿namespace Rosalia.FileSystem
{
    public interface IFileSystemItem
    {
        string AbsolutePath { get; }

        bool Exists { get; }

        string Name { get; }

        void EnsureExists();

        void Delete();

        string GetRelativePath(IDirectory directory);
    }
}