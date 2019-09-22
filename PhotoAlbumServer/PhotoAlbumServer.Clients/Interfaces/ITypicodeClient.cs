using PhotoAlbumServer.External.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAlbumServer.Clients.Interfaces
{
    public interface ITypicodeClient
    {
        Task<IReadOnlyCollection<AlbumDto>> GetAlbums();
        Task<IReadOnlyCollection<PhotoDto>> GetPhotos();
    }
}
