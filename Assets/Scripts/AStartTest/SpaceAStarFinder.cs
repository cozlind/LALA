using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class SpaceAStarFinder
{
    public class Node
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
    }
    public List<Node> OpenList = new List<SpaceAStarFinder.Node>();
    public List<Node> CloseList = new List<SpaceAStarFinder.Node>();

    private int[] XMove = { 0, -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1 };
    private int[] ZMove = { -1, 0, 1, 0, -1, 0, 1, 0, -1, 0, 1, 0 };
    private int[] YMove = { -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1 };

    public List<Node> find(int[, ,] map, int sx, int sy, int sz, int ex, int ey, int ez)

    {
        Node start = new Node(sx, sy, sz);
        Node end = new Node(ex, ey, ez);
        Node node = start;
        CloseList.Add(node);
        bool flag = true;
        while (flag)
        {
            for (int i = 0; i < XMove.Count(); i++)
            {
                int fx = node.x + XMove[i];
                int fy = node.y + YMove[i];
                int fz = node.z + ZMove[i];
                if (fx >= map.GetLength(0) || fy >= map.GetLength(1) || fz >= map.GetLength(2) || fx < 0 || fy < 0 || fz < 0 || map[fx, fy, fz]<=0)
                {
                    continue;
                }
                if (end.equals(fx, fy, fz)  && getPass(map, node.x, node.y, node.z, fx, fy, fz))
                {
                    end.parent = node;
                    flag = false;
                    break;
                }
                if (containClose(fx, fy, fz))
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
                if ( getPass(map, node.x, node.y, node.z, fx, fy, fz))
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
    public bool getPass(int[, ,] map, int x1, int y1, int z1, int x2, int y2, int z2)
    {
        //if (Math.Abs(z2-z1) <= 2 && GlobalController.jumpBlock[x1, y1,z2] > 0)
        //{
        //    return true;
        //}
        //return false;
        int tallX = y1 > y2 ? x1 : x2;
        int tallY = y1 > y2 ? y1 : y2;
        int tallZ = y1 > y2 ? z1 : z2;
        int shortX = y1 > y2 ? x2 : x1;
        int shortY = y1 > y2 ? y2 : y1;
        int shortZ = y1 > y2 ? z2 : z1;

        if (tallY + 1 < map.GetLength(1))
        {
            if (map[shortX, tallY + 1, shortZ] != 0)
            {
                return false;
            }
        }
        return true;
    }
}
