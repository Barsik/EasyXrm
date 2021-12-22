[![Nuget](https://img.shields.io/nuget/v/EasyXrm)](https://www.nuget.org/packages/EasyXrm/)

EasyXrm
============

EasyXrm is a **Microsoft Dynamics CRM/365** framework designed to be easy to development.

`⚠️ Wiki is under constructions`
## Get Started

EasyXrm is available on [NuGet](https://www.nuget.org/packages/EasyXrm). Add the library to your project:
```sh
Install-Package EasyXrm
```
or
```sh
dotnet add package EasyXrm
```
For plugin and workflow assemblies you need to merge EasyXrm with your assemblies. Use [ILMerge](https://github.com/dotnet/ILMerge) for merging multiple .NET assemblies into a single assembly.

If you don't want to use ILMerge you can use a shared project. Add to your solution a shared project. Consume EasyXrm repository to the shared project with command:
 ```sh
git submodule add https://github.com/barsik/easyxrm
```
Include in the shared project EasyXrm folder. Then you can add reference to this shared project from your project. This it!

## Features

* **Extension methods**: Huge pack of extensions for IOrganizationServe, Entity, EntityReference etc
* **ChangeTracker**: Designed for updating only modified fields. The main idea was taken from [Xrm-Update-Context](https://github.com/XRM-OSS/Xrm-Update-Context) by @DigitalFlow, but slightly improved
* **QueryBuilder**: The main purpose is building QueryExpression easy
  *  **Plugin/Workflow base classes**: Proxy classes to work with Execution Context, Tracing Service, Image Collections etc