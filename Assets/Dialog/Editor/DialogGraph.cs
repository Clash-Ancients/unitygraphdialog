using UnityEditor;
using UnityEngine;

public class DialogGraph : EditorWindow
{
    [MenuItem("Graph/Dialog Graph")]
    public static void OpenDialogGraphWindow()
    {
        var win = GetWindow<DialogGraph>();
        win.titleContent = new GUIContent("Dialogue Graph");
    }
}
