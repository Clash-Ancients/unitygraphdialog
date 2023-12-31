using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = UnityEditor.Graphs.Node;

public class GraphSaveUtility
{
   private DialogGraphView mTargetGraphView;

   private List<Edge> Edges => mTargetGraphView.edges.ToList();
   private List<DialogNode> Nodes => mTargetGraphView.nodes.ToList().Cast<DialogNode>().ToList();

   private DialogueContainer mCacheDialogContainer;
   
   
   public static GraphSaveUtility GetInstance(DialogGraphView _graphView)
   {
      return new GraphSaveUtility
      {
         mTargetGraphView = _graphView,
      };
   }

   public void SaveGraph(string fileName)
   {
      if(!Edges.Any())
         return;

      var dialogContainer = ScriptableObject.CreateInstance<DialogueContainer>();

      var connectedPorts = Edges.Where(x => x.input.node != null).ToList();

      foreach (var VARIABLE in connectedPorts)
      {
         var outputNode = VARIABLE.output.node as DialogNode;
         var inputNode = VARIABLE.input.node as DialogNode;
          
         dialogContainer.ListNodeLinkData.Add(new NodeLinkData
         {
            BaseNodeGuid = outputNode.GUID,
            PortName = VARIABLE.output.portName,
            TargetNodeGuid = inputNode.GUID,
         });
      }

      //foreach (var VARIABLE in Nodes.Where(node => !node.EntryPoint))
      foreach (var VARIABLE in Nodes)
      {
         dialogContainer.ListDialogueNodeData.Add(new DialogueNodeData
         {
            GUID = VARIABLE.GUID,
            DialogueText = VARIABLE.DialogText,
            Position = VARIABLE.GetPosition().position,
         });
      }

      if (!AssetDatabase.IsValidFolder("Assets/Resources"))
      {
         AssetDatabase.CreateFolder("Assets", "Resources");
      }
      
      AssetDatabase.CreateAsset(dialogContainer, $"Assets/Resources/{fileName}.asset");
      AssetDatabase.SaveAssets();
      
   }

   public void LoadGraph(string fileName)
   { 
      //load asset file content
      mCacheDialogContainer = AssetDatabase.LoadAssetAtPath<DialogueContainer>($"Assets/Resources/{fileName}.asset");

      if (null == mCacheDialogContainer)
      {
         EditorUtility.DisplayDialog("File Not Found", "target file not exist", "OK");
         return;
      }
      
      //clear
      ClearGraph();
      //create nodes
      CreateNodes();
      //connect nodes
      ConnectNodes();
   }

   void ClearGraph() 
   {
      
      Nodes.Find(x => x.EntryPoint).GUID = mCacheDialogContainer.ListDialogueNodeData[0].GUID;

      foreach (var variable in Nodes)
      {
         if(variable.EntryPoint)
            continue;
         Edges.Where(edge => edge.input.node == variable).ToList().ForEach(edge => mTargetGraphView.RemoveElement(edge));
         
         mTargetGraphView.RemoveElement(variable);
      }
      
   }

   void CreateNodes()
   {
      foreach (var variable in mCacheDialogContainer.ListDialogueNodeData.Where(x => !x.DialogueText.Equals("ENTRYPOINT")))
      {
         var tempNode = mTargetGraphView.CreateDialogNode(variable.DialogueText, Vector2.zero);
         tempNode.GUID = variable.GUID;
         tempNode.SetPosition(new Rect(variable.Position, DialogGraphView.DefaultNodeSize));
         mTargetGraphView.AddElement(tempNode);

         var nodePorts = mCacheDialogContainer.ListNodeLinkData.Where(x => x.BaseNodeGuid == variable.GUID).ToList();
         
         nodePorts.ForEach(x => mTargetGraphView.AddChoicePort(tempNode, x.PortName));
      }
   }

   void ConnectNodes()
   {
      for (var i = 0; i < Nodes.Count; i++)
      {
         var connnections = mCacheDialogContainer.ListNodeLinkData.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();

         for (var j = 0; j < connnections.Count; j++)
         {
            //output link to input
            var conn = connnections[j];
            var targetGuid = conn.TargetNodeGuid;
            var targetNode = Nodes.First(x => x.GUID == targetGuid);

            var outputPort = Nodes[i].outputContainer[j].Q<Port>();
            
            var inputPort = (Port)targetNode.inputContainer[0];
            
            LinkNodes(outputPort, inputPort);
         }
         
      }
   }

   void LinkNodes(Port _output, Port _input)
   {
      var tempEdge = new Edge
      {
         output = _output,
         input = _input
      };
      
      tempEdge.input.Connect(tempEdge);
      tempEdge.output.Connect(tempEdge);

      mTargetGraphView.Add(tempEdge);
   }
   
   
}
