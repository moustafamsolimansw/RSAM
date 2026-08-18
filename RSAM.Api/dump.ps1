$assembly = [System.Reflection.Assembly]::LoadFrom('E:\Projects\RSAM\RSAM.Api\bin\Debug\net10.0\Swashbuckle.AspNetCore.SwaggerGen.dll')
$type = $assembly.GetType('Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext')
$type.GetProperties() | Select-Object Name, PropertyType
