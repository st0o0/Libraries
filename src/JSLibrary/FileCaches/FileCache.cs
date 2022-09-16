using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyCaching.Core;
using JSLibrary.Extensions;
using JSLibrary.FileCaches.Interfaces;
using JSLibrary.Logics.Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace JSLibrary.FileCaches
{
    public class FileCache<ModelType, APILogicType> : IFileCache<ModelType, APILogicType> where APILogicType : class, IAPILogicBase<ModelType> where ModelType : class, IFileCacheModel
    {
        private readonly FileSystemWatcher _watcher;

        public FileCache(ILogger<FileCache<ModelType, APILogicType>> logger, APILogicType apiLogicBase, IEasyCachingProvider easyCachingProvider, string folderName = "Cache")
        {
            this.Logger = logger;
            this.APILogicBase = apiLogicBase;
            this.EasyCachingProvider = easyCachingProvider;
            this.FolderName = folderName;
            this._watcher = new()
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.FileName,
            };
        }

        protected ILogger Logger { get; init; }

        protected APILogicType APILogicBase { get; init; }

        protected IEasyCachingProvider EasyCachingProvider { get; init; }

        public string FolderName { get; }

        public async Task DownloadAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, cancellationToken);
        }

        public async Task DownloadAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(progress, nameof(progress));

            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, progress, cancellationToken);
        }

        public async Task<string> GetFilePathAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, cancellationToken);
            return filepath;
        }

        public async Task<string> GetFilePathAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(progress, nameof(progress));

            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, progress, cancellationToken);
            return filepath!;
        }

        public async Task<byte[]> GetByteArrayAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            string filepath = await this.GetFilePathAsync(model, cancellationToken);
            using FileStream fs = new(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using MemoryStream ms = new((int)fs.Length);

            await fs.CopyToAsync(ms, cancellationToken);
            return ms.ToArray();
        }

        public async Task<byte[]> GetByteArrayAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNull(progress, nameof(progress));

            string filepath = await this.GetFilePathAsync(model, progress, cancellationToken);
            using FileStream fs = new(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using MemoryStream ms = new((int)fs.Length);

            await fs.CopyToAsync(ms, cancellationToken);
            return ms.ToArray();
        }

        public void CheckForClean(TimeSpan timeSpan)
        {
            if (timeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            }

            Directory
            .GetDirectories(GetCachePath(), "", SearchOption.AllDirectories)
            .SelectMany(s => Directory.GetFiles(s))
            .Where(s => File.GetLastAccessTimeUtc(s) < DateTime.UtcNow.Subtract(timeSpan))
            .ToList()
            .ForEach(s => File.Delete(s));
        }

        private string GetCachePath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.FolderName);

        private async Task DownloadAsync(ModelType model, string filepath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filepath))
            {
                try
                {
                    using FileStream fileStream = new(filepath, FileMode.Create, FileAccess.Write, FileShare.Read);
                    await this.APILogicBase.DownloadAsync(model, fileStream, cancellationToken);
                }
                catch
                {
                    File.Delete(filepath);
                    throw;
                }
            }
        }

        private async Task DownloadAsync(ModelType model, string filepath, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filepath))
            {
                try
                {
                    using FileStream fileStream = new(filepath, FileMode.Create, FileAccess.Write, FileShare.Read);
                    await this.APILogicBase.DownloadAsync(model, fileStream, progress, cancellationToken);
                }
                catch
                {
                    File.Delete(filepath);
                    throw;
                }
            }
        }

        // new FileCache with EasyCaching as BytesSafe

        // TODO: Timer mit einem Event zum Checken ob die Datei gebraucht wird

        private async Task<byte[]> GetByteArrayAsyncV2(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            if (!this.EasyCachingProvider.Exists(model.FileName))
            {
                byte[] bytes = await this.APILogicBase.DownloadAsync(model, cancellationToken);
                await this.EasyCachingProvider.SetAsync(model.FileName, bytes, TimeSpan.MaxValue, cancellationToken);
            }

            return (await this.EasyCachingProvider.GetAsync<byte[]>(model.FileName, cancellationToken)!).Value;
        }

        private async Task<string> GetFilePathAsyncV2(ModelType model, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(model);

            byte[] bytes = await this.GetByteArrayAsyncV2(model, cancellationToken);

            string filePath = Path.Combine(this.GetCachePath(), model.FilePath, model.FileName);
            using FileStream fs = new(path: filePath, mode: FileMode.Create);
            await fs.WriteAsync(bytes, cancellationToken);

            return filePath!;
        }
    }
}
