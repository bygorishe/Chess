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

        public class SubButton : Button
        {
            public int Type;
        }


        Image[] img = new Image[12];

        int[,] map = new int[4, 8]
        {
           {5,2,1,3,0,1,2,5},
           {104,104,104,104,104,104,104,104},
           {610,610,610,610,610,610,610,610},
           {711,708,707,709,706,707,708,711},
        };

        SubButton[,] buttonMap = new SubButton[8, 8];

        SubButton previousButton = new SubButton();

      


        public MainWindow()
        {
            InitializeComponent();

            CreateMap();

            previousButton = null;


            foreach (UIElement c in chessBoard.Children)
            {
                if (c is SubButton)
                {
                    ((SubButton)c).Click += Pressed;
                }
            }

            //Turn(buttonMap[7, 7], buttonMap[6, 7]);

            //Swap(buttonMap[7, 7], buttonMap[6, 7]);

        }

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

            
            /*for (int i = 0; i < 32; i++)
            {
                if((i/4) % 2 == 0)
                {
                    Rectangle rect1 = new Rectangle();
                    rect1.Fill = Brushes.Black;
                    chessBoardBackground.Children.Add(rect1);

                    Rectangle rect2 = new Rectangle();
                    //rect2.Fill = Brushes.Peru;
                    chessBoardBackground.Children.Add(rect2);
                }
                else
                {
                    Rectangle rect2 = new Rectangle();
                    //rect2.Fill = Brushes.Peru;
                    chessBoardBackground.Children.Add(rect2);

                    Rectangle rect1 = new Rectangle();
                    rect1.Fill = Brushes.Black;
                    chessBoardBackground.Children.Add(rect1);          
                }
            }*/

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    buttonMap[i, j] = new SubButton();
                    if ((i + j) % 2 == 0)
                    {
                        buttonMap[i, j].Background = Brushes.Peru;
                        buttonMap[i, j].BorderBrush = Brushes.Black;
                    }
                    else
                    {
                        buttonMap[i, j].Background = Brushes.Sienna;
                        buttonMap[i, j].BorderBrush = Brushes.Black;
                    }
                    //buttonMap[i, j].Opacity = 0.25; //потом поменять прозрачность
                    chessBoard.Children.Add(buttonMap[i, j]);

                }

            img[0] = new Image();
            img[0].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\kingBlack.png"));
            img[1] = new Image();
            img[1].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\bishopBlack.png"));
            img[2] = new Image();
            img[2].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\knightBlack.png"));
            img[3] = new Image();
            img[3].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\queenBlack.png"));
            img[4] = new Image();
            img[4].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\pawnBlack.png"));
            img[5] = new Image();
            img[5].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\rookBlack.png"));
            img[6] = new Image();
            img[6].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\kingWhite.png"));
            img[7] = new Image();
            img[7].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\bishopWhite.png"));
            img[8] = new Image();
            img[8].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\knightWhite.png"));
            img[9] = new Image();
            img[9].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\queenWhite.png"));
            img[10] = new Image();
            img[10].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\pawnWhite.png"));
            img[11] = new Image();
            img[11].Source = new BitmapImage(new Uri(@"C:\Users\angry\source\repos\Chess\Chess\Fig\rookWhite.png"));

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 8; j++)
                {
                    int ii = map[i, j] / 100;
                    int i_map = map[i, j] % 100;
                    Image img1 = new Image();
                    img1.Source = img[i_map].Source;
                    buttonMap[ii, j].Content = img1;
                }
        }

        public void Pressed(object sender, RoutedEventArgs e)
        {
            SubButton pressedButton = sender as SubButton;

           // if(ressedButton.Content != null)

            if (pressedButton.Content != null)
            {
                if (pressedButton.Content != null && previousButton != null)
                    previousButton.Effect = null;
                // pressedButton.Background = Brushes.Red; //какой-то эффект добавить
                BlurEffect blur = new BlurEffect();
                blur.Radius = 3;
                pressedButton.Effect = blur;

                previousButton = pressedButton;

                //not bad

                int col = 0, row = 0;
                    
                for (int i=0;i<8;i++)
                    for (int j = 0; j < 8; j++) { 
                        if (buttonMap[i,j].Equals(previousButton))
                    {
                            col = j;
                            row = i;
                    }
                }
                buttonMap[row, col].Background = Brushes.Red;
            }

            /*if (previousButton == null)
                previousButton = pressedButton;*/
            else if (pressedButton.Content == null && previousButton != null)
            {
                
                Turn(previousButton, pressedButton);
                previousButton.Effect = null;

                //   previousButton.Background = Brushes.Blue;
                previousButton = null;
            }

            
        }

        public void Swap(SubButton button1, SubButton button2)
        {
            SubButton temp = new SubButton();
            temp.Content = button1.Content;
            button1.Content = button2.Content;
            button2.Content = temp.Content;
        }

        public void Turn(SubButton button1, SubButton button2)
        {
            button2.Content = button1.Content;
            button1.Content = null;
        }
    }
}
