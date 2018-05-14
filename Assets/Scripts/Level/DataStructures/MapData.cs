using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnotherTankGame{
    public class MapData{
        private QuadCell[,] _qaadCellsMap;
        private Cell[,] _cellsMap;

        public MapData(Texture2D texture) {
            PopulateQuadMap(texture);
        }

        private void PopulateQuadMap(Texture2D texture) {
            if (texture != null) {
                int yLim = texture.height / 2;
                int xLim = texture.width / 2;
                _qaadCellsMap = new QuadCell[xLim, yLim];
                _cellsMap = new Cell[texture.width, texture.height];

                for (var y = 0; y < texture.height; y += 2) {
                    for (var x = 0; x < texture.width; x += 2) {
                        QuadCell quadCell = new QuadCell();

                        Color pixelColor = texture.GetPixel(x, y);

                        var cell = CreateCellByColor(pixelColor, x, y);

                        quadCell.AddCell(cell);
                        _cellsMap[x, y] = cell;

                        pixelColor = texture.GetPixel(x + 1, y);
                        cell = CreateCellByColor(pixelColor, x + 1, y);
                        quadCell.AddCell(cell);
                        _cellsMap[x + 1, y] = cell;


                        pixelColor = texture.GetPixel(x, y + 1);
                        cell = CreateCellByColor(pixelColor, x, y + 1);
                        quadCell.AddCell(cell);
                        _cellsMap[x, y + 1] = cell;

                        pixelColor = texture.GetPixel(x + 1, y + 1);
                        cell = CreateCellByColor(pixelColor, x + 1, y + 1);
                        quadCell.AddCell(cell);
                        _cellsMap[x + 1, y + 1] = cell;

                        _qaadCellsMap[x / 2, y / 2] = quadCell;
                    }
                }
            }
        }

        private Cell CreateCellByColor(Color color, int x, int y) {
            Cell cell = null;
            if (color == Color.red) {
                cell = new Cell(BlockType.Brick, x, y);
            }
            else if (color == Color.white) {
                cell = new Cell(BlockType.Steel, x, y);
            }
            else if (color == Color.black || color == Color.yellow || color == Color.green) {
                cell = new Cell(BlockType.Empty, x, y);
            }

            return cell;
        }

        public bool CheckForQuadCellIsEmpty(int x, int y) {
            if (x < 0 || y < 0 || x >= 13 || y >= 13) return false;
            return _qaadCellsMap[x, y].IsEmpty();
        }

        public void SetCellToEmpty(int x, int y) {
            _cellsMap[x, y].Block = BlockType.Empty;
        }
    }
}