using Newtonsoft.Json;
using PhotoAlbumServer.Clients.Interfaces;
using PhotoAlbumServer.External.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAlbumServer.Clients.Clients
{
    public class TypicodeClient : ITypicodeClient
    {
        private readonly HttpClient _httpClient;

        public TypicodeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<AlbumDto>> GetAlbums()
        {
            var response = await _httpClient.GetStringAsync("albums");
            var albums = JsonConvert.DeserializeObject<IReadOnlyCollection<AlbumDto>>(response);
            return albums;
        }

        public async Task<IReadOnlyCollection<PhotoDto>> GetPhotos()
        {
            var response = await _httpClient.GetStringAsync("photos");
            var photos = JsonConvert.DeserializeObject<IReadOnlyCollection<PhotoDto>>(response);
            return photos;
        }
    }
}
