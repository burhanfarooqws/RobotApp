using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Lib.Model
{
    public class RouteMap
    {
        public Position StartPosition { get; set; }
        public string[] Commands { get; set; }//Each character is a command, either to turn (L = left, R = right) or to move forwards (F).
        public Position EndPosition { get; set; }
    }
}
