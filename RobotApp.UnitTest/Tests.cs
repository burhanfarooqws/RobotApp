using NUnit.Framework;
using RobotApp.Lib.Enums;
using RobotApp.Lib.Extension;
using RobotApp.Lib.FileMapping;
using RobotApp.Lib.Model;
using RobotApp.Lib.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp.UnitTest
{
    [TestFixture]
    public class Tests
    {
        private Position position;
        private List<Obstacle> obstacles;
        private FileParse fileParse;
        private Parse parse;

        [SetUp]
        public void Initialize()
        {
            position = new Position();
            obstacles = new List<Obstacle>();
            fileParse = new FileParse();
            parse = new Parse();
        }

        [TestCase("1")]
        public void Test(string expectedOutput)
        {            
            Assert.AreEqual(expectedOutput, "1");
        }

        #region Extension Method
        [TestCase(false, 1, 1, 0, 0)]
        [TestCase(true, 1, 1, 1, 1)]
        public void Test_IsObstacleCollided(bool expectedOutput, int pColumn, int pRow, int oColumn, int oRow)
        {
            position.Column = pColumn;
            position.Row = pRow;

            obstacles.Add(new Obstacle() { Column = oColumn, Row = oRow });

            Assert.AreEqual(expectedOutput, position.IsObstacleCollided(obstacles));
        }

        [TestCase("CRASHED 3 1", 1, 1)]
        [TestCase("CRASHED 3 1", 3, 1)]
        public void Test_CrashMessage(string expectedOutput, int column, int row)
        {
            position.Column = column;
            position.Row = row;

            Assert.AreEqual(expectedOutput, position.CrashMessage());
        }
        #endregion

        #region FileMapping
        [TestCase("")]
        public void Test_FileParseEmptyContent(string expectedOutput)
        {
            fileParse.Process();

            Assert.AreEqual(expectedOutput, fileParse.FileContent);
        }

        [TestCase(0, 0)]
        public void Test_FileParseZeroColumnRow(int Column, int Row)
        {
            fileParse.Process();

            Assert.AreEqual(Column, fileParse.GridColumnLength);
            Assert.AreEqual(Row, fileParse.GridRowLength);
        }
        #endregion

        #region Parse
        [TestCase("1 1 E", 1, 1, "E")]
        [TestCase("1 1 E", 1, 1, "N")]
        public void Test_ParseGetPosition(string value, int column, int row, string direction)
        {
            Enum.TryParse(direction, out Direction _direction);
            position.Column = column;
            position.Row = row;
            position.Pointing = _direction;

            var output = parse.GetPosition(value);

            Assert.AreEqual(position.Column, output.Column);
            Assert.AreEqual(position.Row, output.Row);
            Assert.AreEqual(position.Pointing, output.Pointing);
        }


        [TestCase("1 1 E", 1, 1, "E", "L")]
        [TestCase("1 1 E", 1, 1, "N", "R")]
        public void Test_ParseGetNewPositionOnDirectionChange(string expectedOutput, int column, int row, string direction, string command)
        {
            Enum.TryParse(direction, out Direction _direction);
            Enum.TryParse(command, out Command _command);

            position.Column = column;
            position.Row = row;
            position.Pointing = _direction;

            var output = parse.GetNewPositionOnDirectionChange(position, _command);

            Assert.AreEqual(position.Column, output.Column);
            Assert.AreEqual(position.Row, output.Row);
            Assert.AreEqual(position.Pointing, output.Pointing);
            Assert.AreEqual(position.ToString(), expectedOutput);
        }

        [TestCase("1 1 E", 1, 1, "E", "F")]
        [TestCase("1 1 E", 1, 1, "N", "F")]
        public void Test_ParseGetNewPositionOnLocationChange(string expectedOutput, int column, int row, string direction, string command)
        {
            Enum.TryParse(direction, out Direction _direction);
            Enum.TryParse(command, out Command _command);

            position.Column = column;
            position.Row = row;
            position.Pointing = _direction;

            var output = parse.GetNewPositionOnLocationChange(position, _command);

            Assert.AreEqual(position.Column, output.Column);
            Assert.AreEqual(position.Row, output.Row);
            Assert.AreEqual(position.Pointing, output.Pointing);
            Assert.AreEqual(position.ToString(), expectedOutput);
        } 
        #endregion
    }
}
