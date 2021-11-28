using System;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateMap();

            previousButton = null;

            foreach (UIElement c in chessBoard.Children)
            {
                if (c is Button)
                {
                    ((Button)c).Click += Pressed;
                }
            }
        }

        public class NewButton : Button
        {
            public int Side { get; set; }
            public int Turn = 0;
            public int Type { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool Potential(int x2, int y2, int s)
            {
                switch (Type)
                {
                    case 1: //pawn
                        {
                            int temp = 1;
                            if (Side == 0)
                                temp = -1;
                            if (Turn == 0)
                            {
                                if ((x2 == X + temp || (x2 == X + 2 * temp && buttonMap[X + temp, Y].Type == 0)) && Y == y2 && s == 2 || (s != Side && s != 2 && x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1)))
                                    return true;
                                else
                                    return false;
                            }
                            else
                            {
                                if ((x2 == X + temp && Y == y2 && s == 2) || (s != Side && s != 2 && x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1)))
                                    return true;
                                else
                                    return false;
                            }
                        }
                    case (2): //rook
                        return HorizontalVertical(X, Y, x2, y2, Side, true);
                    case (3): //knight
                        {
                            if ((Math.Abs(X - x2) == 2 && Math.Abs(Y - y2) == 1) || (Math.Abs(X - x2) == 1 && Math.Abs(Y - y2) == 2))
                                return true;
                            else
                                return false;
                        }
                    case (4): //bishop
                        return Diagonal(X, Y, x2, y2, Side, true);
                    case (5): //queen
                        return HorizontalVertical(X, Y, x2, y2, Side, true) || Diagonal(X, Y, x2, y2, Side, true);
                    case (6): //king
                        {
                            return HorizontalVertical(X, Y, x2, y2, Side, false) || Diagonal(X, Y, x2, y2, Side, false);
                        }

                    default:
                        return false;
                }
            }
            public NewButton() { }
            public NewButton(int Side, int Type, int X, int Y)
            {
                this.Side = Side;
                this.Type = Type;
                //this.Turn = 0;
                this.X = X;
                this.Y = Y;
                switch (Side, Type)
                {
                    case (1, 1):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\pawnBlack.png"));
                            this.Content = img;
                            break;
                        }
                    case (1, 2):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\rookBlack.png"));
                            this.Content = img;
                            break;
                        }
                    case (1, 3):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\knightBlack.png"));
                            this.Content = img;
                            break;
                        }
                    case (1, 4):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\bishopBlack.png"));
                            this.Content = img;
                            break;
                        }
                    case (1, 5):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\queenBlack.png"));
                            this.Content = img;
                            break;
                        }
                    case (1, 6):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\kingBlack.png"));
                            this.Content = img;
                            break;
                        }
                    case (0, 1):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\pawnWhite.png"));
                            this.Content = img;
                            break;
                        }
                    case (0, 2):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\rookWhite.png"));
                            this.Content = img;
                            break;
                        }
                    case (0, 3):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\knightWhite.png"));
                            this.Content = img;
                            break;
                        }
                    case (0, 4):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\bishopWhite.png"));
                            this.Content = img;
                            break;
                        }
                    case (0, 5):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\queenWhite.png"));
                            this.Content = img;
                            break;
                        }
                    case (0, 6):
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\kingWhite.png"));
                            this.Content = img;
                            break;
                        }
                }
            }
        }

        readonly int[,] map = new int[8, 8]
        {
           {12,13,14,15,16,14,13,12},
           {11,11,11,11,11,11,11,11},
           {20,20,20,20,20,20,20,20 },
           {20,20,20,20,20,20,20,20 },
           {20,20,20,20,20,20,20,20 },
           {20,20,20,20,20,20,20,20 },
           {1,1,1,1,1,1,1,1},
           {2,3,4,5,6,4,3,2}
        };

        public static NewButton[,] buttonMap = new NewButton[8, 8];

        public NewButton previousButton = new NewButton();

        public int turnNumber = 0;

        public string[] sideArray = new string[2] {"White", "Black" };

        public void CreateMap()
        {
            char[] alpha = "ABCDEFGH".ToCharArray();

            for (int i = 0; i < 8; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = (8 - i).ToString();
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                chessNumber1.Children.Add(textBlock);

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = alpha[i].ToString();
                //textBlock1.Text = (string)("A" + i);
                textBlock1.TextAlignment = TextAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                chessLetter1.Children.Add(textBlock1);
            }

            for (int i = 0; i < 8; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = (8 - i).ToString();
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                chessNumber2.Children.Add(textBlock);

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = alpha[i].ToString();
                textBlock1.TextAlignment = TextAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                chessLetter2.Children.Add(textBlock1);
            }

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    buttonMap[i, j] = new NewButton(map[i, j] / 10, map[i, j] % 10, i, j);

                    buttonMap[i, j].Background = null;
                    buttonMap[i, j].BorderBrush = Brushes.Black;

                    Rectangle rect = new Rectangle();
                    if ((i + j) % 2 == 0)
                        rect.Fill = Brushes.Peru;
                    else
                        rect.Fill = Brushes.Sienna;
                    chessBoard.Children.Add(buttonMap[i, j]);
                    chessBoardBackground.Children.Add(rect);
                }

            TurnTextBox.Text = (turnNumber + 1).ToString();
            SideTextBox.Text = sideArray[turnNumber % 2];
        }

        public void Pressed(object sender, RoutedEventArgs e)
        {
            NewButton pressedButton = (NewButton)sender;
            if (pressedButton.Content != null && pressedButton.Side == turnNumber % 2)
            {
                if (pressedButton.Content != null && previousButton != null)
                {
                    previousButton.Effect = null;
                    previousButton.BorderBrush = Brushes.Black;
                }
                BlurEffect blur = new BlurEffect();
                blur.Radius = 1;
                pressedButton.Effect = blur;
                pressedButton.BorderBrush = Brushes.Red;
                previousButton = pressedButton;
            }
            else if (pressedButton.Content == null && previousButton != null || pressedButton.Side != previousButton.Side) //********************************//
                if (previousButton.Potential(pressedButton.X, pressedButton.Y, pressedButton.Side))
                    Turn(previousButton, pressedButton);
        }

        public void Turn(NewButton button1, NewButton button2)
        {
            if (button2.Type == 6)
                MessageBox.Show("Win");
            button2.Content = button1.Content;
            button2.Type = button1.Type;
            button2.Side = button1.Side;
            button2.Turn = button1.Turn;
            if (button2.Turn == 0)
                button2.Turn = 1;
            button1.Content = null;
            button1.Type = 0;
            button1.Side = 2;

            turnNumber++;
            TurnTextBox.Text = (turnNumber + 1).ToString();
            SideTextBox.Text = sideArray[turnNumber % 2];
        }

        public static bool HorizontalVertical(int x1, int y1, int x2, int y2, int s, bool check)
        {
            if (x1 == x2 || y1 == y2)
            {
                if(x1 == x2)
                {
                    int signY = Math.Sign(y1 - y2), y = y1 - signY;
                    if (check)
                        while (y != y2 && buttonMap[x1, y].Type == 0)
                            y -= signY;
                    if (buttonMap[x1, y].Side != s && y == y2)
                        return true;
                    else
                        return false;
                }
                else
                {
                    int signX = Math.Sign(x1 - x2), x = x1 - signX;
                    if (check)
                        while (x != x2 && buttonMap[x, y1].Type == 0)
                            x -= signX;
                    if (buttonMap[x, y1].Side != s && x == x2)
                        return true;
                    else
                        return false;
                }
            }
            else
                return false;
        }

        public static bool Diagonal(int x1, int y1, int x2, int y2, int s, bool check)
        {
            if (Math.Abs(x1 - x2) == Math.Abs(y1 - y2))
            {
                int signX = Math.Sign(x1 - x2),
                signY = Math.Sign(y1 - y2),
                x = x1 - signX, y = y1 - signY;
                if (check)
                    while (x != x2 && y != y2 && buttonMap[x, y].Type == 0)
                    {
                        x -= signX;
                        y -= signY;

                    }

                if (buttonMap[x, y].Side != s && x == x2 && y == y2)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
