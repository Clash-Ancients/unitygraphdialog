using System;

//hold data connections between two nodes
[Serializable]
public class NodeLinkData
{
    public string BaseNodeGuid;
    public string PortName;
    public string TargetNodeGuid;
}
