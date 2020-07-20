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
            Test.Pathfind();
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
}
