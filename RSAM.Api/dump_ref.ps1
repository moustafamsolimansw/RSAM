$assembly = [System.Reflection.Assembly]::LoadFrom('C:\Users\mo.ahmed\.nuget\packages\microsoft.openapi\2.11.0\lib\net8.0\Microsoft.OpenApi.dll')
$type = $assembly.GetType('Microsoft.OpenApi.Models.OpenApiSecuritySchemeReference')
if ($null -eq $type) { $type = $assembly.GetType('Microsoft.OpenApi.OpenApiSecuritySchemeReference') }
$type.GetConstructors() | ForEach-Object { $_.ToString() }
