
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Feed.Application.Interfaces.Services;
using Feed.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Feed.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly ICloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
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
                                        .Quality("Auto")
                                        .FetchFormat("Auto")
                };
                
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if(uploadResult.Error != null)
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


    }
}
