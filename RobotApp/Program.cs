using RobotApp.Lib.FileMapping;
using RobotApp.Lib.Parser;
using System;
using System.IO;

namespace RobotApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var fileFound = File.Exists(args[0]);
                if (fileFound)
                {
                    var fileName = args[0];
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Extension == ".txt")
                    {
                        if (fi.Length > 0)
                        {
                            Start(File.ReadAllText(fileName));
                        }
                        else
                        {
                            Console.WriteLine("File size is 0 bytes");
                        }
                    }
                    else
                    {
                        Console.WriteLine("File extension should be [.txt]");
                    }
                }
                else
                {
                    Console.WriteLine("File not found");
                }
            }
            else
            {
                Console.WriteLine("Empty input parameters");
            }            

            Console.ReadLine();
        }

        private static void Start(string fileContent)
        {
            //var fileContent = Properties.Resources.Sample;
            FileParse fileParse = new FileParse();
            Parse parse = new Parse();
            fileParse.FileContent = fileContent;

            var lastObstacle = Array.FindLastIndex(fileParse.FileContentArray, x => x.ToLower().Contains("obstacle"));
            var firstJourney = Array.FindIndex(fileParse.FileContentArray, s => !(string.IsNullOrEmpty(s) || s.ToLower().Contains("grid") || s.ToLower().Contains("obstacle")));

            if(lastObstacle > firstJourney)
            {
                Console.WriteLine("Can't process Journey invalid obstacle location");
                return;
            }
            
            fileParse.Process();

            parse.GetObstacles(fileParse.ListObstacleMap);
            parse.GetRouteMap(fileParse.ListRawRouteMap);

            foreach (var routeMap in parse.ListRouteMap)
            {
                var result = parse.ParseRouteMap(routeMap, fileParse.GridColumnLength, fileParse.GridRowLength);
                Console.WriteLine(result.JourneyResult);
                foreach (var obstacleResult in result.ObstacleResult)
                {
                    Console.WriteLine(obstacleResult);
                }
            }
        }

    }        
}