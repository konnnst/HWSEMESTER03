namespace MultiThread;

public class Matrix
{
    private int[,] matrix;
    private int Width, Height;

    /// <summary>
    /// Creates matrix with random elements by given height and width
    /// </summary>
    /// <param name="h">Matrix height</param>
    /// <param name="w">Matrix width</param>
    public Matrix(int h, int w)
    {
        if (h == 0 || w == 0)
        {
            this.Width = 0;
            this.Height = 0;
            this.matrix = new int[0, 0];
            return;
        }

        this.matrix = new int[h, w];
        this.Width =    w;
        this.Height = h;
    
        var random = new Random();
        for (var i = 0; i < h; ++i)
        {
            for (var k = 0; k < w; ++k)
                matrix[i, k] = random.Next(-5, 5);
        }
    }

    /// <summary>
    /// Reads matrix from given file
    /// </summary>
    /// <param name="fileName">Name of file to read from /bin project folder</param>
    /// <exception cref="FormatException"></exception>
    public Matrix(string fileName)
    {
        if (!File.Exists(MyConstants.CurrentFolder + fileName))
        {
            Console.WriteLine("File not exists");
            this.matrix = new int[0, 0];
            this.Height = 0;
            this.Width = 0;
            return;
        }
        StreamReader reader = new StreamReader(MyConstants.CurrentFolder + fileName);

        string? line;
        var matrix = new List<int[]>();

        if ((line = reader.ReadLine()) == null)
        {
            this.Height = 0;
            this.Width = 0;
            this.matrix = new int[0, 0];
            return;
        }

        var w = line.Split().Length;

        while (line != null)
        {
            var rowNums = line.Split();
            if (rowNums.Length != w)
                throw new FormatException("Incorrect matrix size");
            var row = new int[w];
            for (int i = 0; i < w; ++i)
            {
                if (!Int32.TryParse(rowNums[i], out row[i]))
                    throw new FormatException("Incorrect element type");
            }
            matrix.Add(row);
            line = reader.ReadLine();
        }
        this.Height = matrix.Count;
        this.Width = w;
        this.matrix = new int[this.Height, this.Width];

        for (int i = 0; i < this.Height; ++i)
        {
            for (int k = 0; k < this.Width; ++k)
                this.matrix[i, k] = matrix[i][k];
        }
    }

    /// <summary>
    /// Creates matrix by given 2d array
    /// </summary>
    /// <param name="newMatrix">2d array of mattrix coefficients</param>
    public Matrix(int[,] newMatrix)
    {
        this.matrix = newMatrix;
        this.Height = matrix.GetLength(0);
        this.Width = matrix.GetLength(1);
    }

    /// <summary>
    /// Saves matrix to file in directory, where .exe located
    /// with name "matrix_{index}.txt" with first free index
    /// </summary>
    public void Save()
    {
        var i = -1;
        while (File.Exists(String.Format(MyConstants.MatrixPath, ++i))) { }
        this.Save(i);
    }

    /// <summary>
    /// Saves matrix to file in directory, where .exe located
    /// with name "matrix_{index}.txt", if not matrix with this
    /// index not exists
    /// </summary>
    /// <param name="fileIndex">index in matrix file name</param>
    public void Save(int fileIndex)
    {
        var maxIndex = -1;
        while (File.Exists(String.Format(MyConstants.MatrixPath, ++maxIndex))) { }
        if (maxIndex > fileIndex)
        {
            Console.WriteLine("File with this index already exists");
            return;
        }

        StreamWriter writer = new StreamWriter(String.Format(MyConstants.MatrixPath, fileIndex));
        for (var i = 0; i < this.Height; ++i)
        {
            for (var k = 0; k < this.Width; ++k)
                writer.Write(matrix[i, k] + " ");
            writer.Write("\n");
        }
        writer.Close();
    }

    /// <summary>
    /// Prints matrix in console
    /// </summary>
    public void Print()
    {
        if (this.Width == 0)
            Console.WriteLine("Empty matrix");
        for (var i = 0; i < this.Height; ++i)
        {
            for (var k = 0; k < this.Width; ++k)
                Console.Write("{0} ", this.matrix[i, k].ToString().PadLeft(4));
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Multiplies two matrices taken from files and writes to file in
    /// folder with .exe
    /// </summary>
    /// <param name="leftMatrixFile">First matrix file name</param>
    /// <param name="rightMatrixFile">Second matix file name</param>
    /// <param name="threadCount">Count of threads for multiplication</param>
    static public void FileMultiThreadMultiply(string leftMatrixFile, string rightMatrixFile, int threadCount)
    {
        if (!File.Exists(leftMatrixFile) || !File.Exists(rightMatrixFile))
        {
            Console.WriteLine("Files not exits");
            return;
        }
        var a = new Matrix(leftMatrixFile);
        var b = new Matrix(rightMatrixFile);

        var c = MultiThreadMultiply(a, b, threadCount);

        c.Save();
    }

    /// <summary>
    /// Multiplies two matrices taken from files and
    /// writes result in folder with .exe file
    /// </summary>
    /// <param name="f1">First matrix</param>
    /// <param name="f2">Second matrix</param>
    static public void FileMultilply(string f1, string f2)
    {
        if (!File.Exists(f1) || !File.Exists(f2))
        {
            Console.WriteLine("Files not exits");
            return;
        }
        var a = new Matrix(f1);
        var b = new Matrix(f2);

        var c = Multiply(a, b);

        c.Save();
    }

    /// <summary>
    /// Checks if two matrices equal
    /// </summary>
    /// <param name="leftMatrix">First matrix</param>
    /// <param name="rightMatrix">Second matrix</param>
    /// <returns>True if equal, else false</returns>
    static public bool Compare(Matrix leftMatrix, Matrix rightMatrix)
    {
        if (System.Object.ReferenceEquals(leftMatrix, rightMatrix))
            return true;
        if (leftMatrix.Width != rightMatrix.Width ||
            leftMatrix.Height != rightMatrix.Height ||
            rightMatrix.IsNull() || leftMatrix.IsNull())
            return false;

        for (var i = 0; i < leftMatrix.Height; ++i)
        {
            for (var j = 0; j < leftMatrix.Width; ++j)
            {
                if (leftMatrix.matrix[i, j] != rightMatrix.matrix[i, j])
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Multiplies two matrices in single thread mode
    /// </summary>
    /// <param name="leftMatrix">First matrix</param>
    /// <param name="rightMatrix">Second matrix</param>
    /// <returns>Multiplication result</returns>
    static public Matrix Multiply(Matrix leftMatrix, Matrix rightMatrix)
    {
        if (leftMatrix.Width != rightMatrix.Height)
            return new Matrix(0, 0);

        var resultMatrix = new Matrix(leftMatrix.Height, rightMatrix.Width);

        if (resultMatrix.IsNull())
            return resultMatrix;

        for (var i = 0; i < leftMatrix.Height; ++i)
        {
            for (var j = 0; j < rightMatrix.Width; ++j)
                resultMatrix.matrix[i, j] = Enumerable.Range(0,rightMatrix.Height).Sum(k
                    => leftMatrix.matrix[i, k] * rightMatrix.matrix[k, j]);
        }

        return resultMatrix;
    }

    /// <summary>
    /// Multiplies two matrices in multithreading mode
    /// </summary>
    /// <param name="leftMatrix">First matrix</param>
    /// <param name="rightMatrix">Second matrix</param>
    /// <param name="threadCount">Count of using threads</param>
    /// <returns>Multiplication result</returns>
    static public Matrix MultiThreadMultiply(Matrix leftMatrix, Matrix rightMatrix, int threadCount)
    {
        if (threadCount < 0 || threadCount > leftMatrix.Height || threadCount > Environment.ProcessorCount)
        {
            Console.WriteLine("Thread count clipped according to matrix size");
            threadCount = Math.Max(Math.Min(leftMatrix.Height, Environment.ProcessorCount), 1);
        }
        if (leftMatrix.Width != rightMatrix.Height)
            return new Matrix(0, 0);

        var resultMatrix = new Matrix(leftMatrix.Height, rightMatrix.Width);
        var threads = new Thread[threadCount];
        var threadPiece = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(leftMatrix.Height) / threadCount)));

        if (resultMatrix.IsNull())
            return resultMatrix;

        for (var i = 0; i < threadCount; ++i)
        {
            var localI = i;
            threads[i] = new Thread(() =>
            {
                for (var n = threadPiece * localI; n < threadPiece * (localI + 1) && n < leftMatrix.Height; ++n)
                {
                    for (var j = 0; j < rightMatrix.Width; ++j)
                        resultMatrix.matrix[n, j] = Enumerable.Range(0, leftMatrix.Width).Sum(k => leftMatrix.matrix[n, k] * rightMatrix.matrix[k, j]);
                }
            });
        }

        for (var i = 0; i < threads.Length; ++i)
            threads[i].Start();

        for (var i = 0; i < threads.Length; ++i)
            threads[i].Join();

        return resultMatrix;
    }

    public bool IsNull()
    {
        if (this.Height != 0 || this.Width != 0)
            return false;
        return true;
    }
}

internal class Program
{
    static void Main()
    {   
        var a = new Matrix(5, 5);
        a.Save();
       /* var bench = new MultBenchmark();

        Console.WriteLine(MyConstants.MatrixPath);
        Console.WriteLine(Environment.ProcessorCount);

        Console.WriteLine("Welcome to matrix multiplication benchmark");

        Console.Write("Input max size of matrix in stats row: ");
        if (Int32.TryParse(Console.ReadLine(), out int maxMatrixSize))
        {
            Console.Write("Input working thread count: ");
            if (Int32.TryParse(Console.ReadLine(), out int threadCount))
                bench.StatsTable(threadCount, maxMatrixSize);
            else
                Console.WriteLine("Incorrect input format, int expected");
        }
        else
            Console.WriteLine("Incorrect input format, int expected");

        Console.ReadKey();*/
    }
}
