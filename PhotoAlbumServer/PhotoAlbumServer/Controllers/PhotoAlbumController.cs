using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbumServer.Domain;
using PhotoAlbumServer.Interfaces.Services;

namespace PhotoAlbumServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoAlbumController : ControllerBase
    {
        private readonly IPhotoAlbumService _photoAlbumService;

        public PhotoAlbumController(IPhotoAlbumService photoAlbumService)
        {
            _photoAlbumService = photoAlbumService;
        }

        [HttpGet]
        public IEnumerable<PhotoAlbum> Get()
        {
            return _photoAlbumService.GetAll();
        }

        [HttpGet("user/{userId}")]
        public IEnumerable<PhotoAlbum> Get(int userId)
        {
            return _photoAlbumService.GetByUserId(userId);
        }
    }
}
