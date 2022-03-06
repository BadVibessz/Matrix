using System;
using System.Collections;
using System.Collections.Generic;

namespace matr_try2
{
    public struct Node
    {
        private readonly double _value;
        private string comment;
        private readonly int[] _position;

        public Node(int i, int j, double val, string c="")
        {
            this._position = new int[2];
            this._position[0] = i;
            this._position[1] = j;
            this._value = val;
            comment = c;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node other)
            {
                if ((this._position != other._position) || (this._value != other._value))
                    return false;
                return true;
            }

            return base.Equals(obj);
        }
        
        public static bool operator ==(Node n1, Node n2) => n1.Equals(n2);


        public static bool operator !=(Node n1, Node n2) => !n1.Equals(n2);

        public override int GetHashCode()
            => HashCode.Combine(_value, _position);

        public double value
        {
            get => this._value;
        }

        public int[] pos
        {
            get => this._position;
        }
    }

    public struct Neighbours : IEnumerable<Node>
    {
        private readonly Node[] _values;

        public int Size
        {
            get => this._values.Length;
        }

        public Neighbours(Node[] val)
        {
            this._values = new Node[val.Length];
            val.CopyTo(this._values, 0);
        }


        public IEnumerator<Node> GetEnumerator()
        {
            for (int i = 0; i < this._values.Length; i++)
                yield return this._values[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    public class SparseMatrix : IEnumerable<Node>
    {
        private readonly int _row, _col;
        private readonly Node[] _values;

        public SparseMatrix(Matrix other)
        {
            var temp = new List<Node>();
            for (int i = 0; i < other.Row; i++)
            for (int j = 0; j < other.Col; j++)
                if (other[i, j] != 0)
                    temp.Add(new Node(i, j, other[i, j]));
            this._values = temp.ToArray();
            this._row = other.Row;
            this._col = other.Col;
        }

        public SparseMatrix(SparseMatrix other)
        {
            other._values.CopyTo(this._values, 0);
            this._row = other._row;
            this._col = other._col;
        }


        public IEnumerator<Node> GetEnumerator()
        {
            for (int i = 0; i < this._row; i++)
            for (int j = 0; j < this._col; j++)
                yield return this[i, j];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj is SparseMatrix other)
            {
                if (this._values.Length != other._values.Length) return false;
                for (int i = 0; i < this._values.Length; i++)
                    if (this._values[i] != other._values[i])
                        return false;
                return true;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode() => this._values.GetHashCode();

        public static bool operator ==(SparseMatrix m1, SparseMatrix m2) => m1.Equals(m2);

        public static bool operator !=(SparseMatrix m1, SparseMatrix m2) => !m1.Equals(m2);


        public Node this[int i, int j]
        {
            get
            {
                i %= this._row;
                j %= this._col;

                if (i < 0) i += this._row;
                if (j < 0) j += this._col;

                if (Array.Exists(this._values, node => node.pos[0] == i && node.pos[1] == j))
                    return Array.Find(this._values, node => node.pos[0] == i && node.pos[1] == j);
                return new Node(i, j, 0);
            }
        }

        public Neighbours Neighbours(Node item)
        {
            var temp = new List<Node>();

            for (int i = item.pos[0] - 1; i <= item.pos[0] + 1; i++)
            {
                for (int j = item.pos[1] - 1; j <= item.pos[1] + 1; j++)
                {
                    if (i == item.pos[0] && j == item.pos[1]) continue;
                    if (this[i, j].value != 0) temp.Add(this[i, j]);
                    else temp.Add(new Node(i, j, 0));
                }
            }

            return new Neighbours(temp.ToArray());
        }
    }
}