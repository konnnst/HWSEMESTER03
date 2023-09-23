namespace MultiThread;

public class Matrix
{
    private int[,] matrix;
    private int Width { get; set; }
    private int Height { get; set; }

    /// <summary>
    /// Creates matrix with random elements by given height and width
    /// </summary>
    /// <param name="h">Matrix height</param>
    /// <param name="w">Matrix width</param>
    public Matrix(int h, int w)
    {
        this.matrix = new int[h, w];
        this.Width = w; this.Height = h;

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
        if (!File.Exists(Constants.CurrentFolder + fileName))
        {
            Console.WriteLine("File not exists");
            return;
        }
        StreamReader reader = new StreamReader(Constants.CurrentFolder + fileName);
        int w;
        string line;
        var matrix = new List<int[]>();

        if ((line = reader.ReadLine()) == null)
        {
            this.Height = 0; this.Width = 0;
            this.matrix = new int[0, 0];
            return;
        }

        w = line.Split().Length;

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
        this.Height = matrix.Count; this.Width = w;
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
    /// Returns matrix width
    /// </summary>
    /// <returns></returns>
    public int GetWidth()
    {
        return this.Width;
    }

    /// <summary>
    /// Returns matrix height
    /// </summary>
    /// <returns></returns>
    public int GetHeight()
    {
        return this.Height;
    }

    /// <summary>
    /// Saves matrix to file in directory, where .exe located
    /// with name "matrix_{index}.txt" with first free index
    /// </summary>
    public void Save()
    {
        var i = -1;
        while (File.Exists(String.Format(Constants.MatrixPath, ++i))) { }
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
        var maxIndex = 0;
        while (File.Exists(String.Format(Constants.MatrixPath, ++maxIndex))) { }
        if (maxIndex > fileIndex)
        {
            Console.WriteLine("File with this index already exists");
            return;
        }

        StreamWriter writer = new StreamWriter(String.Format(Constants.MatrixPath, fileIndex));
        for (var i = 0; i < this.Height; ++i)
        {
            for (var k = 0; k < this.Width; ++k)
                writer.Write(matrix[i, k] + " ");
            writer.Write("\n");
        }
        writer.Write("{0} {1}", this.Height, this.Width);
        writer.Close();
    }

    /// <summary>
    /// Prints matrix in console
    /// </summary>
    public void Print()
    {
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
    /// <param name="f1">First matrix file name</param>
    /// <param name="f2">Second matix file name</param>
    /// <param name="threadCount">Count of threads for multiplication</param>
    static public void FileMultThread(string f1, string f2, int threadCount)
    {
        if (!File.Exists(f1) || !File.Exists(f2))
        {
            Console.WriteLine("Files not exits");
            return;
        }
        var a = new Matrix(f1);
        var b = new Matrix(f2);

        var c = MultThread(a, b, threadCount);

        c.Save();
    }

    /// <summary>
    /// Multiplies two matrices taken from files and
    /// writes result in folder with .exe file
    /// </summary>
    /// <param name="f1">First matrix</param>
    /// <param name="f2">Second matrix</param>
    static public void FileMult(string f1, string f2)
    {
        if (!File.Exists(f1) || !File.Exists(f2))
        {
            Console.WriteLine("Files not exits");
            return;
        }
        var a = new Matrix(f1);
        var b = new Matrix(f2);

        var c = Mult(a, b);

        c.Save();
    }

    /// <summary>
    /// Checks if two matrices equal
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns>True if equal, else false</returns>
    static public bool Compare(Matrix a, Matrix b)
    {
        if (a.Width != b.Width || a.Height != b.Height)
            return false;

        for (var i = 0; i < a.Height; ++i)
        {
            for (var j = 0; j < a.Width; ++j)
            {
                if (a.matrix[i, j] != b.matrix[i, j])
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Multiplies two matrices in single thread mode
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <returns>Multiplication result</returns>
    static public Matrix Mult(Matrix a, Matrix b)
    {
        if (a.Width != b.Height)
            return null;
        var c = new Matrix(a.Height, b.Width);

        for (var i = 0; i < a.Height; ++i)
        {
            for (var j = 0; j < b.Width; ++j)
            {
                c.matrix[i, j] = 0;
                for (var k = 0; k < b.Height; ++k)
                    c.matrix[i, j] += a.matrix[i, k] * b.matrix[k, j];
            }
        }

        return c;
    }

    /// <summary>
    /// Multiplies two matrices in multithreading mode
    /// </summary>
    /// <param name="a">First matrix</param>
    /// <param name="b">Second matrix</param>
    /// <param name="threadCount">Count of using threads</param>
    /// <returns>Multiplication result</returns>
    static public Matrix MultThread(Matrix a, Matrix b, int threadCount)
    {
        if (threadCount < 0 || threadCount > a.Height)
        {
            Console.WriteLine("Thread count clipped according to matrix size");
            threadCount = a.Height;
        }
        if (a.Width != b.Height)
        {
            Console.WriteLine("Incorrect matrix sizes");
            return null;
        }

        var c = new Matrix(a.Height, b.Width);
        var threads = new Thread[threadCount];
        var threadPiece = a.Height / threadCount + Convert.ToInt32(a.Height % threadCount != 0);

        for (var i = 0; i < threadCount; ++i)
        {
            var localI = i;
            threads[i] = new Thread(() =>
            {
                for (var n = threadPiece * localI; n < threadPiece * (localI + 1) && n < a.Height; ++n)
                {
                    for (var j = 0; j < b.Width; ++j)
                    {
                        c.matrix[n, j] = 0;
                        for (var k = 0; k < a.Width; ++k)
                            c.matrix[n, j] += a.matrix[n, k] * b.matrix[k, j];
                    }
                }
            });

        }

        for (var i = 0; i < threads.Length; ++i)
            threads[i].Start();

        for (var i = 0; i < threads.Length; ++i)
            threads[i].Join();

        return c;
    }
}

internal class Program
{
    static void Main()
    {
        var bench = new MultBenchmark();

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


        Console.ReadKey();
    }
}
