using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinefieldGame
{
    public class Mine
    {
        private Point loc;
        private bool hail;
        private bool check;

        public Mine(Point location)
        {
            hail = false;
            loc = location;
        }

        //methoda çevir
        public Point GetLocation
        {
            get { return loc; }
        }
       public bool AnyMines
        {
            get
            {
                return hail;
            }
            set
            {
                hail = value;
            }

        }

        public bool Looked
        {
            get
            {
                return check;
            }
            set
            {
                check = value;
            }

        }



    }

}
