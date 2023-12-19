using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinefieldGame
{
    public class MineField
    {

       private Size big;
       private List<Mine> mines;
       private readonly int fullmine = 10;
       private readonly Random rnd = new Random();


        public MineField(Size greatness, int minenumber)
        {
            mines = new List<Mine>();
            fullmine = minenumber;
            big = greatness;

            for (int x = 0; x < greatness.Width; x = x + 20)
            {
                for (int y = 0; y < greatness.Height; y = y + 20)
                {
                    Mine m = new Mine(new Point(x, y));
                    AddMine(m);

                }

            }
            FillMine();

        }

        public void AddMine(Mine m)
        {
            mines.Add(m);
        }

        private void FillMine()
        {
            AddMine(new Mine(new Point()));
            int number = 0;
            while (number < fullmine)
            {
                int i = rnd.Next(0, mines.Count);
                Mine item = mines[i];
                if (item.AnyMines == false)
                {
                    item.AnyMines = true;
                    number++;
                }
            }


        }
        public Size HeightPanel
        {
            get
            {
                return big;
            }
        }
        //1
        public Mine ShowLocMine(Point loc)
        {
            foreach (Mine item in mines)
            {
                if (item.GetLocation == loc)
                {
                    return item;

                }

            }
            return null;




        }
        //2
        public List<Mine> gettallMayin
        {
            get { return mines; }

        }

       
    }
}
