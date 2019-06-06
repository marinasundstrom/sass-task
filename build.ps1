#!/usr/bin/env powershell
$ErrorActionPreference = 'Stop'

function exec($_cmd) {
    write-host " > $_cmd $args" -ForegroundColor cyan
    & $_cmd @args
    if ($LASTEXITCODE -ne 0) {
        throw 'Command failed'
    }
}

Remove-Item artifacts/ -Recurse -ErrorAction Ignore
Remove-Item src/SassTask.Compiler/obj/ -Recurse -ErrorAction Ignore
Remove-Item src/SassTask/obj/ -Recurse -ErrorAction Ignore
Remove-Item sample/obj/ -Recurse -ErrorAction Ignore

exec dotnet restore ./src/SassCompile/
exec dotnet pack -c Release ./src/SassTask.Compiler/
exec dotnet restore ./src/SassTask/
exec dotnet pack -c Release ./src/SassTask/
exec dotnet restore ./sample/
exec dotnet msbuild /nologo '/t:Sass' ./sample/
