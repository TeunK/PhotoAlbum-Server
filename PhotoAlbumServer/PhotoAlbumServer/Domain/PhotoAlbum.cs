using System.Collections.Generic;

namespace PhotoAlbumServer.Domain
{
    public class PhotoAlbum
    {
        public Album Album { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}
