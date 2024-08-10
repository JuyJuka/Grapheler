namespace Grapheler
{
  using System;
  using System.Diagnostics;
  using System.Threading;

  internal class Program
  {
    private const int colums = 3;

    static void Main(string[] args)
    {
      Stopwatch sw = Stopwatch.StartNew();
      int tries = 1000000000;
      int target = 177;
      int threads = 100;
      int[] max = new int[threads];
      string[] console = new string[threads];
      Thread[] t = new Thread[threads];
      Thread d = new Thread(() =>
      {
        while (t != null)
        {
          Draw(console, false);
          Thread.Sleep(1000);
        }
      });
      d.Start();
      for (int i = 0; i < threads; i++)
        t[i] = ToThread(target, tries / threads, i, max, console);
      for (int i = 0; i < threads; i++)
        t[i].Join();
      t = null;
      int all = 0;
      for (int i = 0; i < threads; i++)
        if (all < max[i])
          all = max[i];
      d.Join();
      sw.Stop();
      Draw(console, false);
      Console.WriteLine(string.Format("Final Max Result: {0}", all) + new string(' ', console[0].Length * colums));
      Console.WriteLine(string.Format("Elapsed Time: {0}", sw.Elapsed));
      Console.ReadLine();
    }

    public static void Draw(string[] console, bool pluseOne)
    {
      Console.SetCursorPosition(0, pluseOne ? 1 : 0);
      int offset = (int)Math.Floor((double)(console.Length / colums));
      for (int i = 0; i < offset; i++)
      {
        string line = string.Empty;
        for (int j = 0; j < colums; j++)
        {
          line = line + ((i + (offset * j) < console.Length) ? console[i + (offset * j)] : string.Empty) + "\t";
        }
        Console.WriteLine(line);
      }
      for (int i = (offset * colums); i < console.Length; i++)
        Console.WriteLine(console[i]);
    }

    public static Thread ToThread(int target, int tries, int index, int[] maxes, string[] console)
    {
      Random r = new Random();
      int max = 0;
      int success = 1;
      Thread re = new Thread(() =>
      {
        while (tries > 0)
        {
          int run = 0;
          int next = success;
          while (run < target && next == success) if ((next = r.Next(1, 4)) == success) run++;
          if (run > max) max = run;
          tries--;
          if (tries % 10 == 0 || tries <= 0)
          {
            console[index] = string.Format("Thread {0:000} Remaining {1:0000000} Cur/Max {3}/{2}", index + (int)decimal.One, tries, max, run);
          }
        }
        maxes[index] = max;
      });
      re.Start();
      return re;
    }
  }
}
