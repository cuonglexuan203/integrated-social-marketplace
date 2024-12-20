﻿using Chat.Application.Dtos;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Interfaces.Services
{
    public interface ICloudinaryService
    {
        //Task<ImageUploadResult> UploadImageAsync(IFormFile file);
        //Task<DeletionResult> DeleteImageAsync(string publicId);
        Task<List<MediaDto>> UploadMultipleImagesAsync(IList<IFormFile> files, string folder = "");
        Task<List<MediaDto>> UploadMultipleVideosAsync(IList<IFormFile> files, string folder = "");
        Task<List<DeletionResult>> DeleteMultipleFilesAsync(List<string> publicIds);
        Task<List<MediaDto>> UploadMultipleFilesAsync(IList<IFormFile> files, string folder = "");
        bool IsFileValid(IFormFile file, out string errorMessage);
        Task<MediaDto> UploadSingleFileAsync(IFormFile file, string folder = "");
    }
}
