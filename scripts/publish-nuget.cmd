for /r ..\src %%p in (*.csproj) do dotnet pack "%%p" -o C:\NuGet\ -c Debug