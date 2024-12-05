using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildComplete : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPostprocessBuild(BuildReport report)
    {
        BuildPlayerOptions build = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(new BuildPlayerOptions());
        Debug.Log(Application.dataPath);
        Debug.Log(Directory.GetParent(build.locationPathName).FullName);

        string dest = Directory.GetParent(build.locationPathName).FullName + "/Music";
        string par = Directory.GetParent(Application.dataPath).FullName + "/Music";
        var dir = new DirectoryInfo(par);
        if (!dir.Exists)
            throw new DirectoryNotFoundException(par);

        DirectoryInfo[] dirs = dir.GetDirectories();
        
 
        Directory.CreateDirectory(dest);

        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(dest, file.Name);
            file.CopyTo(targetFilePath);
        }
    }
}
