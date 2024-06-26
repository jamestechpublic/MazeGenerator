﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MazeGenerator.AppLogic;

namespace MazeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int COLS = 20;
        const int ROWS = 20;
        const int SCALE = 16; // pixel width and height of block

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonGenerate_Click(object sender, RoutedEventArgs e)
        {
            var maze = MazeGen2.Generate(COLS, ROWS);
            DrawMaze(maze, SCALE);
        }

        private void DrawMaze(WallState[,] maze, int scale)
        {
            myCanvas.Children.Clear();
            for (int y = 0; y < 20; y++)
                for (int x = 0; x < 20; x++)
                {
                    if (maze[x, y].HasFlag(WallState.UP)) DrawWall(x, y, WallState.UP, scale);
                    if (maze[x, y].HasFlag(WallState.LEFT)) DrawWall(x, y, WallState.LEFT, scale);
                    // optimized to only draw on last col and first row (canvas 0,0 is bottom left)
                    if (y == 0 && maze[x, y].HasFlag(WallState.DOWN)) DrawWall(x, y, WallState.DOWN, scale);
                    if (x == 19 && maze[x, y].HasFlag(WallState.RIGHT)) DrawWall(x, y, WallState.RIGHT, scale);
                }
        }

        private void DrawWall(int x, int y, WallState wallState, int scale)
        {
            x = x * scale + 20; y = y * scale + 20;

            var wall = new Line();
            wall.Stroke = Brushes.Black;
            wall.StrokeThickness = 2;
            
            // Note: maze is flipped horizontally because WPF Canvas 0,0 is bottom left
            int s = scale / 2;
            switch (wallState)
            {
                case WallState.DOWN:
                    wall.X1 = x - s;
                    wall.X2 = x + s;
                    wall.Y1 = y - s;
                    wall.Y2 = y - s;
                    break;
                case WallState.UP:
                    wall.X1 = x - s;
                    wall.X2 = x + s;
                    wall.Y1 = y + s;
                    wall.Y2 = y + s;
                    break;
                case WallState.LEFT:
                    wall.X1 = x - s;
                    wall.X2 = x - s;
                    wall.Y1 = y - s;
                    wall.Y2 = y + s;
                    break;
                case WallState.RIGHT:
                    wall.X1 = x + s;
                    wall.X2 = x + s;
                    wall.Y1 = y - s;
                    wall.Y2 = y + s;
                    break;
            }

            
            myCanvas.Children.Add(wall);
        }
    }
}
