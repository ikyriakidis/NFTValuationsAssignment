# NFTValuations Assignment

## Prerequisites
- Download and install [.NET Core 3.1 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- infura.io valid api token. You can obtain such a token for free from [infura.io](https://infura.io/)
- Edit appsettings file and add your infura.io api token - Both for application and test project

## Build
```sh
dotnet build
```
## Run
```sh
dotnet run "0x1a92f7381b9f03921564a437210bb9396471050c, 0" "0xec9c519d49856fd2f8133a0741b4dbe002ce211b, 30"
```

### Application Settings file example

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "Serilog": {
    "MinimumLevel": "Debug"
  },

  "AppSettings": {
    "InfuraIoToken": [ADD_YOUR_KEY_FROM_INFURA_IO]
  }
}
```

## Test

```sh
dotnet test
```

### Test Settings file example

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "Serilog": {
    "MinimumLevel": "Debug"
  },

  "AppSettings": {
    "InfuraIoToken": [ADD_YOUR_KEY_FROM_INFURA_IO]
  }
}
```