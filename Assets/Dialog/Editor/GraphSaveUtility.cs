using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSaveUtility
{
   private DialogGraphView mTargetGraphView;

   public static GraphSaveUtility GetInstance(DialogGraphView _graphView)
   {
      return new GraphSaveUtility
      {
         mTargetGraphView = _graphView,
      };
   }

   public void SaveGraph(string fileName)
   {
      
   }

   public void LoadGraph(string fileName)
   {
      
   }
   
}
