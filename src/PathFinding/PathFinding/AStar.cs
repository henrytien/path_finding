using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ZiLongGame
{

    public class Node
    {
        public static int NODE_SIZE = 32;
        public Node m_parent;
        public Vector2 m_position;
        public Vector2 m_center
        {
            get
            {
                return new Vector2(m_position.X + NODE_SIZE / 2, m_position.Y + NODE_SIZE / 2);
            }
        }
        public float m_distanceToTarget;
        public float m_cost;
        public float m_weight;
        public float m_f
        {
            get
            {
                if (m_distanceToTarget != -1 && m_cost != -1)
                {
                    return m_distanceToTarget + m_cost;
                }
                else
                {
                    return -1;
                }
            }
        }

        public bool m_walkable;

        public Node(Vector2 pos, bool walkable, float weight = 1)
        {
            m_parent = null;
            m_position = pos;
            m_distanceToTarget = -1;
            m_cost = 1;
            m_weight = weight;
            m_walkable = walkable;
        }
    }
    public class OpenPoint : IComparable
    {
        public Int32 m_x;
        public Int32 m_y;
        public Int32 m_cost;
        public double m_pred;
        OpenPoint m_parent;
        public OpenPoint(Vector2 start, Vector2 end, float c, OpenPoint parent)
        {
            float relativeX = Math.Abs(end.X - start.X);
            float relativeY = Math.Abs(end.Y - start.Y);
            float delta = relativeX - relativeY;
            m_pred = Math.Max(relativeX, relativeY) * 14.0 - Math.Abs(delta) * 4.0 + c;
        }

        public bool CompareTo(object iObj)
        {
            OpenPoint openPoint = (OpenPoint)iObj;
            double result = this.m_pred - openPoint.m_pred;
            if (result > 0.0f)
                return true;
            else if (result == 0.0f)
                return true;
            return false;
        }

        public static bool operator <(OpenPoint a, OpenPoint b)
        {
            return a.m_pred < b.m_pred;
        }

        public static bool operator >(OpenPoint a, OpenPoint b)
        {
            return a.m_pred > b.m_pred;
        }


        public override int GetHashCode()
        {
            return m_x ^ m_y;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            OpenPoint p = obj as OpenPoint;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (m_x == p.m_x) && (m_y == p.m_y);
        }

        public bool Equals(OpenPoint p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (m_x == p.m_x) && (m_y == p.m_y);
        }

        int IComparable.CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(OpenPoint a, OpenPoint b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.m_x == b.m_x && a.m_y == b.m_y;
        }

        public static bool operator !=(OpenPoint a, OpenPoint b)
        {
            return !(a == b);
        }
    }

    
    public class AStar
    {
        public List<List<Node>> m_grid;
        int m_gridRows
        {
            get
            {
                return m_grid[0].Count;
            }
        }

        int m_gridCols
        {
            get
            {
                return m_grid.Count;
            }
        }

        public AStar(List<List<Node>> grid)
        {
            m_grid = grid;
        }

        public AStar()
        {
            m_grid = new List<List<Node>>();
        }

        public Stack<Node> FindPath(Vector2 start, Vector2 end)
        {
            Node m_start = new Node(new Vector2((int)(start.X / Node.NODE_SIZE), (int)(start.Y / Node.NODE_SIZE)), true);
            Node m_end = new Node(new Vector2((int)(end.X / Node.NODE_SIZE), (int)(end.Y / Node.NODE_SIZE)), true);

            Stack<Node> m_path = new Stack<Node>();
            List<Node> m_openList = new List<Node>();
            List<Node> m_closeList = new List<Node>();
            List<Node> m_adjacencies;
            Node m_current = m_start;

            // add start to open list
            m_openList.Add(m_start);

            while (m_openList.Count != 0 && !m_closeList.Exists(x => x.m_position == m_end.m_position))
            {
                m_current = m_openList[0];
                m_openList.Remove(m_current);
                m_closeList.Add(m_current);
                m_adjacencies = GetAdjacentNodes(m_current);

                foreach (var node in m_adjacencies)
                {
                    if (!m_closeList.Contains(node) && node.m_walkable)
                    {
                        if (!m_openList.Contains(node))
                        {
                            node.m_parent = m_current;
                            node.m_distanceToTarget = Math.Abs(node.m_position.X - m_end.m_position.X) +
                                Math.Abs(node.m_position.Y - m_end.m_position.Y);
                            node.m_cost = node.m_weight + node.m_parent.m_cost;
                            m_openList.Add(node);
                            m_openList = m_openList.OrderBy(n => n.m_f).ToList<Node>();
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!m_closeList.Exists(x => x.m_position == m_end.m_position))
            {
                return null;
            }

            // if all good, return path
            Node temp = m_closeList[m_closeList.IndexOf(m_current)];
            if (temp == null) return null;
            do
            {
                m_path.Push(temp);
                temp = temp.m_parent;
            } while (temp != m_start && temp != null);

            return m_path;
        }

        public List<Node> GetAdjacentNodes(Node node)
        {
            List<Node> temp = new List<Node>();
            int row = (int)node.m_position.X;
            int col = (int)node.m_position.Y;

            if (row + 1 < m_gridRows)
            {
                temp.Add(m_grid[col][row + 1]);
            }

            if (row - 1 >= 0)
            {
                temp.Add(m_grid[col][row - 1]);
            }

            if (col + 1 < m_gridCols)
            {
                temp.Add(m_grid[col + 1][row]);
            }

            if (col - 1 >= 0)
            {
                temp.Add(m_grid[col - 1][row]);
            }
            return temp;
        }
    }
}


