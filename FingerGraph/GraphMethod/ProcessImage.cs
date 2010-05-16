using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using AForge.Imaging.Filters;

namespace ImageProcessing
{
    abstract class ProcessImage
    {

        private const int WINDOWSIZE = 5;
        private const int DIRECTIONNUM = 4;
        private const int MINNOISECOUNT = 4;

        // Apply filter on the image
        private static void ApplyFilter(IFilter filter, ref Bitmap image)
        {
            if (filter is IFilterInformation)
            {
                IFilterInformation filterInfo = (IFilterInformation)filter;
                if (!filterInfo.FormatTransalations.ContainsKey(image.PixelFormat))
                {
                    if (filterInfo.FormatTransalations.ContainsKey(PixelFormat.Format24bppRgb))
                    {
                        MessageBox.Show("The selected image processing routine may be applied to color image only.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                            "The selected image processing routine may be applied to grayscale or binary image only.\n\nUse grayscale (and threshold filter if required) before.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            try
            {
                // apply filter to the image
                image = filter.Apply(image);
            }
            catch
            {
                MessageBox.Show("Error occured applying selected filter to the image", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private static byte Computedirection(Vector v)
        {
            if (v.y == 0) return 0;
            double segment = Math.PI / DIRECTIONNUM;
            double a = Math.Atan(v.x / v.y) + Math.PI / 2 + segment / 2;
            byte res = (byte)(Math.Truncate(a / segment));
            if (res == DIRECTIONNUM) res = 0;
            return res;
        }


        private static bool Findline(ref int linepointx, ref int linepointy, Bitmap bimage)
        {
            byte begincolor = (byte)bimage.GetPixel(linepointx - WINDOWSIZE / 2, linepointy - WINDOWSIZE / 2).ToArgb();
            for (int i = 0; i < WINDOWSIZE; i++)
            {
                int yi = linepointy + i - WINDOWSIZE / 2;
                for (int j = 0; j < WINDOWSIZE; j++)
                {
                    int xj = linepointx + j - WINDOWSIZE / 2;
                    byte curr = (byte)bimage.GetPixel(xj, yi).ToArgb();
                    if (Math.Abs(begincolor - curr) > 100)
                    {
                        linepointx = xj;
                        linepointy = yi;
                        if (curr < 120) //line (black) found
                        {
                            if (j == 0) linepointy--;
                            else linepointx--;
                        }
                        return true;
                    }
                }
            }
            return false;
        }




        private static DirectionalImage CreateDirections(Image image)
        {
            Bitmap bimage = new Bitmap(image);
            ApplyFilter(new GrayscaleBT709(), ref bimage);

            int width = bimage.Width;
            int height = bimage.Height;

            int dirwidth = (width - WINDOWSIZE - 1) / WINDOWSIZE;
            int dirheight = (height - WINDOWSIZE - 1) / WINDOWSIZE;

            DirectionalImage directions = new DirectionalImage(dirwidth, dirheight);

            int heightpos = WINDOWSIZE;
            int dirposheight = 0;
            bool foundfirst = false;

            while (heightpos < height - WINDOWSIZE)
            {
                int widthpos = WINDOWSIZE;
                int dirposwidth = 0;

                while (widthpos < width - WINDOWSIZE)
                {
                    int linepointx = widthpos;
                    int linepointy = heightpos;
                    byte direction;
                    if (Findline(ref linepointx, ref linepointy, bimage)) //managed to find line in window
                    {
                        Vector tangent = Tangent(linepointx, linepointy, bimage);
                        direction = Computedirection(tangent);
                        if (!foundfirst)
                        {
                            foundfirst = true;
                            directions.startline = dirposheight;
                        }
                        directions.endline = dirposheight;
                    }
                    else
                    {
                        direction = 9;
                    }
                    directions.arr[dirposwidth, dirposheight] = direction;
                    widthpos += WINDOWSIZE;
                    dirposwidth++;
                }

                heightpos += WINDOWSIZE;
                dirposheight++;
            }
            //writetofile(directions.arr, "image1");

            for (int i = 1; i <= 3; ++i)
                removenoise(directions);

            //writetofile(directions.arr, "image2");
            return directions;
        }


        private static void writetofile(byte[,] directions, string s)
        {
            StreamWriter sw = new StreamWriter("..\\..\\" + s + ".txt");
            for (int i = 0; i < directions.GetLength(1); i++)
            {
                for (int j = 0; j < directions.GetLength(0); j++)
                {
                    sw.Write(directions[j, i]);
                }
                sw.WriteLine();
            }
            sw.Close();
        }


        private static void removenoise(DirectionalImage directions)
        {
            byte curr, u, d, l, r;
            int changecount = 0;
            for (int j = Math.Max(directions.startline, 1); j < directions.arr.GetLength(1) - 1; j++)
            {
                for (int i = 1; i < directions.arr.GetLength(0) - 1; i++)
                {
                    curr = directions.arr[i, j];
                    u = directions.arr[i - 1, j];
                    d = directions.arr[i + 1, j];
                    l = directions.arr[i, j - 1];
                    r = directions.arr[i, j + 1];
                    int dircount = 0;

                    if (u != 9) ++dircount;
                    if (d != 9) ++dircount;
                    if (l != 9) ++dircount;
                    if (r != 9) ++dircount;

                    if (curr == 9)
                    {
                        if ((l != 9 && r != 9) || (u != 9 && d != 9)) //it's a noise!!
                        {
                            directions.arr[i, j] = (byte)Math.Round((u + d + l + r - 9.0 * (4 - dircount)) / dircount);
                            changecount++;
                        }
                    }
                    else //curr is not 9
                    {
                        if (curr != u && curr != r && curr != d && curr != l)
                        {
                            if (dircount == 0) //only 9s around
                            {
                                directions.arr[i, j] = 9;
                                changecount++;
                            }
                            else//not all 9s
                            {
                                if (dircount == 1 && u + l + r + d - 27 != curr) //3 nines & something strange around
                                {
                                    directions.arr[i, j] = 9;
                                    changecount++;
                                }
                                else
                                {
                                    directions.arr[i, j] =
                                 (byte)Math.Round((u + d + l + r - 9.0 * (4 - dircount)) / dircount);
                                    changecount++;
                                }
                            }
                        }
                    }
                }
            }
        }



        private static DirectionGraph CreateGraph(DirectionalImage directions)
        {

            DirectionGraph graph = new DirectionGraph();
            int width = directions.arr.GetLength(0);
            int height = directions.arr.GetLength(1);

            DirectionNode[,] tempnodes = new DirectionNode[width, height];

            DirectionNode dirnode;
            #region First line and first column


            if (directions.arr[0, 0] != 9)
            {
                dirnode = new DirectionNode();
                dirnode.direction = directions.arr[0, 0];
                dirnode.add(new Point(0, 0));
                graph.add(dirnode);
                tempnodes[0, 0] = dirnode;
            }
            for (int i = 1; i < width; i++)
            {
                byte curr = directions.arr[i, 0];
                if (curr == 9) continue;
                byte l = directions.arr[i - 1, 0];
                if (curr == l)
                {
                    tempnodes[i, 0] = tempnodes[i - 1, 0];
                    tempnodes[i - 1, 0].add(new Point(i, 0));
                }
                else
                {
                    dirnode = new DirectionNode();
                    dirnode.direction = curr;
                    dirnode.add(new Point(i, 0));
                    graph.add(dirnode);
                    tempnodes[i, 0] = dirnode;
                }
            }

            int a = 1;

            for (int j = 1; j < height; j++)
            {
                byte curr = directions.arr[0, j];
                if (curr == 9) continue;
                byte u = directions.arr[0, j - 1];
                if (curr == u)
                {
                    tempnodes[0, j] = tempnodes[0, j - 1];
                    tempnodes[0, j - 1].add(new Point(0, j));
                }
                else
                {
                    dirnode = new DirectionNode();
                    dirnode.direction = curr;
                    dirnode.add(new Point(0, j));
                    graph.add(dirnode);
                    tempnodes[0, j] = dirnode;
                }
            }

            #endregion

            int b = 0;

            #region Main
            for (int j = 1; j < directions.arr.GetLength(1); j++)
            {
                for (int i = 1; i < directions.arr.GetLength(0); i++)
                {
                    byte curr = directions.arr[i, j];
                    if (curr == 9) continue;
                    byte l = directions.arr[i - 1, j];
                    byte u = directions.arr[i, j - 1];

                    if (curr == l && curr == u && tempnodes[i, j - 1] != tempnodes[i - 1, j])
                    {
                        DirectionNode input, output;
                        if (tempnodes[i - 1, j].Weight > tempnodes[i, j - 1].Weight)
                        {
                            input = tempnodes[i, j - 1];
                            output = tempnodes[i - 1, j];
                        }
                        else
                        {
                            output = tempnodes[i, j - 1];
                            input = tempnodes[i - 1, j];
                        }

                        foreach (Point point in input.PointList)
                        {
                            tempnodes[point.x, point.y] = output;
                            output.add(point);
                        }
                        tempnodes[i, j] = output;
                        input.PointList.Clear();
                        output.add(new Point(i, j));
                    }

                    else if (curr == l)
                    {
                        tempnodes[i, j] = tempnodes[i - 1, j];
                        tempnodes[i - 1, j].add(new Point(i, j));
                    }
                    else if (curr == u)
                    {
                        tempnodes[i, j] = tempnodes[i, j - 1];
                        tempnodes[i, j - 1].add(new Point(i, j));
                    }
                    else
                    {
                        dirnode = new DirectionNode();
                        dirnode.direction = curr;
                        dirnode.add(new Point(i, j));
                        graph.add(dirnode);
                        tempnodes[i, j] = dirnode;
                    }
                }
            }

            #endregion

            DirectionGraph endgraph = new DirectionGraph();
            foreach (DirectionNode node in graph.Nodes)
            {
                if (node.Weight >= MINNOISECOUNT) endgraph.add(node);
            }


            #region computeweights

            foreach (DirectionNode node in endgraph.Nodes)
            {
                node.ComputeCenter();
            }

            #endregion

            FindNeighbours(endgraph, tempnodes); //tempnodes - array with references to DirectionNodes
            return endgraph;
        }


        public static DirectionGraph BuildSuperGraph(Image image)
        {
            DirectionalImage dirimage = CreateDirections(image);
            DirectionGraph graph = CreateGraph(dirimage);
            DirectionGraph supergraph = CreateSuperGraph(graph);
            return supergraph;
        }



        private static DirectionGraph CreateSuperGraph(DirectionGraph graph)
        {

            Point[] centres = new Point[DIRECTIONNUM];

            #region initcentres

            for (int i = 0; i < DIRECTIONNUM; i++)
            {
                centres[i] = new Point(0, 0);
            }
            #endregion

            int[] number = new int[DIRECTIONNUM]; //numbers of buckets
            int[] nodeweights = new int[DIRECTIONNUM];

            DirectionGraph supergraph = new DirectionGraph();
            double[,] edgeweights = new double[DIRECTIONNUM, DIRECTIONNUM];



            DirectionNode node1, node2;
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                node1 = graph.Nodes[i];
                int dir1 = node1.direction;
                centres[dir1] += node1.gravitycenter;
                number[dir1]++;
                nodeweights[dir1] += node1.Weight;
                for (int j = i + 1; j < graph.Nodes.Count; j++)
                {
                    node2 = graph.Nodes[j];
                    int dir2 = node2.direction;
                    if (dir1 == dir2) continue;
                    edgeweights[dir1, dir2] += graph.perimeterarray[i, j];
                    edgeweights[dir2, dir1] = edgeweights[dir1, dir2];

                }
            }


            for (byte i = 0; i < DIRECTIONNUM; i++)
            {
                supergraph.add(new DirectionNode(i));
                if (number[i] == 0) supergraph.Nodes[i].gravitycenter = new Point(0, 0);
                else supergraph.Nodes[i].gravitycenter = centres[i] / number[i];
                supergraph.Nodes[i].Weight = nodeweights[i];
            }


            for (int i = 0; i < DIRECTIONNUM; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    double dist = Distance(supergraph.Nodes[i].gravitycenter, supergraph.Nodes[j].gravitycenter);
                    edgeweights[i, j] += dist;
                    edgeweights[j, i] += dist;
                }
            }

            supergraph.weightarray = edgeweights;
            return supergraph;
        }


        private static double Distance(Point a, Point b)
        {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }



        private static void FindNeighbours(DirectionGraph graph, DirectionNode[,] nodes)
        {
            int count = graph.Nodes.Count;
            int[,] adjmatrix = new int[count, count];

            Dictionary<DirectionNode, int> dict = new Dictionary<DirectionNode, int>(40);
            int k = 0;
            foreach (DirectionNode node in graph.Nodes)
            {
                dict.Add(node, k);
                ++k;
            }

            int width = nodes.GetLength(0);
            int height = nodes.GetLength(1);

            for (int j = 1; j < height; j++)
            {
                for (int i = 1; i < width; i++)
                {
                    DirectionNode curr = nodes[i, j];
                    if (curr == null || curr.Weight < MINNOISECOUNT) continue;
                    DirectionNode l = nodes[i - 1, j];
                    DirectionNode u = nodes[i, j - 1];

                    int num1, num2;
                    if (l != null && l != curr && l.Weight >= MINNOISECOUNT)
                    {
                        dict.TryGetValue(l, out num1);
                        dict.TryGetValue(curr, out num2);
                        adjmatrix[num1, num2]++;
                        adjmatrix[num2, num1]++;
                    }
                    if (u != null && u != curr && u.Weight >= MINNOISECOUNT)
                    {
                        dict.TryGetValue(u, out num1);
                        dict.TryGetValue(curr, out num2);
                        adjmatrix[num1, num2]++;
                        adjmatrix[num2, num1]++;
                    }
                }
            }


            graph.perimeterarray = adjmatrix;
        }




        public static bool Compare(DirectionGraph g1, DirectionGraph g2)
        {
            const double q = 0.3;

            int simnodecount = 0;
            for (int i = 0; i <= 3; i++)
            {
                if (Math.Abs(g1.Nodes[i].Weight - g2.Nodes[i].Weight)
                    <= q * Math.Max(g1.Nodes[i].Weight, g2.Nodes[i].Weight))
                    simnodecount++;
            }

            if (simnodecount < 3) return false;

            int simcount = 0;
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    if (Math.Abs(g1.weightarray[i, j] - g2.weightarray[i, j])
                        <= q * Math.Max(g1.weightarray[i, j], g2.weightarray[i, j]))
                        simcount++;
                }

            }
            if (simcount < 3) return false;
            return true;
        }


        public static double CostFunction(DirectionGraph gr1, DirectionGraph gr2)
        {
            if (gr1.Nodes.Count != gr2.Nodes.Count) return -1;
            int count = gr1.Nodes.Count;
            int nodesum = 0;
            for (int i = 0; i < count; ++i)
            {
                nodesum += Math.Abs(gr1.Nodes[i].Weight - gr2.Nodes[i].Weight); //? Abs?
            }
            double weightsum = 0;
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    weightsum += Math.Abs(gr1.weightarray[i, j] - gr2.weightarray[i, j]);
                }
            }
            return nodesum * weightsum;
        }


        private static Vector Tangent(int x, int y, Bitmap b)
        {

            //            //Sobel
            //            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            //            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            ////Scharr
            int[,] gx = new int[,] { { -3, 0, 3 }, { -10, 0, 10 }, { -3, 0, 3 } };
            int[,] gy = new int[,] { { 3, 10, 3 }, { 0, 0, 0 }, { -3, -10, -3 } };

            float x1 = 0, y1 = 0;

            for (int hw = 0; hw < 3; hw++)
            {
                for (int wi = 0; wi < 3; wi++)
                {
                    int _i = x + wi - 1;
                    int _j = y + hw - 1;
                    byte curr = (byte)b.GetPixel(_i, _j).ToArgb();
                    x1 += gx[hw, wi] * curr;
                    y1 += gy[hw, wi] * curr;
                }
            }
            return new Vector(y1, -x1);
        }

        /// <summary>
        /// selects image with the lowest cost function value
        /// </summary>
        /// <param name="fprint">image we want to identify</param>
        /// <param name="printarray">array of fingerprints</param>
        /// <returns>index of the most appropriate image</returns>
        public static int MatchGraphPrint(DirectionGraph fprint, DirectionGraph[] printarray)
        {
            int minindex = 0;
            double minvalue = CostFunction(fprint, printarray[0]);

            for (int i = 1; i < printarray.Length; i++)
            {
                double value = CostFunction(fprint, printarray[i]);
                if (value < minvalue)
                {
                    minindex = i;
                    minvalue = value;
                }
            }
            return minindex;
        }

    }
}
