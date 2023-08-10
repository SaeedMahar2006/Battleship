using System;
using System.Data;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            Battleship b=new Battleship();
            Matrix m =new Matrix(10,10);
            //b.plot(5, 5, 3, 5, m);
            //m.Print();
            b.GenerateBoard(m, b.ships);
            Console.ReadLine();
        }
    }
    public class Battleship
    {
        Random rnd = new Random();
        Matrix board1 = new Matrix(10,10);
        Matrix board2 = new Matrix(10,10);
        //0 empty
        //1 ship
        //-1 shot square (ship or not)
        //0.5 space surrounding ship (cant place ship in here)

        public List<int> ships = new List<int>() {5,4,4,3,3,2,2,2,2,2}; //descending order more efficient

        public Battleship()
        {
            Clear();
            List<int> shipss = new List<int>();
            foreach(int i in ships) { shipss.Add(i); };
            List<int> shipsss = new List<int>();
            foreach (int i in ships) { shipsss.Add(i); };
            board1 = Board(board1,shipss);
            board2 = Board(board2,shipsss);
            Console.Clear();
        }

        void Clear()
        {
            board1 = new Matrix(10, 10);
            board2 = new Matrix(10, 10);
        }

        void PlayTurn()
        {

        }

        public void plot(int x1,int y1, int dir, int len, Matrix m)
        {
            switch (dir) {
                case 0: //up
                    for (int i=y1;i<y1+len;i++)
                    {
                        m[x1, i] = 1; //all validation shouldve happened up to here
                        //grey
                        try { m[x1-1, i] = 0.5; } catch { }
                        try { m[x1+1, i] = 0.5; } catch { }
                    }
                    try { m[x1 - 1, y1-1] = 0.5; } catch { }
                    try { m[x1 + 1, y1-1] = 0.5; } catch { }
                    try { m[x1 - 1, y1+len] = 0.5; } catch { }
                    try { m[x1 + 1, y1 +len] = 0.5; } catch { }
                    try { m[x1 , y1 - 1] = 0.5; } catch { }
                    try { m[x1 , y1 + len] = 0.5; } catch { }
                    break;

                case 1: //right
                    for (int i = x1; i < x1 + len; i++)
                    {
                        m[i, y1] = 1; //all validation shouldve happened up to here
                        //grey
                        try { m[i,y1+1] = 0.5; } catch { }
                        try { m[i, y1-1] = 0.5; } catch { }
                    }
                    try { m[x1 - 1, y1 - 1] = 0.5; } catch { }
                    try { m[x1 - 1, y1 + 1] = 0.5; } catch { }
                    try { m[x1 +len, y1+1] = 0.5; } catch { }
                    try { m[x1 + len, y1-1] = 0.5; } catch { }
                    try { m[x1-1, y1] = 0.5; } catch { }
                    try { m[x1 + len, y1 ] = 0.5; } catch { }
                    break;

                case 2: //down
                    for (int i = y1; i > y1 - len; i--)
                    {
                        m[x1, i] = 1; //all validation shouldve happened up to here
                        //grey
                        try { m[x1 - 1, i] = 0.5; } catch { }
                        try { m[x1 + 1, i] = 0.5; } catch { }
                    }
                    try { m[x1 - 1, y1 + 1] = 0.5; } catch { }
                    try { m[x1 + 1, y1 + 1] = 0.5; } catch { }
                    try { m[x1 - 1, y1 - len ] = 0.5; } catch { }
                    try { m[x1 + 1, y1 - len ] = 0.5; } catch { }
                    try { m[x1, y1 + 1] = 0.5; } catch { }
                    try { m[x1, y1 - len] = 0.5; } catch { }
                    break;

                case 3: //left
                    for (int i = x1; i > x1 - len; i--)
                    {
                        m[i, y1] = 1; //all validation shouldve happened up to here
                        //grey
                        try { m[i, y1 + 1] = 0.5; } catch { }
                        try { m[i, y1 - 1] = 0.5; } catch { }
                    }
                    try { m[x1 + 1, y1 - 1] = 0.5; } catch { }
                    try { m[x1 + 1, y1 + 1] = 0.5; } catch { }
                    try { m[x1 - len, y1 + 1] = 0.5; } catch { }
                    try { m[x1 - len, y1 - 1] = 0.5; } catch { }
                    try { m[x1 + 1, y1] = 0.5; } catch { }
                    try { m[x1 - len, y1] = 0.5; } catch { }
                    break;

            }
        }
        public Matrix Board(Matrix m, List<int> ships, int depth = 1000)
        {
            Matrix board=new Matrix(10,10);
            bool status = false;
            int counter = 0;
            while (!status)
            {
                counter++;
                if (counter > depth) throw new Exception("Unlucky, mate");
                (status, board) = GenerateBoard(m,ships);
            }
            return board;
        }
        public (bool, Matrix) GenerateBoard(Matrix m, List<int> ships, int depth=0)
        {
            depth++;
            if (ships.Count == 0) return (true, m);
            Matrix M = new Matrix(10, 10);
            m.Copy_To(M);

            int ship = ships.First();
            ships.Remove(ship);

            int x, y;
            do
            {
                x = rnd.Next(10);  //between 0 and 9
                y = rnd.Next(10);  //between 0 and 9
            } while (m[x, y] != 0);
            //available square x,y
            //4 directions

            bool gen_status = false;
            Matrix gen_matrix;
            //TODO in the future consider a proper DFS, randomise orientations

                bool av = true;
                for (int i = 0; i < ship; i++)
                {
                    try
                    {
                        if (m[x + i, y] != 0)
                        {
                            av = false; break;
                        }
                    }
                    catch (Exception e) { av = false; }
                }
                if (av)
                {//done probing in this direction
                    plot(x, y, 1, ship, M);
                    Console.WriteLine($"DEPTH {depth}");
                    M.Print();
                    (gen_status, gen_matrix)= GenerateBoard(M, ships, depth);
                    return (gen_status, gen_matrix);
                }
                av = true;


                for (int i = 0; i < ship; i++)
                {
                    try
                    {
                        if (m[x, y + i] != 0)
                        {
                            av = false; break;
                        }
                    }
                    catch (Exception e) { av = false; }
                }
                if (av)
                {//done probing in this direction
                    plot(x, y, 0, ship, M);
                    Console.WriteLine($"DEPTH {depth}");
                    M.Print();
                    (gen_status, gen_matrix) = GenerateBoard(M, ships, depth);
                    return (gen_status, gen_matrix);
                }
                av = true;



                for (int i = 0; i < ship; i++)
                {
                    try
                    {
                        if (m[x - i, y] != 0)
                        {
                            av = false; break;
                        }
                    }
                    catch (Exception e) { av = false; }
                }
                if (av)
                {//done probing in this direction
                    plot(x, y, 3, ship, M);
                    Console.WriteLine($"DEPTH {depth}");
                    M.Print();
                    (gen_status, gen_matrix) = GenerateBoard(M, ships, depth);
                    return (gen_status, gen_matrix);
                }
                av = true;


                for (int i = 0; i < ship; i++)
                {
                    try
                    {
                        if (m[x, y - i] != 0)
                        {
                            av = false; break;
                        }
                    }
                    catch (Exception e) { av = false; }
                }
                if (av)
                {//done probing in this direction
                    plot(x, y, 2, ship, M);
                    Console.WriteLine($"DEPTH {depth}");
                    M.Print();
                    (gen_status, gen_matrix) = GenerateBoard(M, ships, depth);
                    return (gen_status, gen_matrix);
                }

                av = true;
            
            Console.WriteLine($"DEPTH {depth} and FAILED");
            M.Print();
            return (false, M);
        }
    }
}