using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace iDental.Class.RectThumbs
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control designerItem = this.DataContext as Control;

            if (designerItem != null)
            {
                Canvas canvas = designerItem.Parent as Canvas;
                Border border = canvas.FindName("border") as Border;

                designerItem.Width = designerItem.ActualWidth;
                designerItem.Height = designerItem.ActualHeight;

                Point p1 = new Point(Canvas.GetLeft(designerItem), Canvas.GetTop(designerItem));
                Point p2 = new Point((p1.X + designerItem.Width), p1.Y);
                Point p3 = new Point((p1.X + designerItem.Width), (p1.Y + designerItem.Height));
                Point p4 = new Point(p1.X, (p1.Y + designerItem.Height));

                double left = Canvas.GetLeft(designerItem);
                double top = Canvas.GetTop(designerItem);

                Point tmpP1;
                Point tmpP2;
                Point tmpP3;
                Point tmpP4;
                if (e.HorizontalChange != 0)
                {
                    tmpP1 = canvas.TranslatePoint(new Point(p1.X + e.HorizontalChange, p1.Y), border);
                    tmpP2 = canvas.TranslatePoint(new Point(p2.X + e.HorizontalChange, p2.Y), border);
                    tmpP3 = canvas.TranslatePoint(new Point(p3.X + e.HorizontalChange, p3.Y), border);
                    tmpP4 = canvas.TranslatePoint(new Point(p4.X + e.HorizontalChange, p4.Y), border);
                    if (IsValidArea(tmpP1.X, 0, border.Width) &&
                        IsValidArea(tmpP2.X, 0, border.Width) &&
                        IsValidArea(tmpP3.X, 0, border.Width) &&
                        IsValidArea(tmpP4.X, 0, border.Width) &&
                        IsValidArea(tmpP1.Y, 0, border.Height) &&
                        IsValidArea(tmpP2.Y, 0, border.Height) &&
                        IsValidArea(tmpP3.Y, 0, border.Height) &&
                        IsValidArea(tmpP4.Y, 0, border.Height))
                    {
                        Canvas.SetLeft(designerItem, left + e.HorizontalChange);
                    }
                }
                if (e.VerticalChange != 0)
                {
                    tmpP1 = canvas.TranslatePoint(new Point(p1.X, p1.Y + e.VerticalChange), border);
                    tmpP2 = canvas.TranslatePoint(new Point(p2.X, p2.Y + e.VerticalChange), border);
                    tmpP3 = canvas.TranslatePoint(new Point(p3.X, p3.Y + e.VerticalChange), border);
                    tmpP4 = canvas.TranslatePoint(new Point(p4.X, p4.Y + e.VerticalChange), border);
                    if (IsValidArea(tmpP1.X, 0, border.Width) &&
                        IsValidArea(tmpP2.X, 0, border.Width) &&
                        IsValidArea(tmpP3.X, 0, border.Width) &&
                        IsValidArea(tmpP4.X, 0, border.Width) &&
                        IsValidArea(tmpP1.Y, 0, border.Height) &&
                        IsValidArea(tmpP2.Y, 0, border.Height) &&
                        IsValidArea(tmpP3.Y, 0, border.Height) &&
                        IsValidArea(tmpP4.Y, 0, border.Height))
                    {
                        Canvas.SetTop(designerItem, top + e.VerticalChange);
                    }
                }
            }
        }

        private bool IsValidArea(double Area, double Min, double Max)
        {
            if (Area >= Min && Area <= Max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
