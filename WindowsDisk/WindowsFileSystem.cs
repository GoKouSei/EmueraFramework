using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PCLStorage;

namespace WindowsDisk
{
    public class WindowsFileSystem : IFileSystem
    {
        private string _root;

        public WindowsFileSystem(string root)
        {
            _root = root;
        }


        public IFolder LocalStorage
        {
            get
            {
                return new WindowsFolder(new System.IO.DirectoryInfo(_root));
            }
        }

        public IFolder RoamingStorage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task<IFile> GetFileFromPathAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => new WindowsFile(new System.IO.FileInfo(path)) as IFile);
        }

        public Task<IFolder> GetFolderFromPathAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => new WindowsFolder(new System.IO.DirectoryInfo(path)) as IFolder);
        }
    }
}
