using JSLibrary.Extensions;
using JSLibrary.FileCaches.Interfaces;
using JSLibrary.Logics.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.FileCaches
{
    public class FileCache<ModelType, ApiLogicType> : IFileCache<ModelType, ApiLogicType> where ApiLogicType : class, IApiLogicBase<ModelType> where ModelType : class, IFileCacheModel
    {
        public FileCache(ApiLogicType apiLogicBase, string folderName = "Cache")
        {
            this.ApiLogicBase = apiLogicBase;
            this.FolderName = folderName;
        }

        public ApiLogicType ApiLogicBase { get; }

        public string FolderName { get; }

        public async Task DownloadAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, cancellationToken);
        }

        public async Task DownloadAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, progress, cancellationToken);
        }

        public async Task<string> GetFilePathAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, cancellationToken);
            return filepath;
        }

        public async Task<string> GetFilePathAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            string filepath = Path.Combine(GetCachePath(), model.FilePath, model.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            await this.DownloadAsync(model, filepath, progress, cancellationToken);
            return filepath;
        }

        public async Task<FileStream> GetFileStreamAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            string filepath = await this.GetFilePathAsync(model, cancellationToken);
            return new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public async Task<FileStream> GetFileStreamAsync(ModelType model, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            string filepath = await this.GetFilePathAsync(model, progress, cancellationToken);
            return new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void CheckForClean(TimeSpan timeSpan)
        {
            if (timeSpan < TimeSpan.Zero)
            {
                throw new ArgumentNullException(nameof(timeSpan));
            }

            Directory
            .GetDirectories(GetCachePath(), "", SearchOption.AllDirectories)
            .SelectMany(x => Directory.GetFiles(x))
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
                    await this.ApiLogicBase.DownloadAsync(model, fileStream, cancellationToken);
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
                    await this.ApiLogicBase.DownloadAsync(model, fileStream, progress, cancellationToken);
                }
                catch
                {
                    File.Delete(filepath);
                    throw;
                }
            }
        }
    }
}