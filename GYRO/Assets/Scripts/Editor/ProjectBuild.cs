using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ProjectBuild : UnityEditor.Editor
    {
        [MenuItem("Build/BuildForAndroid %e")]
        public static void BuildForAndroid()
        {
            string path = @"D:\workspace\Project\UnityGYRO\Android.apk";
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
        }

        //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
        static string[] GetBuildScenes()
        {
            List<string> names = new List<string>();
            foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
            {
                if (e == null)
                    continue;
                if (e.enabled)
                    names.Add(e.path);
            }

            return names.ToArray();
        }
    }
}