/*using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class handGenerator
{
    [MenuItem("MyGenerator/CreateScriptableObject")]
    private static void CreateScriptableObject()
    {
        var obj = ScriptableObject.CreateInstance<HandData>();
        var path = "Assets/Resources";
        if (!Directory.Exists(path)){
            Directory.CreateDirectory(path);
        }
        for(int i=0;i<25;i++){
            var fileName = $"{TypeNameToString(obj.GetType().ToString())}.asset";
            AssetDatabase.CreateAsset(obj, Path.Combine(path, fileName));
        }        
    }
    
    private static string TypeNameToString(string type)
    {
        var typeParts = type.Split(new char[] { '.' });
        if (!typeParts.Any())
            return string.Empty;

        var words = Regex.Matches(typeParts.Last(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
            .OfType<Match>()
            .Select(match => match.Value)
            .ToArray();
        return string.Join(" ", words);
    }
}*/