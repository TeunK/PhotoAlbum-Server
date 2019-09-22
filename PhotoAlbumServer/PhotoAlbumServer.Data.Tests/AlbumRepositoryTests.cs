using NSubstitute;
using NUnit.Framework;
using PhotoAlbumServer.Clients.Interfaces;
using PhotoAlbumServer.Data.Repositories;
using PhotoAlbumServer.External.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Tests
{
    public class AlbumRepositoryTests
    {
        private AlbumRepository _repository;
        private ITypicodeClient _mockClient;

        [SetUp]
        public void Setup()
        {
            _mockClient = Substitute.For<ITypicodeClient>();
            _repository = new AlbumRepository(_mockClient);
        }

        [Test]
        public void GetAll_NoClientResults_ReturnsEmptyList()
        {
            IReadOnlyCollection<AlbumDto> clientResponseAlbums = new List<AlbumDto>();
            _mockClient.GetAlbums().Returns(Task.FromResult(clientResponseAlbums));

            var result = _repository.GetAll();

            Assert.AreEqual(0, result.Count);
            _mockClient.Received(1).GetAlbums();
        }

        [Test]
        public void GetAll_WithClientResultsWithoutFilter_ReturnsEmptyList()
        {
            IReadOnlyCollection<AlbumDto> clientResponseAlbums = new List<AlbumDto> {
                new AlbumDto {Id=1},
                new AlbumDto {Id=2},
                new AlbumDto {Id=3},
            };
            _mockClient.GetAlbums().Returns(Task.FromResult(clientResponseAlbums));

            var result = _repository.GetAll();

            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(new List<int> { 1, 2, 3 }, new List<AlbumDto>(result).Select(album => album.Id));
            _mockClient.Received(1).GetAlbums();
        }

        [Test]
        public void GetAll_WithClientResultsWithFilter_ReturnsFilteredList()
        {
            IReadOnlyCollection<AlbumDto> clientResponseAlbums = new List<AlbumDto> {
                new AlbumDto {Id=1},
                new AlbumDto {Id=2},
                new AlbumDto {Id=3},
            };
            _mockClient.GetAlbums().Returns(Task.FromResult(clientResponseAlbums));

            var result = _repository.GetAll(albumDto => albumDto.Id > 2);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result.First().Id);
            _mockClient.Received(1).GetAlbums();
        }
    }
}