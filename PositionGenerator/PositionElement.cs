using System;

namespace PositionGenerator
{
    public class PositionElement
    {
        public int Width { get; }
        public int Height { get; }
        public Coordinate Coords { get; }
        private Coordinate CenterCoords { get; }
        private Coordinate GlobalCenterCoords { get; }

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
            return new Coordinate(Convert.ToInt32(Math.Abs(containtingelement.GlobalCenterCoords.X - element.GlobalCenterCoords.X)), Convert.ToInt32(Math.Abs(containtingelement.GlobalCenterCoords.Y - element.GlobalCenterCoords.Y)));
        }
        public static Coordinate operator +(PositionElement element, PositionElement containtingelement)
        {
            return new Coordinate(Convert.ToInt32(Math.Abs(containtingelement.GlobalCenterCoords.X + element.GlobalCenterCoords.X)), Convert.ToInt32(Math.Abs(containtingelement.GlobalCenterCoords.Y + element.GlobalCenterCoords.Y)));
        }
    }
}
