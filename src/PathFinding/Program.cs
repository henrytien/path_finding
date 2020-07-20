using System;
using System.Collections.Generic;
using System.Numerics;

namespace ZiLongGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Test.Pathfind();
            TestPathFinding.Pathfind();
        }
    }

    class Test
    {
        public static void Pathfind()
        {
           
            AStar m_AStar = new AStar();
            List<Node> m_listNode = new List<Node>();
            for (int widthTrav = 0; widthTrav < 64; widthTrav++)
            {
                
                for (int heightTrav = 0; heightTrav < 64; heightTrav++)
                {
                    Vector2 m_position = new Vector2(widthTrav, heightTrav);
                
                    Node node = new Node(m_position,true);

                    m_listNode.Add(node);
                   
                }
                m_AStar.m_grid.Add(m_listNode);
            }
            Vector2 m_start = new Vector2(11, 10);
            Vector2 m_end = new Vector2(34, 46);
            m_AStar.FindPath(m_start, m_end);
        }
    }

   class TestPathFinding
    {
        public static void Pathfind()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(29, 99);

            NewAstar aStar = new NewAstar();
            aStar.CreateMap();

            aStar.m_mapBuffer[(Int32)start.X, (Int32)start.Y] = aStar.m_mapBuffer[(Int32)end.X, (Int32)end.Y] = ' ';

            aStar.m_closeAndBarrierList[(Int32)start.X, (Int32)start.Y] = aStar.m_closeAndBarrierList[(Int32)end.X, (Int32)end.Y] = false;

            List<OpenPoint> path = aStar.FindPath(start, end);

            for (int i = 0; i < path.Count; i++)
            {
                aStar.m_mapBuffer[path[i].m_x, path[i].m_y] = '0';
            }

            aStar.PrintMap();
            Console.ReadKey();
        }

    }
}
