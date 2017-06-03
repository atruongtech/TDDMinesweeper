using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Library
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public class DifficultySetting
    {
        public int Mines;
        public int Rows;
        public int Columns;

        public DifficultySetting(DifficultyLevel level)
        {
            switch (level)
            {
                case DifficultyLevel.Easy:
                    Mines = 10;
                    Rows = 8;
                    Columns = 8;
                    break;
                case DifficultyLevel.Medium:
                    Mines = 40;
                    Rows = 16;
                    Columns = 16;
                    break;
                case DifficultyLevel.Hard:
                    Mines = 99;
                    Rows = 24;
                    Columns = 24;
                    break;
            }
        }
    }
}
