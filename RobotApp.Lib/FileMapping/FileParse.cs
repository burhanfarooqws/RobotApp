using RobotApp.Lib.Enums;
using RobotApp.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RobotApp.Lib.FileMapping
{
    public class FileParse
    {
        public FileParse()
        {
            FileContent = string.Empty;
            GridColumnLength = 0;
            GridRowLength = 0;
            ListObstacleMap = new List<string>();
            ListRawRouteMap = new List<List<string>>();
        }
        public string FileContent { get; set; }
        public string[] FileContentArray { get { return FileContent.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None); }}
        public int GridColumnLength { get; set; }
        public int GridRowLength { get; set; }
        public List<string> ListObstacleMap { get; set; }
        public List<List<string>> ListRawRouteMap { get; set; }
        public void Process()
        {
            if(FileContentArray.Length > 0)
            {
                var arrGridDimension = Regex.Split(FileContentArray[0], @"\D+");
                if(arrGridDimension.Length > 0 && arrGridDimension.Length == 3)
                {
                    //GridColumnLength = int.Parse(arrGridDimension[1]);
                    //GridRowLength = int.Parse(arrGridDimension[2]);
                    int.TryParse(arrGridDimension[1], out int column);
                    GridColumnLength = column;
                    int.TryParse(arrGridDimension[2], out int row);
                    GridRowLength = row;
                }
            }
            
            var listEachRouteMap = new List<string>();

            foreach (string line in FileContentArray)
            {
                if (string.IsNullOrEmpty(line) || line.ToLower().Contains("grid") || line.ToLower().Contains("obstacle")) // ignoring 1st & empty lines
                {
                    ListRawRouteMap.Add(listEachRouteMap);
                    listEachRouteMap = new List<string>();
                    if (line.ToLower().Contains("obstacle")) // getting obstacle lines
                    {
                        ListObstacleMap.Add(line);
                    }
                }
                else
                {
                    listEachRouteMap.Add(line);
                }
            }

            ListRawRouteMap.Add(listEachRouteMap);
        }

        
    }
}
