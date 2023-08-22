using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

// Output the build size or a failure depending on BuildPlayer.

public class jenkins : MonoBehaviour
{
    [MenuItem("Build/Build AOS")]
    public static void MyBuild_AOS()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Viewer.unity" };
        buildPlayerOptions.locationPathName = $"Builds/Android/";
        buildPlayerOptions.target = BuildTarget.Android;

        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }

        
    }


    [MenuItem("Build/Build IOS")]
    public static void MyBuild_IOS()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Viewer.unity" };
        buildPlayerOptions.locationPathName = $"Builds/IOS/";
        buildPlayerOptions.target = BuildTarget.iOS;

        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }

    }

    public static void MyBuild_AOS2()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Viewer.unity" };
        buildPlayerOptions.locationPathName = $"Builds/Android/";
        buildPlayerOptions.target = BuildTarget.Android;

        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }

    }


}