using System;

namespace matr_try2
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new Matrix(new double[3, 3] {{1, 0, 4}, {0, -1, -2}, {0, 0, 0}});
            var y = new SparseMatrix(x);

            for (int i = 0; i < x.Row; i++)
            {
                for (int j = 0; j < x.Col; j++)
                {
                    Console.Write(x[i, j]);
                    Console.Write(" ");
                }
            
                Console.WriteLine();
            }
            

            Console.WriteLine();
            var test = y.Neighbours(y[-100, -1]);
            foreach (var i in test)
            {
                Console.Write(i.value);
                Console.Write(" ");
            }
        }
    }
}