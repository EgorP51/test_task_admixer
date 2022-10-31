//Завдання 1. Аналог три в ряд
//Матриця 9х9 випадково заповнюється числами від 0 до 3 включно.
//Якщо по горизонталі/вертикалі збігаються 3 або більше чисел,
//видаляємо їх, зсунувши всі елементи матриці зверху донизу заповнивши порожній простір.
//Порожні елементи, що залишилися, заповнюємо випадковими числами від 0 до 3 включно.
//Повторюємо процедуру доти не буде збігів. Результат виконання вивести/залогувати

namespace TestTaskAdmixer
{
    class Matrix
    {
        const int matrixSize = 9;
        private int[,] matrix = new int[matrixSize, matrixSize];
        Random random = new Random();

        public Matrix()
        {
            FillMatrixRandom();
        }

        public void Run()
        {
            Console.WriteLine("Your matrix!");
            DrawMatrix();

            bool canDeleteSimilarity = true;

            while (canDeleteSimilarity)
            {
                int similarityCount = 0;

                for (int i = 0; i < matrixSize; i++)
                {
                    for (int j = 0; j < matrixSize; j++)
                    {
                        List<Point> similarity = FindSimilarity(i, j);

                        bool similarityRemoved = DeleteSimilarity(similarity);

                        if (similarityRemoved)
                        {
                            Console.WriteLine("     ↓  ↓  ↓");
                            DrawMatrix();

                            FillEmptyFields(similarity);

                            Console.WriteLine("     ↓  ↓  ↓");
                            DrawMatrix();
                        }

                        if(similarity.Count > 0)
                        {
                            similarityCount++;
                        }
                    }
                }
                if(similarityCount == 0)
                {
                    canDeleteSimilarity = false;
                    Console.WriteLine("Final matrix:");
                    DrawMatrix();
                }
            }
        }

        private void FillEmptyFields(List<Point> points)
        { 
            int pointsCount = points.Count;

            bool horizontalSimilarity = points[0].x == points[2].x;
            bool verticalSimilarity = points[0].y == points[2].y;

            if (points[0].x == 0 && (horizontalSimilarity || verticalSimilarity))
            {//1
                for (int i = 0; i < pointsCount; i++)
                {
                    matrix[points[i].x, points[i].y] = random.Next(0, 4);
                }
            }
            else
            if (verticalSimilarity && points[0].x >= pointsCount)
            {//2.1
                int offsetColunmLenght = points[0].x + pointsCount - 1;
                int emptyColunmLenght = points[0].x - pointsCount;

                for (int i = offsetColunmLenght; i >= pointsCount; i--)
                {
                    matrix[i, points[0].y] = matrix[i - pointsCount, points[0].y];
                }
                for (int i = 0; i < emptyColunmLenght; i++)
                {
                    matrix[i, points[0].y] = random.Next(0, 4);
                }
            }
            else
            if (horizontalSimilarity && points[0].x > 0)
            {//2.2
                int offsetColunmLenght = points[0].x;

                for (int i = offsetColunmLenght; i >= 1; i--)
                {
                    for (int j = 0; j < pointsCount; j++)
                    {
                        matrix[i, points[j].y] = matrix[i - 1, points[j].y];
                    }
                }
                for (int i = 0; i < pointsCount; i++)
                {
                    matrix[0, points[i].y] = random.Next(0, 4);
                }
            }
            else
            if (verticalSimilarity && points[0].x <= pointsCount && points[0].x != 0)
            {//3.1
                for (int i = 0; i < points[0].x; i++)
                {
                    matrix[i + pointsCount, points[0].y] = matrix[i, points[0].y];
                }
                
                for (int i = 0; i < points[0].x + (pointsCount - points[0].x); i++)
                {
                    matrix[i, points[0].y] = random.Next(0, 4);
                }
            }

        }
        private bool DeleteSimilarity(List<Point> similarity)
        {
            if(similarity.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (var point in similarity)
                {
                    matrix[point.x, point.y] = 4;
                }
                return true;
            }
        }

        private List<Point>? FindSimilarity(int i, int j)
        {
            List<Point> similarity = new List<Point>();

            if (RightPartSimilarity(i, j))
            {
                similarity = CheckNeighbors(i, j, 0, 1);
            }
            else
            if (LeftPartSimilarity(i, j))
            {
                similarity = CheckNeighbors(i, j, 0, -1);
            }
            else
            if (TopPartSimilarity(i, j))
            {
                similarity = CheckNeighbors(i, j, -1, 0);
            }
            else
            if (BottomPartSimilarity(i, j))
            {
                similarity = CheckNeighbors(i, j, 1, 0);
            }

            similarity.Sort();

            return similarity;
        }

        private bool RightPartSimilarity(int i, int j)
        {
            if (CheckNeighbors(i, j, 0, 1).Count >= 3)
                return true;
            else
                return false;
        }
        private bool LeftPartSimilarity(int i, int j)
        {
            if (CheckNeighbors(i, j, 0, -1).Count >= 3)
                return true;
            else
                return false;
        }
        private bool TopPartSimilarity(int i, int j)
        {
            if (CheckNeighbors(i, j, -1, 0).Count >= 3)
                return true;
            else
                return false;
        }
        private bool BottomPartSimilarity(int i, int j)
        {
            if (CheckNeighbors(i, j, 1, 0).Count >= 3)
                return true;
            else
                return false;
        }

        private List<Point> CheckNeighbors(int i, int j, int iCoefficient, int jCoefficient)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(i, j));
            try
            {
                for (int g = 1; g < matrixSize - 1; g++)
                {
                    if (matrix[i, j] == matrix[i + (iCoefficient * g), j + (jCoefficient * g)])
                    {
                        points.Add(new Point(i + (iCoefficient * g), j + (jCoefficient * g)));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (IndexOutOfRangeException) { }

            return points;
        }

        private void FillMatrixRandom()
        {
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    matrix[i, j] = random.Next(0, 4);
                }
            }
        }
        private void DrawMatrix()
        {
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    switch (matrix[i, j])
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case 4:

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.White;
                            break;
                    }

                    Console.Write(matrix[i, j] + " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
    struct Point: IComparable<Point>
    {
        public int x { get; }
        public int y { get; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int CompareTo(Point point)
        {
            var status = (x > point.x) ? 1 : ((x == point.x) ? 0 : -1);
            return status;
        }
    }
}
