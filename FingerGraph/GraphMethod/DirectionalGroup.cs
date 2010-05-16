using System;
using System.Collections.Generic;

namespace ImageProcessing
{

    class DirectionalImage
    {
        public int startline, endline;
        public byte[,] arr;
        public DirectionalImage(int width, int height)
        {
            arr = new byte[width, height];
        }
    }


    public class Point
    {

        public int x
        { get; set; }
        public int y
        { get; set; }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Point()
        {
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }

        public static Point operator /(Point p, double d)
        {
            return new Point((int)Math.Round(p.x / (d)), (int)Math.Round(p.y / d));
        }
    }


    public class DirectionNode
    {
        public List<Point> PointList = new List<Point>(20);
        
        public DirectionNode(byte dir)
        {
            direction = dir;
        }


        public DirectionNode()
        {
        }


        public Point gravitycenter;
        public byte direction;

        public int Weight;


        public void ComputeCenter()
        {
            if (PointList.Count == 0)
            {
                gravitycenter = new Point(-1, -1);
                return;
            }
            Point sum = new Point(0, 0);
            foreach (Point point in PointList)
                sum += point;
            gravitycenter = sum / PointList.Count;
        }

        public void add(Point p)
        {
            PointList.Add(p);
            Weight++;
        }
    }

    public class DirectionGraph
    {
        public List<DirectionNode> Nodes = new List<DirectionNode>(20);
        public int[,] perimeterarray;
        public double[,] weightarray;
        public void add(DirectionNode p)
        {
            Nodes.Add(p);
        }
    }

}
