using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<DialogueNodeData> ListDialogueNodeData = new List<DialogueNodeData>();
    public List<NodeLinkData> ListNodeLinkData = new List<NodeLinkData>();
    
    
}

