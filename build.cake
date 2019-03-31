#tool nuget:?package=OpenCover&version=4.7.922
#tool nuget:?package=codecov&version=1.3.0
#tool nuget:?package=GitVersion.CommandLine&version=4.0.0
#addin nuget:?package=Cake.Codecov&version=0.5.0

var target = Argument("target", "Build");
var configuration = Argument("Configuration", "Debug");

GitVersion version;

Task("CI")
    .IsDependentOn("Pack")
    .IsDependentOn("Codecov").Does(() => {});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() => {
         var settings = new DotNetCorePackSettings {
            MSBuildSettings = new DotNetCoreMSBuildSettings(),
            Configuration = "Release",
            NoBuild = true,
            OutputDirectory = "./nugets/"
        };

        settings.MSBuildSettings.Properties["Version"] = new [] { version.NuGetVersion };

        DotNetCorePack("./src/*", settings);
    });

Task("GitVersion")
    .Does(() => {
        version = GitVersion(new GitVersionSettings {
            UpdateAssemblyInfo = true,
        });

        if (BuildSystem.IsLocalBuild == false) 
        {
            GitVersion(new GitVersionSettings {
                OutputType = GitVersionOutput.BuildServer
            });
        }
    });

Task("Build")
    .IsDependentOn("GitVersion")
    .Does(() => {
        DotNetCoreBuild("Nancy.Rdf.sln", new DotNetCoreBuildSettings {
            Configuration = configuration
        });
    });

Task("Codecov")
    .IsDependentOn("Test")
    .Does(() => {
        Codecov("opencover.xml");
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        var openCoverSettings = new OpenCoverSettings
        {
            OldStyle = true,
            MergeOutput = true,
            MergeByHash = true,
            Register = "user",
            ReturnTargetCodeOffset = 0
        }
        .WithFilter("+[nancy.rdf]*");

        bool success = true;
        foreach(var projectFile in new [] { "nancy.rdf.tests.csproj" }.SelectMany(f => GetFiles($"**\\{f}")))
        {
            try
            {
                openCoverSettings.WorkingDirectory = projectFile.GetDirectory();

                OpenCover(context => {
                        context.DotNetCoreTool(
                          projectPath: projectFile.FullPath,
                          command: "xunit", 
                          arguments: $"-noshadow");
                    },
                    "opencover.xml",
                    openCoverSettings);
            }
            catch(Exception ex)
            {
                success = false;
                Error("There was an error while running the tests", ex);
            }
        }
 
        if(success == false)
        {
            throw new CakeException("There was an error while running the tests");
        }
    });

RunTarget(target);
