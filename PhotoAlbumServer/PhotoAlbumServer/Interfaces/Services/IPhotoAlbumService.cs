using PhotoAlbumServer.Domain;
using System.Collections.Generic;

namespace PhotoAlbumServer.Interfaces.Services
{
    public interface IPhotoAlbumService
    {
        IEnumerable<PhotoAlbum> GetAll();
        IEnumerable<PhotoAlbum> GetByUserId(int userId);
    }
}
