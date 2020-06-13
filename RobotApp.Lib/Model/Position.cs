using RobotApp.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Lib.Model
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Direction Pointing { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.Column, this.Row, this.Pointing.ToString());
            //return base.ToString();
        }
    }
}
