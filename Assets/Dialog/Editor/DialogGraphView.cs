using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
{
    public static readonly Vector2 DefaultNodeSize = new Vector2(200f, 150f);
    
    public DialogGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        
        Insert(0, grid);
        
        grid.StretchToParentSize();
        
        
        AddElement(GenerateEntryPointNode());
    }

    #region create node
    
    //generate entry node
    DialogNode GenerateEntryPointNode()
    {
        var entryNode = new DialogNode
        {
            title = "Entry Node",
            GUID = Guid.NewGuid().ToString(),
            DialogText = "ENTRYPOINT",
            EntryPoint = true,
        };

        var port = GeneratePort(entryNode, Direction.Output);

        port.portName = "Next";

        entryNode.capabilities &= ~Capabilities.Deletable;
        entryNode.capabilities &= ~Capabilities.Movable;
        
        entryNode.outputContainer.Add(port);
        
        entryNode.RefreshExpandedState();

        entryNode.RefreshPorts();
        
        entryNode.SetPosition(new Rect(100f, 100f, 100f, 100f));

        return entryNode;
    }
    
    public void CreateNode(string _nodeName = "DialogNode")
    {
        AddElement(CreateDialogNode(_nodeName));
    }
    
    public DialogNode CreateDialogNode(string _nodeName = "Dialog Node")
    {
        var node = new DialogNode
        {
            title = _nodeName,
            DialogText = _nodeName,
            GUID = Guid.NewGuid().ToString(),
            EntryPoint = false,
        };

        var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
        
        node.inputContainer.Add(inputPort);

        var button = new Button(() => AddChoicePort(node));
        
        button.text = "new choice";
        
        node.titleContainer.Add(button);

        node.styleSheets.Add(Resources.Load<StyleSheet>("DialogNode"));

        var textField = new TextField(string.Empty);

        textField.RegisterValueChangedCallback(x =>
        {
            node.DialogText = x.newValue;
            node.title = x.newValue;
        });
        
        textField.SetValueWithoutNotify(node.title);
        
        node.mainContainer.Add(textField);
        
        node.RefreshExpandedState();

        node.RefreshPorts();
        
        node.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return node;
    }
    
    //generate port
    Port GeneratePort(DialogNode _node, Direction _portDir, Port.Capacity _cap = Port.Capacity.Single)
    {
        return _node.InstantiatePort(Orientation.Horizontal, _portDir, _cap, typeof(float));
    }
    
    //ports: GetCompatiblePorts
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if(startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });


        return compatiblePorts;
    }

    public void AddChoicePort(DialogNode _dialogNode, string _portName = "")
    {
        var genPort = GeneratePort(_dialogNode, Direction.Output);
        //
        // var portLabel = genPort.contentContainer.Q<Label>("type");
        // genPort.contentContainer.Remove(portLabel);
        
      
        
        var count = _dialogNode.outputContainer.Query("connector").ToList().Count;
        
        genPort.portName = string.IsNullOrEmpty(_portName)?$"Choice {count}":_portName;
        
        var textField = new TextField
        {
            name = string.Empty,
            value = genPort.portName,
        };
        
        // textField.style.minWidth = 100;
        // textField.style.maxWidth = 120; 
        textField.RegisterValueChangedCallback(x => genPort.portName = x.newValue);
        var delBtn = new Button(() => RemovePort(_dialogNode, genPort))
        {
            text = "X",
        };
      
//        genPort.contentContainer.Add(new Label("  "));
        genPort.contentContainer.Add(textField);
      
        
        genPort.contentContainer.Add(delBtn);
        _dialogNode.outputContainer.Add(genPort);
        
        _dialogNode.RefreshPorts();
        _dialogNode.RefreshExpandedState();
    }

    void RemovePort(DialogNode _dialogNode, Port _genPort)
    {
       var list = edges.ToList().Where(x => x.output.portName == _genPort.portName && x.output.node == _genPort.node);

       var enumerable = list as Edge[] ?? list.ToArray();
       
       if (enumerable.Any())
       {
           var edge = enumerable.First();
           edge.input.Disconnect(edge);
           RemoveElement(edge);
       }
       
       _dialogNode.outputContainer.Remove(_genPort);
       
       _dialogNode.RefreshPorts();
       _dialogNode.RefreshExpandedState();


    }
    
    #endregion
}
