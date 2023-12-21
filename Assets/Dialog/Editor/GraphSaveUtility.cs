using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphSaveUtility
{
   private DialogGraphView mTargetGraphView;

   private List<Edge> Edges => mTargetGraphView.edges.ToList();
   private List<DialogNode> Nodes => mTargetGraphView.nodes.ToList().Cast<DialogNode>().ToList();
   
   
   
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
      
   }
   
}
