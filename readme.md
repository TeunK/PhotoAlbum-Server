
# Photo Album Service

## Task

Create a Web API that when called
- Calls, combines and returns the result of:
-- http://jsonplaceholder.typicode.com/photos
-- http://jsonplaceholder.typicode.com/photos
- Allows an integrator to filter on the user id - so just returns the albums and photos relevant to a single user.

## Solution

To start hosting the API, simply run the `PhotoAlbumServer` solution.

Tests are located in the `Tests` directory.

### Endpoints

- [http://localhost:58189/api/photoAlbum](http://localhost:58189/api/photoAlbum)
Returns list of all `PhotoAlbum` provided by the typicode.com api.

- [http://localhost:58189/api/photoAlbum/user/\{userId\}](http://localhost:58189/api/photoAlbum/user/3)
Returns list of `PhotoAlbum` where the _**UserId**_ matches.

#### Domain Types:

**PhotoAlbum:**
```
{
	album: Album,
	photos: Photo[]
}
```

**Album:**
```
{
	id: int,
	userId: int,
	title: string
}
```

**Photo:**
```
{
	id: int,
	albumId: int,
	title: string,
	url: Uri,
	thumbnail: Uri
}
```

