using MediatR;
using NewsBook.Repository;

namespace NewsBook.Application.Commands.Images
{
    public class ImageCommandHandler :
        IRequestHandler<InsertProfileCommand, string>,
        IRequestHandler<UpdateProfileCommand, string>
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IImageRepository _imageRepository;
        public ImageCommandHandler(IWebHostEnvironment environment, IImageRepository imageRepository)
        {
            _imageRepository= imageRepository;
            _environment = environment;
        }

        public async Task<string> Handle(InsertProfileCommand request, CancellationToken cancellationToken)
        {
            //NOTE : Another Method
            //string uniqueFileName = null;
            //if(request.Picture.Length > 0)
            //{
            //    string uploadFolfer = Path.Combine(_environment.WebRootPath, "UserProfiles");
            //    uniqueFileName = Guid.NewGuid().ToString() + "_" + request.Picture.FileName;
            //    string filePath = Path.Combine(uploadFolfer, uniqueFileName);
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        request.Picture.CopyTo(fileStream);
            //    }
            //}
            var basePath = Path.Combine(_environment.ContentRootPath, "UsersProfiles");
            var fileExtension = Path.GetExtension(request.Picture.FileName);
            var profilePicturePath = Path.Combine(basePath, $"{Path.GetRandomFileName()}{fileExtension}");

            if (request.Picture.Length > 0)
            {
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                using (FileStream fileStream = File.Create(profilePicturePath))
                {
                    await request.Picture.CopyToAsync(fileStream);
                }
            }
            return await _imageRepository.Add(profilePicturePath);
        }

        public Task<string> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
