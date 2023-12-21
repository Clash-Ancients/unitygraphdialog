using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
{
    public DialogGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
    }
}
