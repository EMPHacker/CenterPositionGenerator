using System;
using System.Windows;
using System.Windows.Controls;

namespace PositionGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            center.IsChecked = true;
        }

        private int GetDigit(TextBox box)
        {
            int input = 0;
            try
            {
                input = Convert.ToInt32(box.Text);
            }
            catch (InvalidCastException ex)
            {
                Console.Write(ex);
                return 0;
            }

            return input;
        }
        private int GetCheckDigit(TextBox box)
        {
            int input = 0;
            try
            {
                input = Convert.ToInt32(box.Text);
                if (input == 0)
                {
                    MessageBox.Show("ERROR: Value of " + box.Name + " cannot be 0.");
                }
            }
            catch (InvalidCastException ex)
            {
                Console.Write(ex);
                return 0;
            }

            return input;
        }

        private void Btn_Generate_Coords(object sender, RoutedEventArgs e)
        {
            PositionElement ElemObject = new PositionElement(GetDigit(object_entry_width), GetDigit(object_entry_height), GetDigit(object_entry_left), GetDigit(object_entry_top));
            PositionElement ElemEnclosing = new PositionElement(GetDigit(avail_entry_width), GetDigit(avail_entry_height), GetDigit(avail_entry_left), GetDigit(avail_entry_top));

            GenType typeSwitch = GetGenTypeRadioBtn(CoordButtonContainer);

            switch (typeSwitch)
            {
                case GenType.center:
                    {
                        Coordinate newCoords = ElemEnclosing - ElemObject;
                        OutputBox.Text = "New Top Coord: " + newCoords.Y + Environment.NewLine + "New Left Coord: " + newCoords.X + Environment.NewLine;
                        break;
                    }
                case GenType.offset:
                    {
                        long offset = 0;
                        Position position = GetPositionRadioBtn(SideButtonContainer);
                        switch (position)
                        {
                            case Position.bottom:
                                {
                                    offset = Math.Abs((ElemEnclosing.Coords.Y + ElemEnclosing.Height) - (ElemObject.Coords.Y + ElemObject.Height));
                                    break;
                                }
                            case Position.top:
                                {
                                    offset = Math.Abs(ElemEnclosing.Coords.Y - ElemObject.Coords.Y);
                                    break;
                                }
                            case Position.left:
                                {
                                    offset = Math.Abs(ElemEnclosing.Coords.X - ElemObject.Coords.X);
                                    break;
                                }
                            case Position.right:
                                {
                                    offset = Math.Abs((ElemEnclosing.Coords.X + ElemEnclosing.Width) - (ElemObject.Coords.X + ElemObject.Width));
                                    break;
                                }

                        }
                        OutputBox.Text = position.ToString() + " offset: " + offset.ToString();
                        break;
                    }
            }
        }

        enum GenType { center, offset };
        enum Position { top, bottom, left, right };

        private GenType GetGenTypeRadioBtn(Panel pnl)
        {
            foreach (RadioButton rb in pnl.Children)
            {
                if (rb.IsChecked == true)
                {
                    foreach (GenType enumvar in Enum.GetValues(typeof(GenType)))
                    {
                        if (rb.Name == enumvar.ToString()) return enumvar;
                    }
                }
            }
            return 0;
        }
        private Position GetPositionRadioBtn(Panel pnl)
        {
            foreach (RadioButton rb in pnl.Children)
            {
                if (rb.IsChecked == true)
                {
                    foreach (Position enumvar in Enum.GetValues(typeof(Position)))
                    {
                        if (rb.Name == enumvar.ToString()) return enumvar;
                    }
                }
            }
            return 0;
        }

        private void Tb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll(); //select all text in TextBox
            }
        }

        private void GenTypeOffset_Checked(object sender, RoutedEventArgs e)
        {
            TextBox_EnclElemDim.Visibility = Visibility.Hidden;
            Panel_RefSideSelector.Visibility = Visibility.Visible;
            TextBox_RefElemPos.Visibility = Visibility.Visible;

        }

        private void GenTypeCenter_Checked(object sender, RoutedEventArgs e)
        {
            TextBox_EnclElemDim.Visibility = Visibility.Visible;
            Panel_RefSideSelector.Visibility = Visibility.Hidden;
            TextBox_RefElemPos.Visibility = Visibility.Hidden;
        }
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}