using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
  /// Support plugins are available for:
  ///   - JetBrains ReSharper        https://nuke.build/resharper
  ///   - JetBrains Rider            https://nuke.build/rider
  ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
  ///   - Microsoft VSCode           https://nuke.build/vscode

  public static int Main() => Execute<Build>(x => x.AwsIs4);

  [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
  readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

  [Solution] readonly Solution Solution;

  AbsolutePath Src => RootDirectory / "src";
  AbsolutePath Test => RootDirectory / "test";

  Target Clean => _ => _
      .Executes(() =>
      {
        DotNetClean();

        Src.GlobDirectories("**/bin", "**/obj").ForEach(d =>
          {
            EnsureCleanDirectory(d);
            DeleteDirectory(d);
          });

        Test.GlobDirectories("**/bin", "**/obj").ForEach(d =>
        {
          EnsureCleanDirectory(d);
          DeleteDirectory(d);
        });
      });

  Target AwsIs4 => _ => _
      .DependsOn(Clean)
      .Executes(() =>
      {
        EnsureCleanDirectory("publish");
        DeleteDirectory("publish");
        DeleteFile("is4.zip");

        DotNetPublish(s => s
          .SetProject(Src / "Mesi.Io.IdentityServer4")
          .SetOutput("publish/is4/app")
          .SetConfiguration("Release"));

        CopyFile("docker/Dockerfile-IdentityServer4", "publish/is4/Dockerfile");
        CopyFile("docker/docker-compose.yml", "publish/is4/docker-compose.yml");

        CompressionTasks.Compress("publish", RootDirectory / "is4.zip");

        EnsureCleanDirectory("publish");
        DeleteDirectory("publish");
      });
}
