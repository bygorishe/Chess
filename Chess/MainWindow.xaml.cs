using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Chess.Enums;
using Chess.FigClasses;
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

        private readonly int[,] map = new int[8, 8]  //изначальное расположение фигур 
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

        //readonly int[,] map = new int[8, 8] 
        //{
        //   {12,13,14,15,16,14,13,20},          
        //   {11,11,11,11,11,11,11,1},           
        //   {20,20,20,20,20,20,20,20},
        //   {20,20,20,20,20,20,20,20},
        //   {20,20,1,20,20,1,20,20},
        //   {20,20,20,20,20,20,20,20},
        //   {20,20,20,20,20,20,20,20},
        //   {2,20,20,20,6,20,20,2}
        //};

        public static NewButton[,] buttonMap = new NewButton[8, 8]; //ужас блять просто поправить тут все надо*****************************************
        public static NewButton passantButton; //кнопка для рокировки*
        private NewButton previousButton,  //запоминаем кнопку для хода     
            transButton,  //для взятия на проходе
            whiteKingPointer, blackKingPointer; //указатели на королей для шаха
        public static int turnNumber = 0;  //тек. ход
        private static bool whiteShah = false, blackShah = false;   //флаг шаха
        private bool themaLight = true; //включена ли исходная цветовая тема 
        private Brush[] brushes = { Brushes.White, Brushes.Black };
        private List<Rectangle> rectanglesList = new List<Rectangle>(64);

        private void CreateMap() //отрисовка заднего фона
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
            for (int i = 0; i < 64; i++)
            {
                Rectangle rectangle = new Rectangle();
                chessBoardBackground.Children.Add(rectangle);
                rectanglesList.Add(rectangle);
            }
        }

        private void RestartClick(object sender, RoutedEventArgs e) => Start(); //рестарт просто переставляет все фигуры на исходное положение и откатывает текстовые элементы

        private void Start()
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
                    buttonMap[i, j] = (ChessType)(map[i, j] % 10) switch
                    {
                        ChessType.Pawn => new Pawn((ChessSide)(map[i, j] / 10), ChessType.Pawn, i, j),
                        ChessType.Rook => new Rook((ChessSide)(map[i, j] / 10), ChessType.Rook, i, j),
                        ChessType.Knight => new Knight((ChessSide)(map[i, j] / 10), ChessType.Knight, i, j),
                        ChessType.Bishop => new Bishop((ChessSide)(map[i, j] / 10), ChessType.Bishop, i, j),
                        ChessType.Queen => new Queen((ChessSide)(map[i, j] / 10), ChessType.Queen, i, j),
                        ChessType.King => new King((ChessSide)(map[i, j] / 10), ChessType.King, i, j),
                        _ => new NoType((ChessSide)(map[i, j] / 10), ChessType.Notype, i, j),
                    };
                    buttonMap[i, j].BorderBrush = Brushes.Black;
                    //Panel.SetZIndex(buttonMap[i, j], 10000000); //не помогает т.к. кнопка с пустым фоном все равно может не выделяться
                    chessBoard.Children.Add(buttonMap[i, j]);
                    Grid.SetColumn(buttonMap[i, j], j);
                    Grid.SetRow(buttonMap[i, j], i);
                }
            whiteKingPointer = buttonMap[7, 4];    //делаем ссылки на королей для отслеживания шаха
            blackKingPointer = buttonMap[0, 4];

            ThemaChange(themaLight);  //раскрашиваем все в соответсвии с выбранной темой

            foreach (UIElement c in chessBoard.Children)
                if (c is NewButton b)
                    b.Click += Pressed;
        }

        private void Pressed(object sender, RoutedEventArgs e) //эвент нажатия на кнопку
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
                        if (!castling)
                            for (int i = previousButton.Y - 1; i > pressedButton.Y; i--)
                                buttonMap[previousButton.X, i].Side = ChessSide.NoSide;
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
                    TextBox1.Text += "Turn: " + turnNumber + "\t" + str + "\n";

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
                Turn(previousButton, pressedButton, false);
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

        private void Turn(NewButton button1, NewButton button2, bool castling)
        {
            //убираем выделение
            button1.BorderBrush = Brushes.Black;
            button1.BorderThickness = new Thickness(1);

            if (!castling) //текстовое описание ne для рокировки
            {
                TextTurn(button1, button2);
                turnNumber++;
                TurnTextBox.Text = (turnNumber + 1).ToString();
                SideTextBox.Text = ((ChessSide)(turnNumber % 2)).ToString();
                FunImage.Background = brushes[turnNumber % 2];
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

            if (button2.Type == ChessType.King)//если срублен король
            {
                MessageBox.Show((ChessSide)(turnNumber % 2) + " Win");
                TextBox1.Text += "Check and mate\n";
                FunImage.Background = null;
                DisableButtons();
            }

            if (button2.Type == ChessType.King && button2.Side == ChessSide.White)  //новая ссылка на королей
                whiteKingPointer = button2;
            else if (button2.Type == ChessType.King && button2.Side == ChessSide.Black)
                blackKingPointer = button2;

            if (button1.Type == ChessType.Pawn && (button1.X == 0 || button1.X == 7))  //пешка дошла до противооложной стороны
            {
                transButton = button1;
                Transformation(button1.Side, button2.X, button2.Y);
            }
        }

        private void Transformation(ChessSide t, int X, int Y) //трансформер пешки
        {
            NewButton b1 = new Rook(t, ChessType.Rook, X, Y),  //варианты превращения пешки
            b2 = new Bishop(t, ChessType.Bishop, X, Y),
            b3 = new Knight(t, ChessType.Knight, X, Y),
            b4 = new Queen(t, ChessType.Queen, X, Y);

            FunImage.Background = null;
            FunImage.Children.Add(b1);
            FunImage.Children.Add(b2);
            FunImage.Children.Add(b3);
            FunImage.Children.Add(b4);

            foreach (UIElement c in FunImage.Children)
                if (c is NewButton b)
                    b.Click += TransformationClick;
        }

        private void TransformationClick(object sender, RoutedEventArgs e)
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
                default:
                    break;
            }
            chessBoard.Children.Remove(transButton);
            Grid.SetRow(newButton, transButton.X);
            Grid.SetColumn(newButton, transButton.Y);
            newButton.BorderBrush = Brushes.Black;
            newButton.Click += Pressed;
            buttonMap[newButton.X, newButton.Y] = newButton;
            FunImage.Children.Clear();  //и убираем кнопки с панельки
            chessBoard.Children.Add(newButton);
        }

        private bool Shah(NewButton button)    //шах, бета
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (button.Side != buttonMap[i, j].Side && buttonMap[i, j].Side != ChessSide.NoSide && buttonMap[i, j].Potential(button.X, button.Y, button.Side)) //если кто-то может дойти до короля
                        return true;
            return false;
        }

        private void DisableButtons()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    buttonMap[i, j].IsEnabled = false;
        }

        private void LightThema(object sender, RoutedEventArgs e)
        {
            if (!themaLight) //если эта тема текущая, то ниччего не делаем
            {
                themaLight = true;
                ThemaChange(themaLight);
            }
        }

        private void DarkThema(object sender, RoutedEventArgs e)
        {
            if (themaLight)
            {
                themaLight = false;
                ThemaChange(themaLight);
            }
        }

        private void ThemaChange(bool temp)
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
                        rectanglesList[i + 8 * j].Fill = br1;
                    else
                        rectanglesList[i + 8 * j].Fill = br2;
                }
        }

        private string ConvertToUnicode(ChessType type, ChessSide side)
        {
            string str = "";
            switch (type)
            {
                case ChessType.Pawn:
                    str = (side == ChessSide.White) ? "\u2659" : "\u265f";
                    break;
                case ChessType.Rook:
                    str = (side == ChessSide.White) ? "\u2656" : "\u265c";
                    break;
                case ChessType.Knight:
                    str = (side == ChessSide.White) ? "\u2658" : "\u265e";
                    break;
                case ChessType.Bishop:
                    str = (side == ChessSide.White) ? "\u2657" : "\u265d";
                    break;
                case ChessType.Queen:
                    str = (side == ChessSide.White) ? "\u2655" : "\u265b";
                    break;
                case ChessType.King:
                    str = (side == ChessSide.White) ? "\u2654" : "\u265a";
                    break;
            }
            return str;
        }

        private void TextTurn(NewButton button1, NewButton button2) =>
            TextBox1.Text += "Turn: " + (turnNumber + 1) + "\t" + ConvertToUnicode(button1.Type, button1.Side)
                    + " " + ((char)(65 + button1.Y)).ToString().ToLower() + (8 - button1.X)
                    + " " + ((char)(65 + button2.Y)).ToString().ToLower() + (8 - button2.X)
                    + " " + ConvertToUnicode(button2.Type, button2.Side) + "\n";
        private void AppExit(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}