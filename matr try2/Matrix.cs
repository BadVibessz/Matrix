﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace matr_try2
{
    public class Matrix : IEnumerable<double>
    {
        protected readonly double[,] values;

        public Matrix(int r, int c)
        {
            if (r <= 0 || c <= 0) throw new Exception("Wrong argument");
            else
                this.values = new double[r, c];
        }

        public Matrix(double[,] ar)
        {
            //this.row = ar.GetLength(0); this.col = ar.Length / ar.GetLength(0);
            //this.value_matr = ar;

            int r = ar.GetLength(0);
            int c = ar.Length / ar.GetLength(0);
            this.values = new double[r, c];

            for (int i = 0; i < r; i++)
            for (int j = 0; j < c; j++)
                this.values[i, j] = ar[i, j];
        }

        public Matrix(Matrix other)
        {
            this.values = new double[other.Row, other.Col];
            for (int i = 0; i < other.Row; i++)
            for (int j = 0; j < other.Col; j++)
                this.values[i, j] = other.values[i, j];
        }

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < this.Row; i++)
            for (int j = 0; j < this.Col; j++)
                yield return this[i, j];
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


        public int Row
        {
            set { }
            get { return this.values.GetLength(0); }
        }

        public int Col
        {
            set { }
            get { return this.values.Length / values.GetLength(0); }
        }

        public double this[int i, int j]
        {
            get => this.values[i, j];
        }


        public int Rang
        {
            get
            {
                var copy = new Matrix(this);
                copy.ToTriangularMatrix();

                int count = 0;
                for (int i = 0; i < copy.Row; i++)
                {
                    int temp = 0;
                    for (int j = 0; j < copy.Col; j++)
                        if (copy[i, j] != 0)
                            temp++;
                    if (temp != 0) count++;
                }

                return count;
            }
        }


        public override bool Equals(object obj)
        {
            if (obj is Matrix m)
            {
                if (this.Row != m.Row || this.Col != m.Col)
                    return false;
                for (int i = 0; i < this.Row; i++)
                for (int j = 0; j < this.Col; j++)
                    if (this.values[i, j] != m.values[i, j])
                        return false;
                return true;
            }
            else return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return values.GetHashCode();
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            if (m1.Equals(m2)) return true;
            return false;
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !(m1 == m2);
        }

        protected Matrix(double a, int row = 1, int col = 1) : this(row, col)
        {
            for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                if (i == j) values[i, i] = a;
                else values[i, j] = 0;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            int r1 = m1.Row;
            int r2 = m2.Row;
            int c1 = m1.Col;
            int c2 = m2.Col;

            if (r1 != r2 || c1 != c2)
                throw new Exception("Cannot add matrix");
            else
            {
                Matrix res = new Matrix(r1, c1);
                for (int i = 0; i < r1; i++)
                for (int j = 0; j < c1; j++)
                    res.values[i, j] = m1.values[i, j] + m2.values[i, j];
                return res;
            }
        }

        public static Matrix operator +(Matrix m1, double n)
        {
            int r = m1.Row;
            int c = m1.Col;

            Matrix m2 = new Matrix(n, r, c);
            Matrix res = new Matrix(r, c);

            for (int i = 0; i < r; i++)
            for (int j = 0; j < c; j++)
                res.values[i, j] = m1.values[i, j] + m2.values[i, j];
            return res;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            int r1 = m1.Row;
            int r2 = m2.Row;
            int c1 = m1.Col;
            int c2 = m2.Col;

            if (r1 != r2 || c1 != c2)
                throw new Exception("Cannot add matrix");
            else
            {
                Matrix res = new Matrix(r1, c1);
                for (int i = 0; i < r1; i++)
                for (int j = 0; j < c1; j++)
                    res.values[i, j] = m1.values[i, j] - m2.values[i, j];
                return res;
            }
        }

        public static Matrix operator -(Matrix m1, double n)
        {
            int r = m1.Row;
            int c = m1.Col;

            Matrix m2 = new Matrix(n, r, c);
            Matrix res = new Matrix(r, c);

            for (int i = 0; i < r; i++)
            for (int j = 0; j < c; j++)
                res.values[i, j] = m1.values[i, j] - m2.values[i, j];
            return res;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int r1 = m1.Row;
            int r2 = m2.Row;
            int c1 = m1.Col;
            int c2 = m2.Col;

            if (c1 != r2)
                throw new Exception("Cannot multiply matrix");
            else
            {
                Matrix res = new Matrix(r1, c2);
                for (int i = 0; i < r1; i++)
                for (int j = 0; j < c2; j++)
                for (int l = 0; l < r2; l++)
                    res.values[i, j] += m1.values[i, l] * m2.values[l, j];
                return res;
            }
        }

        public static Matrix operator *(Matrix m1, double n)
        {
            int r = m1.Row;
            int c = m1.Col;

            Matrix m2 = new Matrix(n, r, c);
            Matrix res = new Matrix(r, c);
            for (int i = 0; i < r; i++)
            for (int j = 0; j < c; j++)
            for (int l = 0; l < r; l++)
                res.values[i, j] += m1.values[i, l] * m2.values[l, j];
            return res;
        }

        public static Matrix operator ~(Matrix m)
        {
            int r = m.Row;
            int c = m.Col;
            double[,] ar = new double[r, c];
            for (int i = 0; i < r; i++)
            for (int j = 0; j < c; j++)
                ar[i, j] = m.values[j, i];
            return new Matrix(ar);
        }


        private void SwapRows(int i, int j)
        {
            for (int k = 0; k < Col; k++)
                (values[i, k], values[j, k]) = (values[j, k], values[i, k]);
        }

        public void ToTriangularMatrix()
        {
            for (int k = 0; k < Row; k++)
            {
                // Initialize maximum value and index for pivot
                int i_max = k;
                int v_max = (int)values[i_max, k];

                /* find greater amplitude for pivot if any */
                for (int i = k + 1; i < Row; i++)
                {
                    if (Math.Abs(values[i, k]) > v_max)
                    {
                        v_max = (int)values[i, k];
                        i_max = i;
                    }

                    /* If a principal diagonal element  is zero,
                    *  it denotes that matrix is singular, and
                    *  will lead to a division-by-zero later. */
                    if (values[k, i_max] == 0)
                        throw new Exception("Matrix is singular"); // Matrix is singular

                    /* Swap the greatest value row with
                       current row
                    */
                    if (i_max != k)
                        SwapRows(k, i_max);

                    for (int l = k + 1; l < Row; l++)
                    {
                        /* factor f to set current row kth element
                        *  to 0, and subsequently remaining kth
                        *  column to 0 */
                        double f = values[l, k] / values[k, k];

                        /* subtract fth multiple of corresponding
                           kth row element*/
                        for (int j = k + 1; j < Col; j++)
                            values[l, j] -= values[k, j] * f;

                        /* filling lower triangular matrix with
                        *  zeros*/
                        values[l, k] = 0;
                    }
                }
            }
        }


        public double[] CalculateUnknowns()
        {
            // An array to store solution
            double[] x = new double[Col];

            /* Start calculating from last equation up to the
               first */
            for (int i = Row - 1; i >= 0; i--)
            {
                /* start with the RHS of the equation */
                x[i] = values[i, Row];

                /* Initialize j to i+1 since matrix is upper
                triangular*/
                for (int j = i + 1; j < Row; j++)
                {
                    /* subtract all the lhs values
                     * except the coefficient of the variable
                     * whose value is being calculated */
                    x[i] -= values[i, j] * x[j];
                }

                /* divide the RHS by the coefficient of the
                unknown being calculated */
                x[i] /= values[i, i];
            }

            return x;
        }
    }

    class SquareMatrix : Matrix
    {
        public SquareMatrix(int n) : base(n, n)
        {
            if (this.Row != this.Col) throw new Exception("isn't square matrix");
        }

        public SquareMatrix(double[,] ar) : base(ar)
        {
            if (this.Row != this.Col) throw new Exception("isn't square matrix");
        }


        public SquareMatrix(SquareMatrix other) : base(other)
        {
        }


        public SquareMatrix(Matrix other) : base(other)
        {
            if (other.Col != other.Row) throw new Exception("is not square");
        }


        public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b) =>
            new SquareMatrix((Matrix)a + b);

        public static SquareMatrix operator +(SquareMatrix a, double n) =>
            new SquareMatrix((Matrix)a + n);

        public static SquareMatrix operator -(SquareMatrix a, SquareMatrix b) =>
            new SquareMatrix((Matrix)a - b);

        public static SquareMatrix operator -(SquareMatrix a, double n) =>
            new SquareMatrix((Matrix)a - n);

        public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b) =>
            new SquareMatrix((Matrix)a * b);

        public static SquareMatrix operator *(SquareMatrix a, double n) =>
            new SquareMatrix((Matrix)a * n);


        public static SquareMatrix operator ~(SquareMatrix a) =>
            new SquareMatrix((Matrix)(~a));


        //maybe private
        protected static SquareMatrix EraseRowCol(SquareMatrix m, int r, int c)
        {
            int n = m.Col;
            double[,] ar = new double[n - 1, n - 1];
            List<double> temp = new List<double>();

            for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                if (i == r || j == c) continue;
                else temp.Add(m.values[i, j]);
            }

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                    ar[i, j] = temp.ElementAt(j);
                for (int k = 0; k < n - 1; k++)
                    temp.RemoveAt(0);
            }

            return new SquareMatrix(ar);
        }

        public double Det
        {
            get
            {
                if (this.Col == 1)
                    return this.values[0, 0];
                if (this.Col == 2)
                    return this.values[0, 0] * this.values[1, 1] - this.values[0, 1] * this.values[1, 0];

                double res = 0;
                int n = this.Col;
                for (int i = 0; i < n; i++)
                {
                    double a = this.values[i, 0];
                    int sgn = Convert.ToInt32(Math.Pow(-1, i));

                    SquareMatrix minor = EraseRowCol(this, i, 0);

                    res += sgn * a * minor.Det;
                }

                return res;
            }
        }

        public double[] Cramer(double[] x) //todo: test
        {
            var temp = new List<double>();
            int n = this.Row;
            double d = this.Det;

            for (int i = 0; i < n; i++)
            {
                var minor = new SquareMatrix(this);

                for (int j = 0; j < n; j++)
                    minor.values[j, i] = x[j];

                temp.Add(minor.Det / d);
            }

            return temp.ToArray();
        }
    }

    class InvertibleMatrix : SquareMatrix
    {
        public InvertibleMatrix(int n) : base(n)
        {
        }

        public InvertibleMatrix(double[,] ar) : base(ar)
        {
            if (this.Det == 0) throw new Exception("Cannot invert matrix, det is zero");
        }

        public InvertibleMatrix(Matrix other) : base(other)
        {
            if (Det == 0) throw new Exception("Det is zero!");
        }

        // public InvertibleMatrix(SquareMatrix other) : base(other)
        // {
        //     if (other.Det == 0) throw new Exception("Det is zero!");
        // }

        public InvertibleMatrix(InvertibleMatrix other) : base(other)
        {
        }


        public static InvertibleMatrix operator ~(InvertibleMatrix m)
        {
            double[,] ar = new double[m.Row, m.Col];
            for (int i = 0; i < m.Row; i++)
            for (int j = 0; j < m.Col; j++)
                ar[i, j] = m.values[j, i];
            return new InvertibleMatrix(ar);
        }


        public static InvertibleMatrix operator +(InvertibleMatrix a, InvertibleMatrix b) =>
            new InvertibleMatrix((SquareMatrix)a + b);

        public static InvertibleMatrix operator +(InvertibleMatrix a, double n) =>
            new InvertibleMatrix((SquareMatrix)a + n);

        public static InvertibleMatrix operator -(InvertibleMatrix a, InvertibleMatrix b) =>
            new InvertibleMatrix((SquareMatrix)a - b);

        public static InvertibleMatrix operator -(InvertibleMatrix a, double n) =>
            new InvertibleMatrix((SquareMatrix)a - n);

        public static InvertibleMatrix operator *(InvertibleMatrix a, InvertibleMatrix b) =>
            new InvertibleMatrix((SquareMatrix)a * b);

        public static InvertibleMatrix operator *(InvertibleMatrix a, double n) =>
            new InvertibleMatrix((SquareMatrix)a * n);

        public InvertibleMatrix Invert()
        {
            int r = this.Row;
            int c = this.Col;
            double d = this.Det;
            double[,] ar = new double[r, c];

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                    ar[i, j] = EraseRowCol(this, i, j).Det;
            }

            InvertibleMatrix algebraicAddition = new InvertibleMatrix(ar);
            algebraicAddition = ~algebraicAddition;

            return algebraicAddition * (1 / d);
        }
    }
}