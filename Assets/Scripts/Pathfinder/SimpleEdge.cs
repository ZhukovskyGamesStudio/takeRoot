using System.Collections.Generic;

public class SimpleEdge {
    public readonly SimpleNode from;
    public readonly SimpleNode destinationNode;

    public int hierrarchyLevel;
    public float weight;

    public SimpleEdge(SimpleNode from, SimpleNode destination) {
        this.from = from;
        destinationNode = destination;
    }

    public List<int3> GetPath() {
        return new List<int3> { from.pos, destinationNode.pos };
    }
}