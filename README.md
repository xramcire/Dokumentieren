## About The Project
Dokumentieren is a simple document upload service created for learning purposes.

### Architectural Considerations
* Follow REST standards.
* Include automated tests.
* Thread safe.
* Handle multiple concurrent requests to the same file.
* Use async where available.
* Use dependency injection where appropriate.
* Support multiple file storage services. 
* Separate large concerns by project (Web / Services).
* Separate smaller concerns by class.

### Future Improvements

* Implement the BucketDocumentService class. Perhaps break it into a class by service (AWS, Azure, Google).
* Implement authentication.
* Implement logging (exceptions, observable metrics). 
* Implement http 302 Not Modified.
* Implement compression.
* Implement configuration.
* Implement configuration to select storage service.
* Implement document searching.
* Implement database file management. 
 
#### On database file management...

Using the local file system to manage a file state is problematic. Using a remote bucket service is more so. Using the host to store files doesn't allow for multiple hosts to access the files (like a web farm). Once files are accessible by multiple hosts code to lock files becomes nearly useless and reactive error handling will have to decide if a file is free for modification.

#### Instead...

File state should be handled by a database. This is true if the files are stored in a network share or a remote bucket.

Files will be stored with a non-human readable hashkey as its name and path (aa/bb/cc/dd/00/aabbccdd00.file). The hash key will be derived from its content bytes. This allows multiple uploads of the same file with the same or different names to consume the same space (deduplication).

The database will maintain the original file information (filename, dates, state, hashkey) in addition to all file hashes (hashkey, size, mimetype). The hash is all that is required to locate the file.

#### Uploads
As files are uploaded the hash is created. The content will be stored in the file store or discarded in the case of a repeat upload then a new reference record will be added to the database.

#### Deletes
Deletes will mark files as deleted (soft delete) in the database. After a delete an async nanny process should query the database for soft deleted file references and hard delete them. If the reference is the last reference to the file, the file will also be deleted.

#### Downloads
The filename will be looked up in the database giving us the hashkey and relevant file info. The content can then be streamed back to the user with the correct filename. Soft deleted files will return 404.

This architecture would ensure that:
* No two processes are trying to modify the same file. 
* Any number of hosts can interact with the same file.
* No file is stored multiple times under different names.
* File metadata can now be retrieved without accessing the remote store.
* File metadata is quickly searchable.
* The number of files stored in any given directory tree is 1.

### Installation

Simply pull and run. No configuration is required.

### Built With
* .Net 5
* .Net Core 3.1
* C# 8
* xUnit
* MimeTypeMapOfficial
