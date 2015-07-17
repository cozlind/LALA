using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class AStarFinder
{

    public class Node
    {
        public int x, y;
        public Node parent;
        public int G;
        public int H;
        public int F;
        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public bool equals(int x, int y)
        {
            return this.x == x && this.y == y;
        }
    }
    public List<Node> OpenList = new List<AStarFinder.Node>();
    public List<Node> CloseList = new List<AStarFinder.Node>();

    private int[] XMove = { 0, -1, 0, 1 };
    private int[] YMove = { -1, 0, 1, 0 };

    public List<Node> find(int[,] map, int sx, int sy, int ex, int ey)
    {
        Node start = new Node(sx, sy);
        Node end = new Node(ex, ey);
        Node node = start;
        CloseList.Add(node);
        bool flag = true;
        while (flag)
        {
            for (int i = 0; i < 4; i++)
            {
                int fx = node.x + XMove[i];
                int fy = node.y + YMove[i];
                if (fx >= map.GetLength(0) || fy >= map.GetLength(1) || fx < 0 || fy < 0)
                {
                    continue;
                }
                if (end.equals(fx, fy) && map[fx, fy] >= 0 && (Math.Abs(map[fx, fy] - map[node.x, node.y]) <= 1 || getPass(map, node.x, node.y, fx, fy)))
                {
                    end.parent = node;
                    flag = false;
                    break;
                }
                if (containClose(fx, fy))
                {
                    continue;
                }
                if (containOpen(fx, fy))
                {
                    Node node3 = getOpen(fx, fy);
                    if (node.G + 1 < node3.G)
                    {
                        node3.parent = node;
                        node3.G = node.G + 1;
                        node3.F = node3.G + node3.H;
                    }
                    continue;
                }
                if (map[fx, fy] >= 0 && (Math.Abs(map[fx, fy] - map[node.x, node.y]) <= 1 || getPass(map,node.x, node.y, fx, fy)))
                {
                    Node node2 = new Node(fx, fy);
                    node2.parent = node;
                    node2.G = node.G + 1;
                    node2.H = Math.Abs(ex - fx + ey - fy);
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
        List<Node> Path = new List<AStarFinder.Node>();
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
    public bool containOpen(int x, int y)
    {
        foreach (var item in OpenList)
        {
            if (item.equals(x, y))
            {
                return true;
            }
        }
        return false;
    }
    public bool containClose(int x, int y)
    {
        foreach (var item in CloseList)
        {
            if (item.equals(x, y))
            {
                return true;
            }
        }
        return false;
    }
    public Node getOpen(int x, int y)
    {
        foreach (var item in OpenList)
        {
            if (item.equals(x, y))
            {
                return item;
            }
        }
        return null;
    }
    public bool getPass(int[,] map,int x1, int y1, int x2, int y2)
    {
        //if (Math.Abs(map[x2, y2] - map[x1, y1]) <= 2 && GlobalController.jumpBlock[x1, y1] > 0)
        //{
        //    return true;
        //}
        //return false;
        return true;
    }
}
