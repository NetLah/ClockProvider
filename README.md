# NetLah.Extensions.ClockProvider - .NET Library

[NetLah.Extensions.ClockProvider](https://www.nuget.org/packages/NetLah.Extensions.ClockProvider/) is a library support setting ASP.NET Core ClockProvider from configuration.

## NuGet package

[![NuGet](https://img.shields.io/nuget/v/NetLah.Extensions.ClockProvider.svg?style=flat-square&label=nuget&colorB=00b200)](https://www.nuget.org/packages/NetLah.Extensions.ClockProvider/)

## Build Status

[![Build Status](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2FNetLah%2Fhttp-overrides%2Fbadge%3Fref%3Dmain&style=flat)](https://actions-badge.atrox.dev/NetLah/http-overrides/goto?ref=main)

## Getting started

### 1. Add/Update PackageReference to .csproj

```xml
<ItemGroup>
  <PackageReference Include="NetLah.Extensions.ClockProvider" Version="1.0.0" />
</ItemGroup>
```

### 2. Add ClockProvider

```csharp
builder.AddClockProvider(true);
```
