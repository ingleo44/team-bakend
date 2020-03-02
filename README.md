# Blog API
The api was a single api just to show some practices during the development process so I just created 2 entities the first is users and the another one is blogs.
# Business Rules
- Every blog has a user associated
- You can only modify the blogs associated to your users.
- You dont need to be authenticated to create users.
# Run the application
You should download the app open it on visual studio and run the solution, I suggest to user the webapi launch configuration.
to access to the app you can create a new user using the end /api/users serviceand the authenticate using the login service.
Features implemented.
JWT token authentication.
Dependency injection
Repository Design Pattern
Entity Framework core

# Database
The database is stored in memory, so every time the application runs it recreates the database.

# Unit testing
I used the nunit framework for making the unit testing


