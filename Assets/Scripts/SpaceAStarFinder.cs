using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class SpaceAStarFinder
{
    public class Node : ICloneable
    {
        public int x, y, z;
        public Node parent;
        public int G;
        public int H;
        public int F;
        public Node(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public bool equals(int x, int y, int z)
        {
            return this.x == x && this.y == y && this.z == z;
        }
        public bool between(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            if (this.x == x2 && this.y == y2 && this.z == z2)
            {
                return true;
            }
            if (this.x == x1 && this.y == y1 && this.z == z1)
            {
                return true;
            }
            if (x1 == x2 && y1 == y2)
            {
                int b = z1 >= z2 ? z1 : z2;
                int s = z1 >= z2 ? z2 : z1;
                return (this.x == x1 && this.y == y1 && this.z <= b && this.z >= s);
            }
            if (x1 == x2 && z1 == z2)
            {
                int b = y1 >= y2 ? y1 : y2;
                int s = y1 >= y2 ? y2 : y1;
                return (this.x == x1 && this.z == z1 && this.y <= b && this.y >= s);
            }
            if (y1 == y2 && z1 == z2)
            {
                int b = x1 >= x2 ? x1 : x2;
                int s = x1 >= x2 ? x2 : x1;
                return (this.y == y1 && this.z == z1 && this.x <= b && this.x >= s);
            }
            return false;
        }
        public object Clone()
        {
            Node node = new Node(this.x, this.y, this.z);
            node.parent = this.parent;
            node.F = this.F;
            node.G = this.G;
            node.H = this.H;
            return node;
        }
    }
    public List<Node> OpenList = new List<SpaceAStarFinder.Node>();
    public List<Node> CloseList = new List<SpaceAStarFinder.Node>();

    private int[] XMove = { 0, -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1 };
    private int[] ZMove = { -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1, 0 };
    private int[] YMove = { -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, -2, -2, -2, -2, 2, 2, 2, 2 };

    public List<Node> find(int[, ,] map, int sx, int sy, int sz, int ex, int ey, int ez)
    {
        int arrowIndex = 0;
        Node start = new Node(sx, sy, sz);
        Node end = new Node(ex, ey, ez);
        Node node = start;
        Node previous = start;
        CloseList.Add(node);
        bool flag = true;
        while (flag)
        {
            for (int i = 0; i < XMove.Count(); i++)
            {
                int fx = node.x + XMove[i];
                int fy = node.y + YMove[i];
                int fz = node.z + ZMove[i];
                //箭头方块
                if (GlobalController.typeMap[node.x, node.y, node.z].StartsWith("Arrow"))
                {
                    if (arrowIndex >= 3)
                    {
                        break;
                    }
                    switch (arrowIndex)
                    {
                        case 0: fy = node.y - 1; break;
                        case 1: fy = node.y; break;
                        case 2: fy = node.y + 1; break;
                    }
                    arrowIndex++;
                    if (GlobalController.typeMap[node.x, node.y, node.z].EndsWith("-x"))
                    {
                        fx = node.x - 1;
                        fz = node.z;
                    }
                    if (GlobalController.typeMap[node.x, node.y, node.z].EndsWith("+x"))
                    {
                        fx = node.x + 1;
                        fz = node.z;
                    }
                    if (GlobalController.typeMap[node.x, node.y, node.z].EndsWith("+z"))
                    {
                        fx = node.x;
                        fz = node.z + 1;
                    }
                    if (GlobalController.typeMap[node.x, node.y, node.z].EndsWith("-z"))
                    {
                        fx = node.x;
                        fz = node.z - 1;
                    }
                }
                //移动方块
                if (GlobalController.typeMap[node.x, node.y, node.z].StartsWith("Move") && !haveMoved(map, previous.x, previous.y, previous.z, node.x, node.y, node.z))
                {
                    if (GlobalController.typeMap[node.x, node.y, node.z].EndsWith("x"))
                    {
                        fy = node.y;
                        fx = int.Parse(GlobalController.typeMap[node.x, node.y, node.z].Split(":".ToCharArray())[1]);
                        fz = node.z;
                    }
                    if (GlobalController.typeMap[node.x, node.y, node.z].EndsWith("y"))
                    {
                        fx = node.x;
                        fy = int.Parse(GlobalController.typeMap[node.x, node.y, node.z].Split(":".ToCharArray())[1]);
                        fz = node.z;
                    }
                    if (GlobalController.typeMap[node.x, node.y, node.z].EndsWith("z"))
                    {
                        fx = node.x;
                        fy = node.y;
                        fz = int.Parse(GlobalController.typeMap[node.x, node.y, node.z].Split(":".ToCharArray())[1]);
                    }
                    i = XMove.Count();
                }
                if (fx >= map.GetLength(0) || fy >= map.GetLength(1) || fz >= map.GetLength(2) || fx < 0 || fy < 0 || fz < 0
                    || getStop(map, previous.x, previous.y, previous.z, node.x, node.y, node.z, fx, fy, fz))
                {
                    continue;
                }
                if (end.between(node.x, node.y, node.z, fx, fy, fz) && getPass(map, node.x, node.y, node.z, fx, fy, fz))
                {
                    end.parent = node;
                    flag = false;
                    break;
                }
                if (containClose(fx, fy, fz) && !haveMoved(map, node.x, node.y, node.z, fx, fy, fz))
                {
                    continue;
                }
                if (containOpen(fx, fy, fz))
                {
                    Node node3 = getOpen(fx, fy, fz);
                    if (node.G + 1 < node3.G)
                    {
                        node3.parent = node;
                        node3.G = node.G + 1;
                        node3.F = node3.G + node3.H;
                    }
                    continue;
                }
                if (getPass(map, node.x, node.y, node.z, fx, fy, fz))
                {
                    Node node2 = new Node(fx, fy, fz);
                    node2.parent = node;
                    node2.G = node.G + 1;
                    node2.H = Math.Abs(ex - fx + ey - fy + ez - fz);
                    node2.F = node2.G + node2.H;
                    OpenList.Add(node2);
                }
            }
            if (flag == false)
            {
                break;
            }
            if (OpenList.Count == 0)
            {
                return null;
            }
            previous = node;
            node = MinF(OpenList);
            OpenList.Remove(node);
            CloseList.Add(node);
        }
        List<Node> Path = new List<SpaceAStarFinder.Node>();
        node = end;
        while (node != null)
        {
            Path.Add(node);
            node = node.parent;
        }
        Path.Reverse();
        return Path;
    }
    public Node MinF(List<Node> list)
    {
        Node min = list[0];
        foreach (var item in list)
        {
            if (item.F <= min.F)
            {
                min = item;
            }
        }
        return min;
    }
    public bool containOpen(int x, int y, int z)
    {
        foreach (var item in OpenList)
        {
            if (item.equals(x, y, z))
            {
                return true;
            }
        }
        return false;
    }
    public bool containClose(int x, int y, int z)
    {
        foreach (var item in CloseList)
        {
            if (item.equals(x, y, z))
            {
                return true;
            }
        }
        return false;
    }
    public Node getOpen(int x, int y, int z)
    {
        foreach (var item in OpenList)
        {
            if (item.equals(x, y, z))
            {
                return item;
            }
        }
        return null;
    }
    public Node getClose(int x, int y, int z)
    {
        foreach (var item in CloseList)
        {
            if (item.equals(x, y, z))
            {
                return item;
            }
        }
        return null;
    }
    public bool getPass(int[, ,] map, int x1, int y1, int z1, int x2, int y2, int z2)
    {
        //跳跃限制
        int tallX = y1 > y2 ? x1 : x2;
        int tallY = y1 > y2 ? y1 : y2;
        int tallZ = y1 > y2 ? z1 : z2;
        int shortX = y1 > y2 ? x2 : x1;
        int shortY = y1 > y2 ? y2 : y1;
        int shortZ = y1 > y2 ? z2 : z1;

        if (tallY + 1 < map.GetLength(1) && tallY != shortY)
        {
            if (map[shortX, tallY + 1, shortZ] != 0)
            {
                return false;
            }
        }
        if (tallY - shortY <= 2 && GlobalController.typeMap[x1, y1, z1].Equals("Jump"))
        {
            return true;
        }
        if (tallY - shortY <= 1)
        {
            return true;
        }
        if (tallX == shortX && tallZ == shortZ && GlobalController.typeMap[x1, y1, z1].StartsWith("Move"))
        {
            return true;
        }
        return false;
    }
    public bool getStop(int[, ,] map, int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3)
    {
        if (GlobalController.typeMap[x2, y2, z2].StartsWith("Move") && !haveMoved(map, x1, y1, z1, x2, y2, z2))
        {
            //Move方块的移动目的点是空的
            if (map[x3, y3, z3] == 0)
            {
                return false;
            }
            //Move方块移动后的始发点是空的，作为另一个MoveBlock的目的点
            if (GlobalController.typeMap[x3, y3, z3].StartsWith("Move") && haveMoved(map, x2, y2, z2, x3, y3, z3))
            {
                return false;
            }
            return true;
        }

        //没有可用方块
        if (map[x3, y3, z3] <= 0)
        {
            return true;
        }
        return false;
    }
    //x2,y2,z2处的可移动方块已经被移开
    public bool haveMoved(int[, ,] map, int x1, int y1, int z1, int x2, int y2, int z2)
    {
        Node node = new Node(x1, y1, z1);
        if (getClose(x1, y1, z1) != null)
        {
            node = getClose(x1, y1, z1).Clone() as Node;
        }
        else
        {
            return false;
        }
        while (node.parent != null)
        {
            if (node.parent.equals(x2, y2, z2))
            {
                return true;
            }
            node = node.parent.Clone() as Node;
        }
        return false;
    }
    //public bool haveMoved(int[, ,] map,Node preNode)
    //{
    //    Node node = getClose(preNode.x, preNode.y, preNode.z) as Node;
    //    while (node.parent != null)
    //    {
    //        if (node.parent.equals(preNode.x, preNode.y, preNode.z))
    //        {
    //            return true;
    //        }
    //        node = new Node(preNode.parent.x, preNode.parent.y, preNode.parent.z);
    //        node.parent = preNode.parent;
    //    }
    //    return false;
    //}
}
