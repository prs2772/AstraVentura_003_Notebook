# UserSecrets required

## Basic Commands

```bash
# Add user secrets
dotnet user-secrets set "ConnectionStrings:MongoDbConnection" "mongodb://root:secretpassword@localhost:27017"

# List user secrets
dotnet user-secrets list

# Remove user secrets
dotnet user-secrets remove "ConnectionStrings:MongoDbConnection"
```

## AstraVenturaNotebook.Api

```bash
# Add user secrets
dotnet user-secrets set "MongoDbSettings:ConnectionString" "mongodb://${MONGO_ROOT_USERNAME}:${MONGO_ROOT_PASSWORD}@localhost:27017"
dotnet user-secrets set "MongoDbSettings:DatabaseName" "${MONGO_DB}"
dotnet user-secrets set "JwtSettings:Issuer" "${JWT_ISSUER}"
dotnet user-secrets set "JwtSettings:Audience" "${JWT_AUDIENCE}"
dotnet user-secrets set "JwtSettings:SecretKey" "${JWT_SECRET_KEY}"
```


