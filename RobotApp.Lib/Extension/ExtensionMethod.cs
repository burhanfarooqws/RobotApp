using RobotApp.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.Lib.Extension
{
    public static class ExtensionMethod
    {
        public static bool IsObstacleCollided(this Position position, List<Obstacle> obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.Column == position.Column && obstacle.Row == position.Row)
                {
                    return true;
                }
            }

            return false;
        }

        public static string CrashMessage(this Position position)
        {
            return string.Format("CRASHED {0} {1}", position.Column, position.Row);
        }
    }
}
