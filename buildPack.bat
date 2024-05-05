dotnet clean Product.proj
dotnet build Product.proj
dotnet pack --no-restore -o C:/LocalPackages --no-build Product.proj
