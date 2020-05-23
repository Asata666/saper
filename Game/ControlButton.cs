using System.Windows.Forms;
using Sapper.Properties;

namespace Sapper
{
    internal class ControlButton : Button
    {
        private readonly Field field;
        public readonly int X;
        public readonly int Y;

        public ControlButton(Field field, int x, int y)
        {
            this.field = field;
            X = x;
            Y = y;
        }

        protected override void OnMouseDown(MouseEventArgs mouseEvent)
        {
            if (mouseEvent.Button == MouseButtons.Right)
            {
                if(!field.Cells[X,Y].IsOpen)
                    BackgroundImage = Resources.cellflag;
            }
            else
                field.OpenCell(X, Y);
        }
    }
}
