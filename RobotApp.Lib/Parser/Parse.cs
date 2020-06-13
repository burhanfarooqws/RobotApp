using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using RobotApp.Lib.Model;
using RobotApp.Lib.Enums;
using RobotApp.Lib.Extension;

namespace RobotApp.Lib.Parser
{
    public class Parse
    {
        public Parse()
        {
            ListRouteMap = new List<RouteMap>();
            ListObstacle = new List<Obstacle>();
        }
        //var listRouteMap = new List<RouteMap>();
        public List<RouteMap> ListRouteMap { get; set; }
        //var listObstacle = new List<Obstacle>();
        public List<Obstacle> ListObstacle { get; set; }
        public ParseResult ParseRouteMap(RouteMap routeMap, int gridCol, int gridRow)
        {
            var result = string.Empty;
            var listObstacleResult = new List<string>();
            Position CurrentPosition = routeMap.StartPosition;

            if (ListObstacle.Count > 0) // checking for obstacle might be startpoint already on obstacle
            {
                if (CurrentPosition.IsObstacleCollided(ListObstacle))
                {
                    listObstacleResult.Add(CurrentPosition.CrashMessage());
                }
            }

            foreach (var c in routeMap.Commands)
            {
                Enum.TryParse(c.ToUpper(), out Command _command);
                // if its a turn command
                if (_command == Command.L || _command == Command.R)
                {
                    CurrentPosition = GetNewPositionOnDirectionChange(CurrentPosition, _command);
                }

                // if its a move forward command
                if (_command == Command.F)
                {
                    CurrentPosition = GetNewPositionOnLocationChange(CurrentPosition, _command);
                    if (ListObstacle.Count > 0)
                    {
                        if (CurrentPosition.IsObstacleCollided(ListObstacle))
                        {
                            listObstacleResult.Add(CurrentPosition.CrashMessage());
                        }
                    }
                    // only validating grid boundary on coordinate change not on turn
                    if (!ValidatePositionInsideGrid(gridCol, gridRow, CurrentPosition.Column, CurrentPosition.Row))
                    {
                        result = "OUT OF BOUNDS";
                        break;
                    }
                }
                result = CurrentPosition.ToString() == routeMap.EndPosition.ToString() ? "SUCCESS " + CurrentPosition.ToString() : "FAILURE " + CurrentPosition.ToString();
            }
            return new ParseResult() { JourneyResult = result, ObstacleResult = listObstacleResult };
        }                       
        public bool ValidatePositionInsideGrid(int ColumnLength, int RowLength, int CurrentColumn, int CurrentRow)
        {
            int ColLowerBound = 0;
            int ColUpperBound = ColumnLength - 1;
            int RowLowerBound = 0;
            int RowUpperBound = RowLength - 1;

            if (CurrentColumn < ColLowerBound || CurrentColumn > ColUpperBound) return false;
            if (CurrentRow < RowLowerBound || CurrentRow > RowUpperBound) return false;

            return true;
        }
        public Position GetNewPositionOnDirectionChange(Position position, Command command)
        {
            switch (position.Pointing)
            {
                case Direction.N:
                    switch (command)
                    {
                        case Command.L:
                            position.Pointing = Direction.W;
                            break;
                        case Command.R:
                            position.Pointing = Direction.E;
                            break;
                        default:
                            break;
                    }
                    break;
                case Direction.S:
                    switch (command)
                    {
                        case Command.L:
                            position.Pointing = Direction.E;
                            break;
                        case Command.R:
                            position.Pointing = Direction.W;
                            break;
                        default:
                            break;
                    }
                    break;
                case Direction.E:
                    switch (command)
                    {
                        case Command.L:
                            position.Pointing = Direction.N;
                            break;
                        case Command.R:
                            position.Pointing = Direction.S;
                            break;
                        default:
                            break;
                    }
                    break;
                case Direction.W:
                    switch (command)
                    {
                        case Command.L:
                            position.Pointing = Direction.S;
                            break;
                        case Command.R:
                            position.Pointing = Direction.N;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return position;
        }
        public Position GetNewPositionOnLocationChange(Position position, Command command)
        {
            switch (command)
            {
                case Command.F:
                    switch (position.Pointing)
                    {
                        case Direction.N:
                            position.Row += 1;
                            break;
                        case Direction.S:
                            position.Row -= 1;
                            break;
                        case Direction.E:
                            position.Column += 1;
                            break;
                        case Direction.W:
                            position.Column -= 1;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return position;
        }
        public void GetObstacles(List<string> listObstacleMap)
        {
            if (listObstacleMap.Count > 0)
            {
                foreach (var _obstacle in listObstacleMap)
                {
                    var _deflate = _obstacle.Split(' ');
                    var obstacle = new Obstacle()
                    {
                        Column = int.Parse(_deflate[1]), // 1st index is column
                        Row = int.Parse(_deflate[2]), // 2nd index is row
                    };
                    ListObstacle.Add(obstacle);
                }
            }
        }
        public void GetRouteMap(List<List<string>> listRawRouteMap)
        {
            foreach (var rawRouteMapList in listRawRouteMap)
            {
                if (rawRouteMapList != null && rawRouteMapList.Count > 0)
                {
                    var routeMap = new RouteMap();
                    if (rawRouteMapList != null && rawRouteMapList.Count > 0)
                    {
                        routeMap.StartPosition = GetPosition(rawRouteMapList[0]);
                        routeMap.Commands = rawRouteMapList[1].ToArray().Select(c => c.ToString()).ToArray();
                        routeMap.EndPosition = GetPosition(rawRouteMapList[2]);
                    }
                    ListRouteMap.Add(routeMap);
                }
            }
        }
        public Position GetPosition(string value)
        {
            var _deflate = value.Split(' ');
            Enum.TryParse(_deflate[2], out Direction _direction);
            var position = new Position()
            {
                Column = int.Parse(_deflate[0]), // 1st index is column
                Row = int.Parse(_deflate[1]), // 2nd index is row
                Pointing = _direction
            };

            return position;
        }
    }
}
