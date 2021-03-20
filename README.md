## About The Project
<br />
Simple document upload service. 

* Follows REST standards.
* Includes integration tests.
* Thread-safe and can handle multiple concurrent requests to the same file.

### Installation

Simply pull and run. No configuration is required.

### Projects

#### Xramcire.Dokumentieren.Web
.Net 5.0 Web Application.

Supported routes
* OPTIONS:	/documents/
* POST:		/documents/
* PUT:		/documents/documentName
* GET:		/documents/documentName
* DELETE:	/documents/documentName

#### Xramcire.Dokumentieren.Tests
xUnit test library.

#### Xramcire.Dokumentieren.Services
.Net 5.0 class library.
