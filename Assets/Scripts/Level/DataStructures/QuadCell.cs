using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnotherTankGame{
    public class QuadCell{
        private Cell[,] _cells;
        private List<Cell> _cellList;

        public QuadCell() {
            _cells = new Cell[2, 2];
            _cellList = new List<Cell>();
        }

        public void AddCell(Cell cell) {
            _cellList.Add(cell);
            switch (_cells.Length) {
                case 0:
                    _cells[0, 0] = cell;
                    break;
                case 1:
                    _cells[0, 1] = cell;
                    break;
                case 2:
                    _cells[1, 0] = cell;
                    break;
                case 3:
                    _cells[1, 1] = cell;
                    break;
            }
        }

        public bool IsEmpty() {
            bool isEmpty = true;
            foreach (Cell cell in _cellList) {
                if (cell.Block != BlockType.Empty) {
                    isEmpty = false;
                }
            }

            return isEmpty;
        }
    }
}