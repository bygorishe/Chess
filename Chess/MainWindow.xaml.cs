using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Math;

namespace Chess
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateMap(); //отрисовка задника
            Start();  //фигуры и прочее
        }
        public enum ChessSide
        {
            White = 0,
            Black,
            NoSide
        }
        public enum ChessType
        {
            Notype = 0, //пустая клетка
            Pawn,  //пешка
            Rook,   //слон
            Knight,  //конь
            Bishop,  //ладбя
            Queen,   //ферзь нарисовался
            King  //король
        }
        public abstract class NewButton : Button
        {
            public ChessSide Side { get; set; }
            public int NumOfFirstTurn = 0;
            public ChessType Type { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public abstract bool Potential(int x2, int y2, ChessSide s);
        }
        public class NoType : NewButton
        {
            public override bool Potential(int x2, int y2, ChessSide s)
            {
                return true;
            }
            public NoType(ChessSide Side, ChessType Type, int X, int Y) {
                this.Side = Side;
                this.Type = Type;
                this.X = X;
                this.Y = Y;
            }
        }
        public class Pawn : NewButton
        {
            public override bool Potential(int x2, int y2, ChessSide s)
            {
                int temp = 1;
                if (Side == 0)  //в зависимости от стороны ход пешки либо вниз либо вверх
                    temp = -1;
                if (NumOfFirstTurn == 0) //первый ход на одну или две клетки        
                    return (x2 == X + temp || (x2 == X + 2 * temp && buttonMap[X + temp, Y].Type == 0)) && Y == y2 && s == ChessSide.NoSide || (s != Side && s != ChessSide.NoSide && x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1));
                //взятие на проходе
                else if (x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1) && s == ChessSide.NoSide &&  //клетка по диагонали свободна
                    (X == 3 || X == 4) &&  //пешка противника сделала ход на две клекти (тут какбы своей X, но как бы да)
                    buttonMap[X, Y + Sign(y2 - Y)].Side != Side && buttonMap[X, Y + Sign(y2 - Y)].NumOfFirstTurn == turnNumber+1) //пешка протиника слева или справа, а также ходила на прошлом ходу
                {
                    //chessBoard.Children.RemovebuttonMap[X, Y + Sign(y2 - Y)]);
                    NoType button = new NoType(ChessSide.NoSide, ChessType.Notype, X, Y + Sign(y2 - Y));
                    button.Background = buttonMap[X, Y + Sign(y2 - Y)].Background;
                    button.BorderBrush = Brushes.Black;
                    Grid.SetRow(button, X);
                    Grid.SetColumn(button, Y + Sign(y2 - Y));
                    passantButton = button;
                    return true;
                }
                else
                    return (x2 == X + temp && Y == y2 && s == ChessSide.NoSide) || (s != Side && s != ChessSide.NoSide && x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1));
            }
            public Pawn(ChessSide Side, ChessType Type, int X, int Y)
            {
                this.Side = Side;
                this.Type = Type;
                this.X = X;
                this.Y = Y;
                Image img = new Image();
                
                //img.Source = new BitmapImage(new Uri("Fig/pawnWhite.png", UriKind.Relative));
                //if (this.Side == ChessSide.Black) //попробовать сделать инверсию цвета

                if (this.Side == ChessSide.Black)
                    img.Source = new BitmapImage(new Uri("Fig/pawnBlack.png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri("Fig/pawnWhite.png", UriKind.Relative));
                Content = img;
            }
        }
        public class Rook : NewButton
        {
            public override bool Potential(int x2, int y2, ChessSide s)
            {
                return HorizontalVertical(X, Y, x2, y2, Side, true);
            }
            public Rook(ChessSide Side, ChessType Type, int X, int Y)
            {
                this.Side = Side;
                this.Type = Type;
                this.X = X;
                this.Y = Y;
                Image img = new Image();
                if (this.Side == ChessSide.Black)
                    img.Source = new BitmapImage(new Uri("Fig/rookBlack.png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri("Fig/rookWhite.png", UriKind.Relative));
                Content = img;
            }
        }
        public class Knight : NewButton
        {
            public override bool Potential(int x2, int y2, ChessSide s)
            {
                return (Abs(X - x2) == 2 && Abs(Y - y2) == 1) || (Abs(X - x2) == 1 && Abs(Y - y2) == 2); //ходы коня "Г" - две в сторону, один вбок  
            }
            public Knight(ChessSide Side, ChessType Type, int X, int Y)
            {
                this.Side = Side;
                this.Type = Type;
                this.X = X;
                this.Y = Y;
                Image img = new Image();
                if (this.Side == ChessSide.Black)
                    img.Source = new BitmapImage(new Uri("Fig/knightBlack.png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri("Fig/knightWhite.png", UriKind.Relative));
                Content = img;
            }
        }
        public class Bishop : NewButton
        {
            public override bool Potential(int x2, int y2, ChessSide s)
            {
                return Diagonal(X, Y, x2, y2, Side, true);
            }
            public Bishop(ChessSide Side, ChessType Type, int X, int Y)
            {
                this.Side = Side;
                this.Type = Type;
                this.X = X;
                this.Y = Y;
                Image img = new Image();
                if (this.Side == ChessSide.Black)
                    img.Source = new BitmapImage(new Uri("Fig/bishopBlack.png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri("Fig/bishopWhite.png", UriKind.Relative));
                Content = img;
            }
        }
        public class Queen : NewButton
        {
            public override bool Potential(int x2, int y2, ChessSide s)
            {
                return HorizontalVertical(X, Y, x2, y2, Side, true) || Diagonal(X, Y, x2, y2, Side, true);
            }
            public Queen(ChessSide Side, ChessType Type, int X, int Y)
            {
                this.Side = Side;
                this.Type = Type;
                this.X = X;
                this.Y = Y;
                Image img = new Image();
                if (this.Side == ChessSide.Black)
                    img.Source = new BitmapImage(new Uri("Fig/queenBlack.png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri("Fig/queenWhite.png", UriKind.Relative));
                Content = img;
            }
        }
        public class King : NewButton
        {
            public override bool Potential(int x2, int y2, ChessSide s)
            {
                return HorizontalVertical(X, Y, x2, y2, Side, false) || Diagonal(X, Y, x2, y2, Side, false); //ходы короля аналогичны ферзю, но на 1 клетку, поэтому false
            }
            public King(ChessSide Side, ChessType Type, int X, int Y)
            {
                this.Side = Side;
                this.Type = Type;
                this.X = X;
                this.Y = Y;
                Image img = new Image();
                if (this.Side == ChessSide.Black)
                    img.Source = new BitmapImage(new Uri("Fig/kingBlack.png", UriKind.Relative));
                else
                    img.Source = new BitmapImage(new Uri("Fig/kingWhite.png", UriKind.Relative));
                Content = img;
            }
        }

        readonly int[,] map = new int[8, 8]  //изначальное расположение фигур 
        {
           {12,13,14,15,16,14,13,12},           // первое число - сторона (1-черные, 2-пустые, 0-белые)
           {11,11,11,11,11,11,11,11},           // второе число - тип  
           {20,20,20,20,20,20,20,20},
           {20,20,20,20,20,20,20,20},
           {20,20,20,20,20,20,20,20},
           {20,20,20,20,20,20,20,20},
           {1,1,1,1,1,1,1,1},
           {2,3,4,5,6,4,3,2}
        };
        //****тестовое для разных ситуаций***//
        //readonly int[,] map = new int[8, 8]  //изначальное расположение фигур 
        //{
        //   {12,13,14,15,16,14,13,20},          // первое число - сторона (1-черные, 2-пустые, 0-белые)
        //   {11,11,11,11,11,11,11,1},           // второе число - тип  
        //   {20,20,20,20,20,20,20,20},
        //   {20,20,20,20,20,20,20,20},
        //   {20,20,1,20,20,1,20,20},
        //   {20,20,20,20,20,20,20,20},
        //   {20,20,20,20,20,20,20,20},
        //   {2,20,20,20,6,20,20,2}
        //};

        public static NewButton[,] buttonMap = new NewButton[8, 8];
        public static NewButton passantButton; //кнопка для рокировки
        public NewButton previousButton,  //запоминаем кнопку для хода     
            transButton,  //для взятия на проходе
            whiteKingPointer, blackKingPointer; //указатели на королей для шаха
        public static int turnNumber = 0;  //тек. ход
        public static bool whiteShah = false, blackShah = false;   //флаг шаха
        public bool themaLight = true; //включена ли исходная цветовая тема 
        public Brush[] brushes = { Brushes.White, Brushes.Black };

        public static bool HorizontalVertical(int x1, int y1, int x2, int y2, ChessSide s, bool check)  //верх низ лево право
        {
            if (x1 == x2) //горизонтальные 
            {
                int signY = Sign(y1 - y2), y = y1 - signY;
                if (check)  //больше одной клетки ход
                    while (y != y2 && buttonMap[x1, y].Type == 0) //если на пути клетки пустые
                        y -= signY;
                return buttonMap[x1, y].Side != s && y == y2;
            }
            else if (y1 == y2) //вертикальные
            {
                int signX = Sign(x1 - x2), x = x1 - signX;
                if (check)
                    while (x != x2 && buttonMap[x, y1].Type == 0)
                        x -= signX;
                return buttonMap[x, y1].Side != s && x == x2;
            }
            else
                return false;
        }

        public static bool Diagonal(int x1, int y1, int x2, int y2, ChessSide s, bool check)
        {
            if (Abs(x1 - x2) == Abs(y1 - y2))  //если клетка находиться на диагонали
            {
                int signX = Sign(x1 - x2),
                signY = Sign(y1 - y2),
                x = x1 - signX, y = y1 - signY;
                if (check) //аналогично с вертикальными и горизонтальными
                    while (x != x2 && y != y2 && buttonMap[x, y].Type == 0)
                    {
                        x -= signX;
                        y -= signY;
                    }
                return buttonMap[x, y].Side != s && x == x2 && y == y2;
            }
            else
                return false;
        }

        public void CreateMap() //отрисовка заднего фона
        {
            for (int i = 0; i < 8; i++)  //цифры буквы на доске
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = (8 - i).ToString();
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                chessNumber1.Children.Add(textBlock);

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = ((char)(65 + i)).ToString();
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
                textBlock1.Text = ((char)(65 + i)).ToString();
                textBlock1.TextAlignment = TextAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                chessLetter2.Children.Add(textBlock1);
            }         
        }

        public void RestartClick(object sender, RoutedEventArgs e) => Start(); //рестарт просто переставляет все фигуры на исходное положение и откатывает текстовые элементы

        public void Start()
        {
            chessBoard.Children.Clear();  //убираем все, зануляем и тд
            previousButton = null;
            turnNumber = 0;
            TextBox1.Text = null;
            TurnTextBox.Text = "1";
            SideTextBox.Text = "White";
            FunImage.Background = Brushes.White;
            for (int i = 0; i < 8; i++)    //создаем кнопки-фигуры
                for (int j = 0; j < 8; j++)
                {
                    switch ((ChessType)(map[i, j] % 10))
                    {
                        case ChessType.Pawn:
                            buttonMap[i, j] = new Pawn((ChessSide)(map[i, j] / 10), ChessType.Pawn, i, j);
                            break;
                        case ChessType.Rook:
                            buttonMap[i, j] = new Rook((ChessSide)(map[i, j] / 10), ChessType.Rook, i, j);
                            break;
                        case ChessType.Knight:
                            buttonMap[i, j] = new Knight((ChessSide)(map[i, j] / 10), ChessType.Knight, i, j);
                            break;
                        case ChessType.Bishop:
                            buttonMap[i, j] = new Bishop((ChessSide)(map[i, j] / 10), ChessType.Bishop, i, j);
                            break;
                        case ChessType.Queen:
                            buttonMap[i, j] = new Queen((ChessSide)(map[i, j] / 10), ChessType.Queen, i, j);
                            break;
                        case ChessType.King:
                            buttonMap[i, j] = new King((ChessSide)(map[i, j] / 10), ChessType.King, i, j);
                            break;
                        default:
                            buttonMap[i, j] = new NoType((ChessSide)(map[i, j] / 10), ChessType.Notype, i, j);
                            break;
                    }
                    buttonMap[i, j].BorderBrush = Brushes.Black;
                    chessBoard.Children.Add(buttonMap[i, j]);
                    Grid.SetColumn(buttonMap[i, j], j);
                    Grid.SetRow(buttonMap[i, j], i);
                }
            whiteKingPointer = buttonMap[7, 4];    //делаем ссылки на королей для отслеживания шаха
            blackKingPointer = buttonMap[0, 4];

            ThemaChange(themaLight);  //раскрашиваем все в соответсвии с выбранной темой

            foreach (UIElement c in chessBoard.Children)
                if (c is NewButton)
                    ((NewButton)c).Click += Pressed;
        }

        public void Pressed(object sender, RoutedEventArgs e) //эвент нажатия на кнопку
        {
            NewButton pressedButton = (NewButton)sender;  //текущая кнопка
            //второе нажатие если рокировка
            if (previousButton != null && previousButton.Side == pressedButton.Side && previousButton.Type == ChessType.King && pressedButton.Type == ChessType.Rook
                && pressedButton.NumOfFirstTurn == 0 && previousButton.NumOfFirstTurn == 0)
            {
                //итак, коротко о рокировке.  нам нужно чтобы все клетки межку королем и ладьей были свободны
                //также, нам нужно чтобы король, а также две клекти(именно две которые займут места король и ладья,
                //даже при длинной рокировке нужны лишь две эти клетки, клетка около ладьи при длинной и сама ладья
                //могу быть связаны боем) от него были недостижимы для противника
                //проблему доставляет то, что пустые клетки ПУСТЫЕ), поэтому мы не сможет узнать наверняка
                //для этого нужно будет временно сделать эти клетки не пустыми(сторону и номер первого хода)
                //далее все это исправиться из-за особенности нашей функции хода
                int tempWay = Sign(pressedButton.Y - previousButton.Y);
                bool castling = true;//возможность рокировки
                string str;
                if (tempWay == 1) //короткая
                {
                    str = "O-O";
                    //подумать над оптимизацией алгоритма
                    for (int j = previousButton.Y + 1; j < pressedButton.Y; j++)
                        if (buttonMap[previousButton.X, j].Side != ChessSide.NoSide)
                            castling = false;
                    if (castling) //еще это дабавил, чтобы если поля не пустые то дальше не идем менять их
                    {
                        for (int i = previousButton.Y; i < pressedButton.Y; i++)
                        {
                            buttonMap[previousButton.X, i].Side = previousButton.Side;
                            if (Shah(buttonMap[previousButton.X, i])) //используем функцию шаха для того чтобы узнать может ли фигура противника на эту клетку
                                castling = false;
                        }
                        //если рокировка не возможна обратно значения полей не меняли//
                        if (!castling)
                            for (int i = previousButton.Y + 1; i < pressedButton.Y; i++)
                                buttonMap[previousButton.X, i].Side = ChessSide.NoSide;
                    }
                }
                else //длинная
                {
                    str = "O-O-O";
                    for (int j = previousButton.Y - 1; j > pressedButton.Y; j--)
                        if (buttonMap[previousButton.X, j].Side != ChessSide.NoSide)
                            castling = false;
                    if (castling)
                    {
                        for (int i = previousButton.Y; i > pressedButton.Y + 1; i--)
                        {
                            buttonMap[previousButton.X, i].Side = previousButton.Side;
                            if (Shah(buttonMap[previousButton.X, i]))
                                castling = false;
                        }
                        //******************************************//
                        //добавил это для исправления хода//
                        if (!castling)
                            for (int i = previousButton.Y - 1; i > pressedButton.Y; i--)
                                buttonMap[previousButton.X, i].Side = ChessSide.NoSide;
                        //*******************************************//
                    }
                }
                if (castling)//если все-таки рокировка возможна
                {
                    Turn(previousButton, buttonMap[previousButton.X, previousButton.Y + 2 * tempWay], true);  //король две клетки в сторону
                    Turn(pressedButton, buttonMap[previousButton.X, previousButton.Y - tempWay], true);  //ладья за короля
                    turnNumber++;
                    TurnTextBox.Text = (turnNumber + 1).ToString();
                    SideTextBox.Text = ((ChessSide)(turnNumber % 2)).ToString();
                    FunImage.Background = brushes[turnNumber % 2];
                    TextBox1.Text += "Turn: " + (turnNumber) + "\t" + str + "\n";

                    //*******зануляем сссылку во избежание ложных ходов*****//
                    previousButton = null;
                }
            }
            //первое нажатие
            else if (pressedButton.Side == (ChessSide)(turnNumber % 2))
            {
                if (previousButton != null) //если выбираем другие фигуры, то убираем с пред выделение
                {
                    //убираем выделение
                    previousButton.BorderBrush = Brushes.Black;
                    previousButton.BorderThickness = new Thickness(1);
                }
                pressedButton.BorderBrush = Brushes.Aquamarine;                    
                pressedButton.BorderThickness = new Thickness(5);
                previousButton = pressedButton;   //запоминаем первое нажатие
            }
            //второе
            else if (previousButton != null && previousButton.Potential(pressedButton.X, pressedButton.Y, pressedButton.Side))
            {
                Turn(previousButton, pressedButton,false);
                if (passantButton != null)
                {
                    chessBoard.Children.Remove(buttonMap[passantButton.X, passantButton.Y]);
                    buttonMap[passantButton.X, passantButton.Y] = passantButton;
                    passantButton.Click += Pressed;
                    chessBoard.Children.Add(passantButton);
                    passantButton = null;
                }

                if ((turnNumber % 2) == 0)
                {
                    whiteShah = Shah(whiteKingPointer);
                    if (whiteShah)
                    {
                        FunImage.Background = Brushes.Red;
                        TextBox1.Text += "Check White\n";
                    }
                }
                else
                {
                    blackShah = Shah(blackKingPointer);
                    if (blackShah)
                    {
                        FunImage.Background = Brushes.Red;
                        TextBox1.Text += "Check Black\n";
                    }
                }              
                //*******зануляем сссылку во избежание ложных ходов*****//
                previousButton = null; 
            }
        }

        public void Turn(NewButton button1, NewButton button2, bool castling)
        {
            //убираем выделение
            button1.BorderBrush = Brushes.Black;
            button1.BorderThickness = new Thickness(1);

            if (!castling) //текстовое описание для рокировки
            {
                TextTurn(button1, button2);
                turnNumber++;
                TurnTextBox.Text = (turnNumber + 1).ToString();
                SideTextBox.Text = ((ChessSide)(turnNumber % 2)).ToString();
                FunImage.Background = brushes[turnNumber % 2];
            }

            if (button2.Type == ChessType.King)//если срублен король
            {
                MessageBox.Show((ChessSide)(turnNumber % 2) + " Win");
                TextBox1.Text += "Check and mate\n";
                FunImage.Background = null;
                //DisableButtons();
            }

            if (button1.NumOfFirstTurn == 0)  //отмечаем ход на котором фигура походила, если до этого она не совершала ходы (для рокировки и пешек)
                button1.NumOfFirstTurn = turnNumber + 1;

            chessBoard.Children.Remove(button1);
            chessBoard.Children.Remove(button2);
            
            Grid.SetRow(button1, button2.X);
            Grid.SetColumn(button1, button2.Y);

            chessBoard.Children.Add(button1);

            NoType button = new NoType(ChessSide.NoSide, ChessType.Notype, button1.X, button1.Y);

            chessBoard.Children.Add(button);

            button.Click += Pressed;

            Grid.SetRow(button, button1.X);
            Grid.SetColumn(button, button1.Y);

            buttonMap[button2.X, button2.Y] = button1;
            buttonMap[button1.X, button1.Y] = button;

            button1.X = button2.X;
            button1.Y = button2.Y;

            button1.BorderBrush = Brushes.Black;
            button.BorderBrush = Brushes.Black;
            button.Background = button1.Background;
            button1.Background = button2.Background;

            //if (button2.Type == ChessType.King && button2.Side == ChessSide.White)  //новая ссылка на королей
            //    whiteKingPointer = button2;
            //else if (button2.Type == ChessType.King && button2.Side == ChessSide.Black)
            //    blackKingPointer = button2;

            if (button1.Type == ChessType.Pawn && (button1.X == 0 || button1.X == 7))  //пешка дошла до противооложной стороны
            {
                transButton = button2;
                Transformation(button1.Side, button2.X, button2.Y);
            }
        }

        public void Transformation(ChessSide t, int X, int Y) //трансформер пешки
        {
            NewButton b1 = new Rook(t, ChessType.Rook, X, Y),  //варианты превращения пешки
            b2 = new Bishop(t, ChessType.Bishop, X, Y),
            b3 = new Knight(t, ChessType.Knight, X, Y),
            b4 = new Queen(t, ChessType.Queen, X, Y);

            FunImage.Children.Add(b1);
            FunImage.Children.Add(b2);
            FunImage.Children.Add(b3);
            FunImage.Children.Add(b4);

            foreach (UIElement c in FunImage.Children)
                if (c is NewButton)
                    ((NewButton)c).Click += TransformationClick;
        }

        public void TransformationClick(object sender, RoutedEventArgs e)
        {
            NewButton temp = (NewButton)sender;
            NewButton newButton = null;
            switch (temp.Type)
            {
                case ChessType.Rook:
                    newButton = new Rook(temp.Side, ChessType.Rook, temp.X, temp.Y);
                    TextBox1.Text += "Transform to Rook\n";
                    break;
                case ChessType.Bishop:
                    newButton = new Bishop(temp.Side, ChessType.Bishop, temp.X, temp.Y);
                    TextBox1.Text += "Transform to Bishop\n";
                    break;
                case ChessType.Knight:
                    newButton = new Knight(temp.Side, ChessType.Knight, temp.X, temp.Y);
                    TextBox1.Text += "Transform to Knight\n";
                    break;
                case ChessType.Queen:
                    newButton = new Queen(temp.Side, ChessType.Queen, temp.X, temp.Y);
                    TextBox1.Text += "Transform to Queen\n";
                    break;
            }
            newButton.Background = transButton.Background;
            chessBoard.Children.Remove(transButton);
            Grid.SetRow(newButton, transButton.X);
            Grid.SetColumn(newButton, transButton.Y);
            newButton.BorderBrush = Brushes.Black;
            newButton.Click += Pressed;
            buttonMap[newButton.X, newButton.Y] = newButton;          
            FunImage.Children.Clear();  //и убираем кнопки с панельки
            chessBoard.Children.Add(newButton);
        }

        #region OldFunctional
        //public void BackTurnClick(object sender, RoutedEventArgs e)  //откат хода.    только одного(((  beta 1.3.3.7
        //{
        //    if (turnNumber != 0)
        //    {
        //        BackTurn();
        //        turnNumber--;
        //        TextBox1.Text += "Turn: " + (turnNumber + 1) + "\t" + "cancel" + "\n";
        //        TurnTextBox.Text = (turnNumber + 1).ToString();
        //        SideTextBox.Text = ((ChessSide)(turnNumber % 2)).ToString();
        //        FunImage.Background = brushes[turnNumber % 2];
        //    }
        //}

        //public void BackTurn()
        //{
        //    prev1Button.Content = prev2Button.Content;
        //    prev1Button.Type = prev2Button.Type;
        //    prev1Button.Side = prev2Button.Side;
        //    prev1Button.NumOfFirstTurn = temp;

        //    //prev2Button.Content = tempButton.Content;
        //    //prev2Button.Type = tempButton.Type;
        //    //prev2Button.Side = tempButton.Side;
        //    //prev2Button.NumOfFirstTurn = tempButton.NumOfFirstTurn;

        //    if (prev1Button.Type == ChessType.King && prev1Button.Side == ChessSide.White)
        //        whiteKingPointer = prev1Button;
        //    else if (prev1Button.Type == ChessType.King && prev1Button.Side == ChessSide.Black)
        //        blackKingPointer = prev1Button;
        //}
        #endregion

        public bool Shah(NewButton button)    //шах, тоже бета
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (button.Side != buttonMap[i, j].Side && buttonMap[i, j].Side != ChessSide.NoSide && buttonMap[i, j].Potential(button.X, button.Y, button.Side)) //если кто-то может дойти до короля
                        return true;
            return false;
        }

        public void DisableButtons() //***************что0то более интерестинг********************//
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    buttonMap[i, j].IsEnabled = false; 
        }

        public void LightThema(object sender, RoutedEventArgs e)
        {
            if (!themaLight) //если эта тема текущая, то ниччего не делаем
            {
                themaLight = true;
                ThemaChange(themaLight);
            }
        }

        public void DarkThema(object sender, RoutedEventArgs e)
        {
            if (themaLight)
            {
                themaLight = false;
                ThemaChange(themaLight);
            }
        }

        public void ThemaChange(bool temp)
        {
            Brush br1, br2;
            if (temp)
            {
                chessBackBoard.Background = Brushes.Peru;
                TextBox1.Background = Brushes.Tan;
                menu.Background = Brushes.RosyBrown;
                br1 = Brushes.Peru;
                br2 = Brushes.Sienna;
            }
            else
            {
                chessBackBoard.Background = Brushes.Gray;
                TextBox1.Background = Brushes.DimGray;
                menu.Background = Brushes.SlateGray;
                br1 = Brushes.LightGray;
                br2 = Brushes.Gray;
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                        buttonMap[i, j].Background = br1;
                    else
                        buttonMap[i, j].Background = br2;
                }
        }

        public string ConvertToUnicode(ChessType type, ChessSide side)
        {
            string str = "";
            switch (type)
            {
                case ChessType.Pawn:
                    _ = (side == ChessSide.White) ? str = "\u2659" : str = "\u265f";
                    break;
                case ChessType.Rook:
                    _ = (side == ChessSide.White) ? str = "\u2656" : str = "\u265c";
                    break;
                case ChessType.Knight:
                    _ = (side == ChessSide.White) ? str = "\u2658" : str = "\u265e";
                    break;
                case ChessType.Bishop:
                    _ = (side == ChessSide.White) ? str = "\u2657" : str = "\u265d";
                    break;
                case ChessType.Queen:
                    _ = (side == ChessSide.White) ? str = "\u2655" : str = "\u265b";
                    break;
                case ChessType.King:
                    _ = (side == ChessSide.White) ? str = "\u2654" : str = "\u265a";
                    break;
            }
            return str;
        }

        public void TextTurn(NewButton button1, NewButton button2)  =>
            TextBox1.Text += "Turn: " + (turnNumber + 1) + "\t" + ConvertToUnicode(button1.Type, button1.Side)
                    + " " + ((char)(65 + button1.Y)).ToString().ToLower() + (8 - button1.X)
                    + " " + ((char)(65 + button2.Y)).ToString().ToLower() + (8 - button2.X)
                    + " " + ConvertToUnicode(button2.Type, button2.Side) + "\n";
    }
}