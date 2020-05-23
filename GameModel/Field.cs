using System;
using System.Collections.Generic;
using System.Linq;

namespace Sapper
{
    public class Field
    {
        public readonly Cell[,] Cells;
        public int Count { get; }
        public event EventHandler<ChangeArgs> Change;
        public event EventHandler<EventArgs> Win;
        public event EventHandler<EventArgs> Lose;
        public readonly List<Levels> Levels = new List<Levels>
        {
            new Levels {Name = "Easy", Percent = 10},
            new Levels {Name = "Middle", Percent = 20},
            new Levels {Name = "Hard", Percent = 30}
        };
 
        public Field(int count, int percent)
        {
            Count = count;
            Cells = new Cell[Count, Count];
            var minesCount = Count * Count * percent / 100;
            var r = new Random();
            for (var i = 0; i < minesCount; i++)
            {
                var x = r.Next(Count);
                var y = r.Next(Count);
                if (Cells[x, y].HasMine)
                {
                    i--;
                    continue;
                }
                Cells[x, y].HasMine = true;
            }
        }

        private int MinesAround(int i, int j)
        {
            var res = 0;
            for (var k = 0; k < Count; k++)
            for (var t = 0; t < Count; t++)
                    if (Math.Abs(i - k) <= 1 && Math.Abs(t - j) <= 1 && Cells[k, t].HasMine)
                        res++;
            return res;
        }

        private void OpenLast()
        {
            if (Cells.Cast<Cell>().Any(t => !t.IsOpen && !t.HasMine))
                return;
            if (Win != null) 
                Win(this, new EventArgs());
        }

        public void OpenCell(int i, int j)
        {
            if (Cells[i, j].HasMine)
                if (Lose != null)
                    Lose(this, new EventArgs());
            Cells[i, j].IsOpen = true;
            if (Change != null)
                Change(this, new ChangeArgs {X = i, Y = j, MinArr = Convert.ToString(MinesAround(i, j))});
            if (MinesAround(i, j) == 0)
                for (var k = 0; k < Count; k++)
                for (var p = 0; p < Count; p++)
                    if (Math.Abs(i - k) <= 1 && Math.Abs(p - j) <= 1 && Cells[k, p].IsOpen==false) 
                        OpenCell(k, p);
            OpenLast();
        }
    }
}
