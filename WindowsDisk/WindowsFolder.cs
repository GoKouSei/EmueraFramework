using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PCLStorage;
using PCLStorage.Exceptions;
using System.IO;

namespace WindowsDisk
{
    class WindowsFolder : IFolder
    {
        private DirectoryInfo _info;

        private string Combine(string path) => System.IO.Path.Combine(_info.FullName, path);

        public WindowsFolder(DirectoryInfo info)
        {
            _info = info;
        }

        public string Name => _info.Name;

        public string Path => _info.FullName;

        public Task<ExistenceCheckResult> CheckExistsAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                var path = Combine(name);
                if (Directory.Exists(path))
                    return ExistenceCheckResult.FolderExists;
                else if (File.Exists(path))
                    return ExistenceCheckResult.FileExists;
                else
                    return ExistenceCheckResult.NotFound;
            });
        }

        public Task<IFile> CreateFileAsync(string desiredName, CreationCollisionOption option, CancellationToken cancellationToken = default(CancellationToken))
        {
            string desiredPath = Combine(desiredName);
            switch (option)
            {
                case CreationCollisionOption.FailIfExists:
                    if (File.Exists(desiredPath))
                        throw new IOException();
                    File.Create(desiredPath);
                    break;
                case CreationCollisionOption.GenerateUniqueName:
                case CreationCollisionOption.OpenIfExists:
                    if (!File.Exists(desiredPath))
                        File.Create(desiredPath);
                    break;
                case CreationCollisionOption.ReplaceExisting:
                    if (File.Exists(desiredPath))
                    {
                        File.Delete(desiredPath);
                        File.Create(desiredPath);
                    }
                    break;
            }
            return Task.Run(() => new WindowsFile(new FileInfo(desiredPath)) as IFile);
        }

        public Task<IFolder> CreateFolderAsync(string desiredName, CreationCollisionOption option, CancellationToken cancellationToken = default(CancellationToken))
        {
            string desiredPath = Combine(desiredName);
            switch (option)
            {
                case CreationCollisionOption.FailIfExists:
                    if (Directory.Exists(desiredPath))
                        throw new IOException();
                    Directory.CreateDirectory(desiredPath);
                    break;
                case CreationCollisionOption.GenerateUniqueName:
                case CreationCollisionOption.OpenIfExists:
                    if (!Directory.Exists(desiredPath))
                        Directory.CreateDirectory(desiredPath);
                    break;
                case CreationCollisionOption.ReplaceExisting:
                    if (Directory.Exists(desiredPath))
                    {
                        Directory.Delete(desiredPath, true);
                        Directory.CreateDirectory(desiredPath);
                    }
                    break;
            }
            return Task.Run(() => new WindowsFolder(new DirectoryInfo(desiredPath)) as IFolder);
        }

        public Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => _info.Delete(true));
        }

        public Task<IFile> GetFileAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => new WindowsFile(_info.GetFiles(name, SearchOption.AllDirectories)[0]) as IFile);
        }

        public Task<IList<IFile>> GetFilesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => _info.GetFiles("*", SearchOption.AllDirectories).Select(info => new WindowsFile(info)).Cast<IFile>().ToList() as IList<IFile>);
        }

        public Task<IFolder> GetFolderAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => new WindowsFolder(_info.GetDirectories(name, SearchOption.AllDirectories)[0]) as IFolder);
        }

        public Task<IList<IFolder>> GetFoldersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => _info.GetDirectories("*", SearchOption.AllDirectories).Select(info => new WindowsFolder(info)).Cast<IFolder>().ToList() as IList<IFolder>);
        }
    }
}
