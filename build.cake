#tool nuget:?package=OpenCover&version=4.7.922
#tool nuget:?package=codecov&version=1.3.0
#tool nuget:?package=GitVersion.CommandLine&version=4.0.0
#addin nuget:?package=Cake.Codecov&version=0.5.0
#tool nuget:?package=JetBrains.dotCover.CommandLineTools&version=2018.3.4
#tool nuget:?package=ReportGenerator&version=4.0.15

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
            Configuration = configuration,
            NoBuild = true,
            OutputDirectory = "./nugets/"
        };

        DotNetCorePack("nancy.rdf.sln", settings);
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
    .Does(() => {
        var settings = new DotNetCoreBuildSettings {
            MSBuildSettings = new DotNetCoreMSBuildSettings(),
            Configuration = configuration
        };

        DotNetCoreBuild("Nancy.Rdf.sln", settings);
    });

Task("Codecov")
    .IsDependentOn("Test")
    .IsDependentOn("GitVersion")
    .Does(() => {
        var buildVersion = string.Format("{0}.build.{1}",
            version.FullSemVer,
            BuildSystem.AppVeyor.Environment.Build.Number
        );
        var settings = new CodecovSettings {
            Files = new[] { "./coverage/cobertura.xml" },
            EnvironmentVariables = new Dictionary<string,string> { { "APPVEYOR_BUILD_VERSION", buildVersion } }
        };
        Codecov(settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotCoverAnalyse(context => {
                context.DotNetCoreTest(GetFiles("src/nancy.rdf.tests/nancy.rdf.tests.csproj").Single().FullPath);
            },
            "./coverage/dotcover.xml",
            new DotCoverAnalyseSettings {
                ReportType = DotCoverReportType.DetailedXML
                }
                .WithFilter("+:Nancy.Rdf")
                .WithFilter("-:Nancy.Rdf.Tests"));
    })
    .Does(() => {
        ReportGenerator("./coverage/dotcover.xml", "./coverage", new ReportGeneratorSettings() {
            ReportTypes = new [] { ReportGeneratorReportType.Cobertura }
        });
    });

RunTarget(target);
