﻿using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using StandardProject.Domain.Entities;
using StandardProject.Domain.Extensions;
using StandardProject.Domain.Services.Storage;
using StandardProject.Domain.ValueObjects;

namespace StandardProject.Infrastructure.Services.Storage;
public class AzureStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task Upload(User user, Stream file, string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        await container.CreateIfNotExistsAsync();

        var blobClient = container.GetBlobClient(fileName);

        await blobClient.UploadAsync(file, overwrite: true);
    }

    public async Task<string> GetFileUrl(User user, string fileName)
    {
        var containerName = user.UserIdentifier.ToString();

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var exist = await containerClient.ExistsAsync();
        if (!exist.Value)
            return string.Empty;

        var blobClient = containerClient.GetBlobClient(fileName);
        exist = await blobClient.ExistsAsync();
        if (exist.Value)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(StandardProjectRuleConstants.MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES),
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }

        return string.Empty;
    }

    public async Task Delete(User user, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        var exist = await containerClient.ExistsAsync();
        if (exist.Value)
        {
            await containerClient.DeleteBlobIfExistsAsync(fileName);
        }
    }

    public async Task DeleteContainer(Guid userIdentifier)
    {
        var container = _blobServiceClient.GetBlobContainerClient(userIdentifier.ToString());
        await container.DeleteIfExistsAsync();
    }
}