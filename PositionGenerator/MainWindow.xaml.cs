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

        private int[] Generate_Coords()
        {
            //x,y
            int[] avail_center = { Convert.ToInt32(avail_entry_width.Text) / 2, Convert.ToInt32(avail_entry_height.Text) / 2 };
            int[] avail_global_center = { (Convert.ToInt32(avail_entry_width.Text) / 2) + Convert.ToInt32(avail_entry_left.Text), (Convert.ToInt32(avail_entry_height.Text) / 2) + Convert.ToInt32(avail_entry_top.Text) };

            int[] object_center = { Convert.ToInt32(object_entry_width.Text) / 2, Convert.ToInt32(object_entry_height.Text) / 2 };
            int[] object_global_center = { (Convert.ToInt32(object_entry_width.Text) / 2) + Convert.ToInt32(object_entry_left.Text), (Convert.ToInt32(object_entry_height.Text) / 2) + Convert.ToInt32(object_entry_top.Text) };

            int[] object_new_coords = { avail_global_center[0] - object_center[0], avail_global_center[1] - object_center[1] };
            return object_new_coords;

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

            GenType typeSwitch = GetSelectedRadioBtn(CoordButtonContainer);

            Coordinate newCoords = null;
            switch (typeSwitch)
            {
                case GenType.center:
                {
                    newCoords = ElemEnclosing - ElemObject;
                    break;
                }
            }
            
            OutputBox.Text = "New Top Coord: " + newCoords.Y + Environment.NewLine + "New Left Coord: " + newCoords.X + Environment.NewLine;
        }
        
        enum GenType { center };

        private GenType GetSelectedRadioBtn(Panel pnl)
        {
            foreach (RadioButton rb in pnl.Children)
            {
                if (rb.IsChecked == true)
                {
                    return CompareToEnum(rb.Name);
                }
            }
            return 0;
        }
        private GenType CompareToEnum(string name)
        {
            foreach (GenType gentype in Enum.GetValues(typeof(GenType)))
            {
                if (name == gentype.ToString()) return gentype;
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

    public class PositionElement
    {
        private int Width { get; }
        private int Height { get; }
        private Coordinate Coords { get; set; }
        private Coordinate CenterCoords { get; set; }
        private Coordinate GlobalCenterCoords { get; set; }

        public PositionElement(int width, int height, int x, int y)
        {
            Width = width;
            Height = height;
            Coords = new Coordinate(x, y);

            CenterCoords = GenerateCenterCoords();
            GlobalCenterCoords = GenerateCenterCoordsWRTCanvas();
        }

        private Coordinate GenerateCenterCoords()
        {
            int xcentercoord;
            if ((this.Width % 2) == 1)
            {
                xcentercoord = (this.Width / 2);
            }
            else
            {
                xcentercoord = Convert.ToInt32(((double)this.Width) / 2);
            }
            int ycentercoord;
            if ((this.Height % 2) == 1)
            {
                ycentercoord = (this.Height / 2);
            }
            else
            {
                ycentercoord = Convert.ToInt32(((double)this.Height) / 2);
            }

            //CenterCoords = new Coordinate(xcentercoord, ycentercoord);
            return new Coordinate(xcentercoord, ycentercoord);
        }

        public Coordinate GenerateCenterCoordsWRTCanvas()
        {
            return new Coordinate(CenterCoords.X + Coords.X, CenterCoords.Y + Coords.Y);
            /*
            if ((this.Coords.X != 0) || (this.Coords.Y != 0))
            {
                return new Coordinate(CenterCoords.X + Coords.X, CenterCoords.Y + Coords.Y);
            }
            return null;*/
        }
        
        
        public static Coordinate operator -(PositionElement element, PositionElement containtingelement)
        {
            return new Coordinate(Math.Abs(containtingelement.GlobalCenterCoords.X - element.GlobalCenterCoords.X), Math.Abs(containtingelement.GlobalCenterCoords.Y - element.GlobalCenterCoords.Y));
        }
        public static Coordinate operator +(PositionElement element, PositionElement containtingelement)
        {
            return new Coordinate(Math.Abs(containtingelement.GlobalCenterCoords.X + element.GlobalCenterCoords.X), Math.Abs(containtingelement.GlobalCenterCoords.Y + element.GlobalCenterCoords.Y));
        }
    }
}
