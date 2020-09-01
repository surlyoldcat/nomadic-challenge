# nomadic-challenge

## Built With
- ASP.Net Core 3 MVC
- EF Core
- ASP.Net Core Identity
- MVCGrid 6
- SQL Express
- ASP.Net Core's built-in DI service

## Database Setup
### The EF Way
1. Edit SqlScripts/CreateDatabase.sql to include a password (or set it after running the script.)
2. Run CreateDatabase.sql
3. Update connection string in appsettings.json.
4. Build the solution.
5. From Package Manager Console in VS, run the Update-Database command.

### The T-SQL Way
1. Edit SqlScripts/CreateDatabase.sql to include password.
2. Run CreateDatabase.sql.
3. Run SqlScripts/Tables.sql.
4. Update connection string in appsettings.json.
5. Hope that EF doesn't get offended.

## Notes
- The user registration process is the very most basic available. There is no email confirmation or other advanced functionality. It's easily available in Identity, however.
- There are no limits/rules around image file sizes. In real life, image upload would be managed in much stricter fashion, not just using multipart form uploads.
- There are a couple unit tests, not intended to be exhaustive. Just to illustrate that the back-end components are testable.
- There is some rudimentary caching of images in the back-end. For a production app, one would never want to cache stuff in memory, I am using it here just for educational purposes.
- Photos are stored separately from actual 'entities' and isolated into a separate API, to kinda emulate how I would prefer to handle blob storage in a production app (using a service like Blob Storage or S3 to store binary content, instead of relational DB.)
- I did not put a lot of error handling/logging in. Normally, I would use something like NLog and customized error handling, but it's a lot more work to do that with an MVC app than with a headless API.
