# Plastrd 

Web Application built with ASP .Net 2.0 MVC

## Resources used :books:
1. [OWIN Authentication for User & Role Management](https://blogs.msdn.microsoft.com/webdev/2013/07/03/understanding-owin-forms-authentication-in-mvc-5/)
2. [Hangfire to schedule background tasks](https://www.hangfire.io/s)
3. [Hasseware.TheMovieDB](https://www.nuget.org/packages/Hasseware.TheMovieDB/)
4. [NLog](https://www.nuget.org/packages/Nlog)

## Get started

In order to sucessfully run the web application, we need to have the following resources available when the app runs for the first time:

1. **AppSettings.config**: Contains passwords, client ids & client secrets for third party authentication and api secrets.

```xml
<?xml version="1.0" encoding="utf-8"?>
<appSettings >
  <!--Authentication-->
  <add key="MicrosoftClientId" value="" />
  <add key="MicrosoftClientSecret" value="" />

  <add key="TwitterClientId" value="" />
  <add key="TwitterClientSecret" value="" />
  
  <add key="FacebookClientId" value=""/>
  <add key="FacebookClientSecret" value=""/>

  <add key="GoogleClientId" value=""/>
  <add key="GoogleClientSecret" value=""/>

  <!--TMDB Service-->
  <add key="TMDBApiKey" value=""/>
  <add key="TMDBApiLanguage" value="en-US" />
</appSettings>
```

2. **ConnectionStrings.config**: contains the database configuration for the web application and for hangfire, so there should 2 separate connection strings
```xml
<?xml version="1.0" encoding="utf-8"?>
<connectionStrings>
    <add name="DefaultConnection" 
        connectionString="Server=localhost;Database=xxx;Uid=xxx;Pwd=xxx;" 
        providerName="MySql.Data.MySqlClient" />
    <add name="DefaultHangfireConnection"
      connectionString="Server=localhost;Database=xxx;Uid=xx;Pwd=xxx;"
      providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

3. **Nlog.config**: Provides the logging configuration for the web app & hangfire.
```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Trace"
      internalLogFile="App_Data\logs\internal-log.txt">
  <targets>
    <target xsi:type="File" 
            name="file" 
            fileName="${basedir}/App_Data/logs/${logger}/${shortdate}.log"
            layout="${longdate} | ${level:uppercase=true:padding=6} | ${threadid:padding=8} | ${message} ${exception:format=tostring}" />
  </targets>
  <rules>
    <logger name="*" minlevel="Trace" writeTo="file" />
  </rules>
</nlog>
```
## Generate Package for deployment

Take a look at the publish profile located at `Properties\PublishProfiles\Local - Web Deploy.pubxml.user`. This publish profiles will copy 

1. `Assets`: contains css, js, fonts and every asset the web application might need.
2. `App_Data\Provisions`: You should place a copy of **AppSettings.config**, **ConnectionStrings.config** and **NLog.config**, and mofify accordingly for the target destinationation.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <WebPublishMethod>FileSystem</WebPublishMethod>
    <PublishProvider>FileSystem</PublishProvider>
    <LastUsedBuildConfiguration>Release</LastUsedBuildConfiguration>
    <LastUsedPlatform>Any CPU</LastUsedPlatform>
    <SiteUrlToLaunchAfterPublish />
    <LaunchSiteAfterPublish>True</LaunchSiteAfterPublish>
    <ExcludeApp_Data>True</ExcludeApp_Data>
    <publishUrl>..\Release</publishUrl>
    <DeleteExistingFiles>True</DeleteExistingFiles>
  </PropertyGroup>
  <Target Name="CustomCollectFiles">
    <ItemGroup>
      <_CustomFiles Include="Assets\**\*" />
      <FilesForPackagingFromProject Include="%(_CustomFiles.Identity)">
        <DestinationRelativePath>Assets\%(RecursiveDir)%(Filename)%(Extension)</DestinationRelativePath>
      </FilesForPackagingFromProject>
    </ItemGroup>
    <ItemGroup>
      <_CustomFiles Include="App_Data\Provisions\**\*" />
      <FilesForPackagingFromProject Include="%(_CustomFiles.Identity)">
        <DestinationRelativePath>%(RecursiveDir)%(Filename)%(Extension)</DestinationRelativePath>
      </FilesForPackagingFromProject>
    </ItemGroup>
  </Target>
  <PropertyGroup>
    <CopyAllFilesToSingleFolderForPackageDependsOn>CustomCollectFiles;</CopyAllFilesToSingleFolderForPackageDependsOn>
    <CopyAllFilesToSingleFolderForMsdeployDependsOn>CustomCollectFiles;</CopyAllFilesToSingleFolderForMsdeployDependsOn>
  </PropertyGroup>
</Project>
```

## Deploying to Azure

From Visual Studio you can create Take a look to the publish profiles to deploy Azure. When you are done creating this profile, please open the file located in `Properties\PublishProfiles\<NAME OF PROFILE>.pubxml.user`, and add the `Target` & `PropertyGroup` right at the end before the closing `Project` tag.


```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  .
  .
  .
  <Target Name="CustomCollectFiles">
    <ItemGroup>
      <_CustomFiles Include="Assets\**\*" />
      <FilesForPackagingFromProject Include="%(_CustomFiles.Identity)">
        <DestinationRelativePath>Assets\%(RecursiveDir)%(Filename)%(Extension)</DestinationRelativePath>
      </FilesForPackagingFromProject>
    </ItemGroup>
    <ItemGroup>
      <_CustomFiles Include="App_Data\Provisions\**\*" />
      <FilesForPackagingFromProject Include="%(_CustomFiles.Identity)">
        <DestinationRelativePath>%(RecursiveDir)%(Filename)%(Extension)</DestinationRelativePath>
      </FilesForPackagingFromProject>
    </ItemGroup>
  </Target>
  <PropertyGroup>
    <CopyAllFilesToSingleFolderForPackageDependsOn>CustomCollectFiles;</CopyAllFilesToSingleFolderForPackageDependsOn>
    <CopyAllFilesToSingleFolderForMsdeployDependsOn>CustomCollectFiles;</CopyAllFilesToSingleFolderForMsdeployDependsOn>
  </PropertyGroup>
</Project>
```

