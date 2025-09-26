using Bogus;
using Moq;
using StandardProject.Domain.Entities;
using StandardProject.Domain.Services.Storage;

namespace CommonTestUtilities.BlobStorage;
public class BlobStorageServiceBuilder
{
    private readonly Mock<IBlobStorageService> _mock;

    public BlobStorageServiceBuilder() => _mock = new Mock<IBlobStorageService>();

    public BlobStorageServiceBuilder GetFileUrl(User user, string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return this;

        var faker = new Faker();
        var imageUrl = faker.Image.LoremFlickrUrl();

        _mock.Setup(blobStorage => blobStorage.GetFileUrl(user, fileName)).ReturnsAsync(imageUrl);

        return this;
    }

    public IBlobStorageService Build() => _mock.Object;
}
