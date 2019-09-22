using System;

namespace PhotoAlbumServer.External.DTO
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }
        public Uri ThumbnailUrl { get; set; }
    }
}
