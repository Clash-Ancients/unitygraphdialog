using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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

      foreach (var VARIABLE in Nodes.Where(node => !node.EntryPoint))
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
      
   }

   void ConnectNodes()
   {
      
   }
   
   
}
