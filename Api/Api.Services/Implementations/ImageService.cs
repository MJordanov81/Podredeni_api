namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Infrastructure.Constants;
    using Interfaces;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImageService : IImageService
    {
        private readonly ApiDbContext db;

        public ImageService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(string url, string productId)
        {
            if(!string.IsNullOrWhiteSpace(url))
            {
                Image image = new Image { Url = url, ProductId = productId, CreationDateTime = DateTime.Now };

                try
                {
                    await this.db.Images.AddAsync(image);
                }
                catch
                {
                    throw new InvalidOperationException(ErrorMessages.UnableToWriteToDb);
                }

                await this.db.SaveChangesAsync();

                return image.Id;
            }

            throw new ArgumentException(ErrorMessages.InvalidImageUrl);
        }

        public async Task Delete(string id)
        {
            if (!this.db.Images.Any(i => i.Id == id)) throw new ArgumentException(ErrorMessages.InvalidImageId);

            Image imageToDelete = await this.db.Images.FindAsync(id);

            this.db.Images.Remove(imageToDelete);

            await this.db.SaveChangesAsync();
        }
    }
}
