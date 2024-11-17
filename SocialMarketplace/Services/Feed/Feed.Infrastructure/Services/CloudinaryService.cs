
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Feed.Application.DTOs;
using Feed.Application.Interfaces.Services;
using Feed.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Feed.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly ICloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        private const int MaxImageSizeInMB = 5;
        private const int MaxVideoSizeInMB = 100;
        private readonly string[] AllowedImageTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif" };
        private readonly string[] AllowedVideoTypes = { "video/mp4", "video/mpeg", "video/quicktime" };
        public CloudinaryService(IOptions<CloudinarySettings> config, ILogger<CloudinaryService> logger)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _logger = logger;
        }
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("No file provid");
                }

                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                                        .Width(500)
                                        .Crop("scale")
                                        .Quality("auto")
                                        .FetchFormat("auto"),
                    AssetFolder = "SocialMarketplace"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception($"Failed to upload image: {uploadResult.Error.Message}");
                }

                return uploadResult;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occur while uploading the image.", ex);
            }
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                return await _cloudinary.DestroyAsync(deleteParams);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while deleting the image.", ex);
            }
        }

        public async Task<List<MediaDto>> UploadMultipleImagesAsync(IList<IFormFile> files, string folder = "")
        {
            var uploadResults = new List<MediaDto>();
            var uploadTasks = new List<Task<MediaDto>>();

            foreach (var file in files)
            {
                if (!IsImageValid(file, out string errorMessage))
                {
                    _logger.LogWarning($"Invalid image file: {file.FileName}. {errorMessage}");
                    continue;
                }

                uploadTasks.Add(UploadSingleImageAsync(file, folder));
            }

            try
            {
                var results = await Task.WhenAll(uploadTasks);
                uploadResults.AddRange(results.Where(r => r != null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading multiple images to Cloudinary");
                throw new Exception("One or more images failed to upload", ex);
            }

            return uploadResults;
        }

        public async Task<List<MediaDto>> UploadMultipleVideosAsync(IList<IFormFile> files, string folder = "")
        {
            var uploadResults = new List<MediaDto>();
            var uploadTasks = new List<Task<MediaDto>>();

            foreach (var file in files)
            {
                if (!IsVideoValid(file, out string errorMessage))
                {
                    _logger.LogWarning($"Invalid video file: {file.FileName}. {errorMessage}");
                    continue;
                }

                uploadTasks.Add(UploadSingleVideoAsync(file, folder));
            }

            try
            {
                var results = await Task.WhenAll(uploadTasks);
                uploadResults.AddRange(results.Where(r => r != null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading multiple videos to Cloudinary");
                throw new Exception("One or more videos failed to upload", ex);
            }

            return uploadResults;
        }

        private async Task<MediaDto> UploadSingleImageAsync(IFormFile file, string folder)
        {
            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    AssetFolder = folder,
                    Transformation = new Transformation()
                        .Width(500).Crop("scale")
                        .Quality("auto")
                        .FetchFormat("auto"),
                    UseFilenameAsDisplayName = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception($"Failed to upload image: {uploadResult.Error.Message}");
                }

                return new MediaDto
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.ToString(),
                    ContentType = uploadResult.ResourceType,
                    FileSize = uploadResult.Length,
                    Format = uploadResult.Format,
                    Width = uploadResult.Width,
                    Height = uploadResult.Height
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading image {file.FileName}");
                return null;
            }
        }

        private async Task<MediaDto> UploadSingleVideoAsync(IFormFile file, string folder)
        {
            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    AssetFolder = folder,
                    EagerTransforms = new List<Transformation>()
                    {
                        new Transformation()
                            .Quality("auto")
                            .FetchFormat("mp4"),
                        new Transformation()
                            .Width(500)
                            //.Height(240)
                            .FetchFormat("jpg")
                            .Crop("scale")
                            .StartOffset("0")
                            .Effect("preview:duration_2")
                    },
                    EagerAsync = true,
                    UseFilenameAsDisplayName = true
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception($"Failed to upload video: {uploadResult.Error.Message}");
                }

                return new MediaDto
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.ToString(),
                    ContentType = uploadResult.ResourceType,
                    FileSize = uploadResult.Length,
                    Format = uploadResult.Format,
                    Duration = uploadResult.Duration,
                    ThumbnailUrl = Path.ChangeExtension(uploadResult.SecureUrl.ToString(), "jpg")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading video {file.FileName}");
                return null;
            }
        }

        private bool IsImageValid(IFormFile file, out string errorMessage)
        {
            if (file == null || file.Length == 0)
            {
                errorMessage = "File is empty";
                return false;
            }

            if (file.Length > MaxImageSizeInMB * 1024 * 1024)
            {
                errorMessage = $"Image size exceeds maximum limit of {MaxImageSizeInMB}MB";
                return false;
            }

            if (!AllowedImageTypes.Contains(file.ContentType.ToLower()))
            {
                errorMessage = "Invalid image type. Only JPEG, PNG, and GIF are allowed";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        private bool IsVideoValid(IFormFile file, out string errorMessage)
        {
            if (file == null || file.Length == 0)
            {
                errorMessage = "File is empty";
                return false;
            }

            if (file.Length > MaxVideoSizeInMB * 1024 * 1024)
            {
                errorMessage = $"Video size exceeds maximum limit of {MaxVideoSizeInMB}MB";
                return false;
            }

            if (!AllowedVideoTypes.Contains(file.ContentType.ToLower()))
            {
                errorMessage = "Invalid video type. Only MP4, MPEG, and QuickTime are allowed";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
        public bool IsFileValid(IFormFile file, out string errorMessage)
        {

            if (!IsImageValid(file, out errorMessage) && !IsVideoValid(file, out errorMessage))
            {
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
        private bool IsImage(string contentType) => AllowedImageTypes.Contains(contentType.ToLower());
        private bool IsVideo(string contentType) => AllowedVideoTypes.Contains(contentType.ToLower());
        public async Task<List<DeletionResult>> DeleteMultipleFilesAsync(List<string> publicIds)
        {
            var deletionResults = new List<DeletionResult>();
            var deletionTasks = new List<Task<DeletionResult>>();

            foreach (var publicId in publicIds)
            {
                deletionTasks.Add(_cloudinary.DestroyAsync(new DeletionParams(publicId)));
            }

            try
            {
                var results = await Task.WhenAll(deletionTasks);
                deletionResults.AddRange(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting multiple files from Cloudinary");
                throw new Exception("One or more files failed to delete", ex);
            }

            return deletionResults;
        }

        public async Task<List<MediaDto>> UploadMultipleFilesAsync(IList<IFormFile> files, string folder = "")
        {
            var uploadResults = new List<MediaDto>();
            var uploadTasks = new List<Task<MediaDto>>();

            foreach (var file in files)
            {
                uploadTasks.Add(UploadSingleFileAsync(file, folder));
            }

            try
            {
                var results = await Task.WhenAll(uploadTasks);
                uploadResults.AddRange(results.Where(r => r != null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading multiple files to Cloudinary");
                throw new Exception("One or more files failed to upload", ex);
            }

            return uploadResults;
        }

        private async Task<MediaDto> UploadSingleFileAsync(IFormFile file, string folder)
        {
            try
            {
                if (!IsFileValid(file, out string errorMessage))
                {
                    _logger.LogWarning($"Invalid file: {file.FileName}. {errorMessage}");
                    return null;
                }

                using var stream = file.OpenReadStream();

                if (IsImage(file.ContentType))
                {
                    return await UploadSingleImageAsync(file, folder);
                }
                else // Video
                {
                    return await UploadSingleVideoAsync(file, folder);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading file {file.FileName}");
                return null;
            }
        }

    }
}
