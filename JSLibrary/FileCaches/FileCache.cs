using JSLibrary.FileCaches.Interfaces;
using JSLibrary.Logics.Api.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.FileCaches
{
    public class FileCache<ModelType, ApiLogicType> : IFileCache<ModelType, ApiLogicType> where ModelType : class, IFileCacheModel where ApiLogicType : IApiLogicBase<ModelType>
    {
        private readonly IApiLogicBase<ModelType> apiLogicBase;

        private readonly string folderName = "FileCache";
        private readonly int afterXDays = 14;

        public FileCache(IApiLogicBase<ModelType> apiLogicBase, IFileCacheSettings fileCacheSettings)
        {
            this.folderName = fileCacheSettings.FolderName;
            this.afterXDays = fileCacheSettings.DeleteFilesAfterXDays;
        }

        public FileCache(IApiLogicBase<ModelType> apiLogicBase)
        {
            this.apiLogicBase = apiLogicBase;
        }

        public IApiLogicBase<ModelType> ApiLogicBase => this.apiLogicBase;

        public async Task DownloadAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            string filepath = this.GetFilePath(model);
            if (!File.Exists(filepath))
            {
                using Stream downloadStream = await this.ApiLogicBase.DownloadAsync(model.Id, cancellationToken);
                using FileStream fileStream = File.Create(filepath);
                await downloadStream.CopyToAsync(fileStream, cancellationToken);
            }
        }

        public async Task<string> GetFilePathAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            string filepath = this.GetFilePath(model);
            if (!File.Exists(filepath))
            {
                using Stream downloadStream = await this.ApiLogicBase.DownloadAsync(model.Id, cancellationToken);
                using FileStream fileStream = File.Create(filepath);
                await downloadStream.CopyToAsync(fileStream, cancellationToken);
            }
            return filepath;
        }

        public async Task<FileStream> GetFileStreamAsync(ModelType model, CancellationToken cancellationToken = default)
        {
            string filepath = await this.GetFilePathAsync(model, cancellationToken);
            return File.OpenRead(filepath);
        }

        public void CleanCheck()
        {
            throw new NotImplementedException();
            // TODO: not finished
            Directory
            .GetDirectories("")
            .SelectMany(x => Directory.GetFiles(x))
            .Select(s => new FileInfo(s))
            .Where(fi => fi.CreationTimeUtc < DateTime.UtcNow.Subtract(TimeSpan.FromDays(this.afterXDays)))
            .ToList()
            .ForEach(x => x.Delete());
        }

        private string GetFilePath(ModelType model) => Path.Combine("%TEMP%", folderName, model.FilePath, model.FileName);
    }
}