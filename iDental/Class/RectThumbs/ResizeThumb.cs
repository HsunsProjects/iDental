using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace iDental.Class.RectThumbs
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control designerItem = this.DataContext as Control;


            if (designerItem != null)
            {
                Canvas canvas = designerItem.Parent as Canvas;
                Border border = canvas.FindName("border") as Border;

                double deltaVertical, deltaHorizontal;

                designerItem.Width = designerItem.ActualWidth;
                designerItem.Height = designerItem.ActualHeight;

                Point p1 = new Point(Canvas.GetLeft(designerItem), Canvas.GetTop(designerItem));
                Point p2 = new Point((p1.X + designerItem.Width), p1.Y);
                Point p3 = new Point((p1.X + designerItem.Width), (p1.Y + designerItem.Height));
                Point p4 = new Point(p1.X, (p1.Y + designerItem.Height));

                Point transPoint1;
                Point transPoint2;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        transPoint1 = canvas.TranslatePoint(new Point(p3.X, p3.Y + e.VerticalChange), border);
                        transPoint2 = canvas.TranslatePoint(new Point(p4.X, p4.Y + e.VerticalChange), border);
                        if (transPoint1.Y >= 0 && transPoint1.Y <= border.Height &&
                            transPoint2.Y >= 0 && transPoint2.Y <= border.Height &&
                            transPoint1.X >= 0 && transPoint1.X <= border.Width &&
                            transPoint2.X >= 0 && transPoint2.X <= border.Width)
                        {
                            deltaVertical = Math.Min(-e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                            designerItem.Height -= deltaVertical;
                        }
                        break;
                    case VerticalAlignment.Top:
                        transPoint1 = canvas.TranslatePoint(new Point(p1.X, p1.Y + e.VerticalChange), border);
                        transPoint2 = canvas.TranslatePoint(new Point(p2.X, p2.Y + e.VerticalChange), border);
                        if (transPoint1.Y >= 0 && transPoint1.Y <= border.Height &&
                            transPoint2.Y >= 0 && transPoint2.Y <= border.Height &&
                            transPoint1.X >= 0 && transPoint1.X <= border.Width &&
                            transPoint2.X >= 0 && transPoint2.X <= border.Width)
                        {
                            deltaVertical = Math.Min(e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                            Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + deltaVertical);
                            designerItem.Height -= deltaVertical;
                        }
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        transPoint1 = canvas.TranslatePoint(new Point(p1.X + e.HorizontalChange, p1.Y), border);
                        transPoint2 = canvas.TranslatePoint(new Point(p4.X + e.HorizontalChange, p4.Y), border);
                        if (transPoint1.X >= 0 && transPoint1.X <= border.Width &&
                            transPoint2.X >= 0 && transPoint2.X <= border.Width &&
                            transPoint1.Y >= 0 && transPoint1.Y <= border.Height &&
                            transPoint2.Y >= 0 && transPoint2.Y <= border.Height)
                        {
                            deltaHorizontal = Math.Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                            Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaHorizontal);
                            designerItem.Width -= deltaHorizontal;
                        }
                        break;
                    case HorizontalAlignment.Right:
                        transPoint1 = canvas.TranslatePoint(new Point(p2.X + e.HorizontalChange, p2.Y), border);
                        transPoint2 = canvas.TranslatePoint(new Point(p3.X + e.HorizontalChange, p3.Y), border);
                        if (transPoint1.X >= 0 && transPoint1.X <= border.Width &&
                            transPoint2.X >= 0 && transPoint2.X <= border.Width &&
                            transPoint1.Y >= 0 && transPoint1.Y <= border.Height &&
                            transPoint2.Y >= 0 && transPoint2.Y <= border.Height)
                        {
                            deltaHorizontal = Math.Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                            designerItem.Width -= deltaHorizontal;
                        }
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }
    }
}
