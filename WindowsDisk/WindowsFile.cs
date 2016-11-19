using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PCLStorage;

namespace WindowsDisk
{
    public class WindowsFile : IFile
    {
        private FileInfo _info;

        public WindowsFile(FileInfo info)
        {
            _info = info;
        }

        public string Name => _info.Name;

        public string Path => _info.FullName;

        public Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => _info.Delete());
        }

        public Task MoveAsync(string newPath, NameCollisionOption collisionOption = NameCollisionOption.ReplaceExisting, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => _info.MoveTo(newPath));
        }

        public Task<Stream> OpenAsync(PCLStorage.FileAccess fileAccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => _info.Open(FileMode.Open, fileAccess == PCLStorage.FileAccess.Read ? System.IO.FileAccess.Read : System.IO.FileAccess.ReadWrite) as Stream);
        }

        public Task RenameAsync(string newName, NameCollisionOption collisionOption = NameCollisionOption.FailIfExists, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => _info.MoveTo(System.IO.Path.Combine(_info.DirectoryName, newName)));
        }
    }
}
