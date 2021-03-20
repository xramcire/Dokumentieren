## About The Project
Simple document upload service. 

### Architectural Considerations
* Follow REST standards.
* Include integration tests.
* Thread-safe.
* Handle multiple concurrent requests to the same file.
* Use async where available.
* Use dependency injection where appropriate.
* Support multiple file storage services. 
* Seperate large concerns by project (Web / Services).

### Future Improvements
Using the local file system to manage a file state is problematic. Using a remote bucket service is more so. Using the host to store files doesn't allow for multiple hosts to access the files (like a web farm). Once files are accessible by multiple hosts code to lock files becomes nearly useless and reactive error handling will have to decide if a file is free for modification.

Instead...

File state should be handled by a database. This is true if the files are a network share or a remote bucket.

Files will be stored with a non-human readable hashkey as its name and path. The hash key will be derrived from its content bytes. This allows multiple uploads of the same file with the same or different names to consume the same space (deduplication).

The database will maintain the original file information (filename, dates, state, hashkey) in addition to all file hashes (hashkey, size, mimetype) references to the files.

#### Uploads
As files are uploaded the hash is created. The content will be stored in the file store (or discarded in the case of a repeat upload) then a new reference record will be added to the database.

#### Deletes
Deletes will mark files as deleted (soft delete) in the database. A nanny service should regularly scan the database for soft deleted file references and hard delete them. If the reference is the last reference to the file, the file will then be deleted.

#### Downloads
The filename will be looked up in the database giving us the hashkey and relavent file info. The content can then be streamed back to the user with the shared content and correct filename. Soft deleted files will return 404.

This architecture would ensure that:
* No two processes are trying to modify the same file. 
* Any number of hosts can interact with the same file.
* No file is stored multiple times with a different name.
* File metadata is now searchable without accessing the remote store.

### Installation

Simply pull and run. No configuration is required.

### Built With
* .Net 5
* C# 8
* xUnit
* MimeTypeMapOfficial
