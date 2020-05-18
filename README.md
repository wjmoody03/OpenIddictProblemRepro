# **Solution** Structure Info: 

There are 3 projects in the solution: Two ASP.NET Core projects and one class library. The 2 ASP.NET core projects are both set as startup projects. The class library just stores the shared certificates for encrypting/decrypting access tokens. 

**IdentityService.Server** : This is the OpenIddict server that issues a token via the password flow

**IdentityService.TestApp**: This mimics a client application attempting to validate the token issued by the server. 

**IdentityService.Shared**: Stores the certificates shared by Server/TestApp for encrypting/decrypting the token. 



## Setup Data: 

The server project uses a SQL database for storing ASP.NET Identity users. The connection string is hardcoded in the Startup file of the Server project. There's a ddl setup script in the root folder, or migrations stored in the project if you prefer. The test app depends on user being added to the database. Navigate to the Register endpoint of the UserController in the Server project to create this test user in the database. 



## Repro Steps

Once the database has been created and the connection string updated, run the solution. The home page of the TestApp project lets you enter a url/username/password for which to retrieve a token from the Server. 

After retrieving the token, it tries to make two different authenticated API requests using the access_token it just received. One request uses the standard bearer authentication in .NET and seems to work. The other uses OpenIddict validation and does not work. Both authentication methods use the same TokenValidationParameters object.






