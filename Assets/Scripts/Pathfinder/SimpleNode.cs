using System.Collections.Generic;

public class SimpleNode {
    public readonly int3 pos;

    public List<SimpleEdge> outgoingEdges;

    public SimpleNode(int3 pos) {
        this.pos = pos;
        outgoingEdges = new List<SimpleEdge>();
    }
}