[EF]
dotnet tool install --global dotnet-ef
dotnet ef migrations add Initialize
dotnet ef database update

<PreserveCompilationContext>true</PreserveCompilationContext>
<CopyRefAssembliesToPublishDirectory>true</CopyRefAssembliesToPublishDirectory>