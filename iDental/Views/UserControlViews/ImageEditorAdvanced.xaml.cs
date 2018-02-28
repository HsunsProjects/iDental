using iDental.Class;
using iDental.iDentalClass;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iDental.Views.UserControlViews
{
    /// <summary>
    /// ImageEditorAdvanced.xaml 的互動邏輯
    /// </summary>
    public partial class ImageEditorAdvanced : UserControl
    {
        private ObservableCollection<ImageInfo> ImagesCollection { get; set; }
        private ImageInfo ImageInfo { get; set; }
        /// <summary>
        /// 圖片source
        /// </summary>
        private BitmapImage bitmapImage;
        /// <summary>
        /// 顯示圖片控制項
        /// </summary>
        private Image image;
        /// <summary>
        /// 原圖比例
        /// </summary>
        private double ratio;


        public ImageEditorAdvanced(ObservableCollection<ImageInfo> imagesCollection, ImageInfo imageInfo)
        {
            InitializeComponent();

            ImagesCollection = imagesCollection;
            ImageInfo = imageInfo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetImageDefault(ImageInfo.Image_FullPath);
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("確定完成修改並儲存?", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                try
                {
                    //原始圖片比例的rectangle
                    double oriWidth = bitmapImage.PixelWidth * (rectangle.Width / border.Width);
                    double oriHeight = bitmapImage.PixelHeight * (rectangle.Height / border.Height);

                    //image 控制項P1
                    Point sourceP1 = new Point(Canvas.GetLeft(rectangle) - Canvas.GetLeft(border), Canvas.GetTop(rectangle) - Canvas.GetTop(border));
                    //比例
                    double rectangleRatioX = sourceP1.X / border.Width;
                    double rectangleRatioY = sourceP1.Y / border.Height;
                    //原始圖片的P1
                    Point oriStartPoint = new Point(bitmapImage.PixelWidth * rectangleRatioX, bitmapImage.PixelHeight * rectangleRatioY);

                    RotateAndSaveImage(bitmapImage, rotateAngle, (int)oriStartPoint.X, (int)oriStartPoint.Y, (int)oriWidth, (int)oriHeight, ImageInfo.Image_FullPath);

                    //儲存完馬上重載修改後的圖片
                    ImageInfo.BitmapImage = new CreateBitmapImage().SettingBitmapImage(ImageInfo.Image_FullPath, 800);
                    MessageBox.Show("儲存成功", "提示", MessageBoxButton.OK);
                    
                    SetImageDefault(ImageInfo.Image_FullPath);
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorMessageOutput(ex.ToString());
                }
            }
        }

        private void Button_Undo_Click(object sender, RoutedEventArgs e)
        {
            SetImageDefault(ImageInfo.Image_FullPath);

            sliderRotate.Value = 0;

            SetRectanglePosition(0);
        }

        private void Button_ExitEditor_Click(object sender, RoutedEventArgs e)
        {
            ImageEditorBase imageEditorBase = new ImageEditorBase(ImagesCollection, ImageInfo);
            Content = imageEditorBase;

            GC.Collect();
        }
        
        /// <summary>
        /// 最大的rectangle
        /// </summary>
        private double[] MaxRectangle;

        private void sliderRotate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetRectanglePosition(Math.Round(e.NewValue,1));
            TipsMsg();
        }

        /// <summary>
        /// 取得旋轉中的框框
        /// </summary>
        /// <param name="imageWidth">image寬</param>
        /// <param name="imageHeight">image長</param>
        /// <param name="rotAngDeg">旋轉角度</param>
        /// <returns>回傳框框長寬</returns>
        public double[] GetLargestRectangle(double imageWidth, double imageHeight, double rotAngDeg)
        {
            double angle = Math.PI * rotAngDeg / 180;
            double minSide, maxSide;
            if (imageWidth <= imageHeight)
            {
                minSide = imageWidth;
                maxSide = imageHeight;
            }
            else
            {
                minSide = imageHeight;
                maxSide = imageWidth;
            }

            double absAngle = Math.Abs(angle);
            double sinAngle = Math.Sin(absAngle);
            double cosAngle = Math.Cos(absAngle);
            double maxOuter = sinAngle * minSide + cosAngle * maxSide;
            double minOuter = sinAngle * maxSide + cosAngle * minSide;
            double sinAcosA = sinAngle * cosAngle;
            double c = maxSide * sinAcosA / (2 * maxSide * sinAcosA + minSide);
            double minInner = minOuter - (2 * minOuter * c);
            double maxInner = maxOuter - (2 * maxOuter * c);

            double w, h;

            if (imageWidth <= imageHeight)
            {
                w = minInner;
                h = maxInner;
            }
            else
            {
                w = maxInner;
                h = minInner;
            }

            double[] wh = new double[2] { w, h };
            return wh;
        }

        private void SetImageDefault(string FileName)
        {
            bitmapImage = new CreateBitmapImage().SettingBitmapImage(FileName, 0);
            image = new Image()
            {
                Source = bitmapImage
            };

            double w;
            double h;
            ratio = canvas.ActualWidth / bitmapImage.PixelWidth;

            if ((ratio * bitmapImage.PixelHeight) > canvas.ActualHeight)
            {
                //鎖定高 寬填滿
                ratio = canvas.ActualHeight / bitmapImage.PixelHeight;
                w = ratio * bitmapImage.PixelWidth;
                h = canvas.ActualHeight;

            }
            else
            {
                w = canvas.ActualWidth;
                h = ratio * bitmapImage.PixelHeight;
            }

            image.Width = w * 4 / 5;
            image.Height = h * 4 / 5;

            border.Width = image.Width;
            border.Height = image.Height;

            border.Child = image;

            rectangle.Width = image.Width;
            rectangle.Height = image.Height;

            sliderRotate.Value = 0;

            SetRectanglePosition(0);

            TipsMsg();
        }
        public void RotateAndSaveImage(BitmapImage sourceImage, double angle,
                              int startX, int startY, int width, int height,
                              string filePath)
        {
            try
            {
                TransformGroup transformGroup = new TransformGroup();
                RotateTransform rotateTransform = new RotateTransform(angle);
                rotateTransform.CenterX = sourceImage.PixelWidth / 2.0;
                rotateTransform.CenterY = sourceImage.PixelHeight / 2.0;
                transformGroup.Children.Add(rotateTransform);
                TranslateTransform translateTransform = new TranslateTransform();
                translateTransform.X = -startX;
                translateTransform.Y = -startY;
                transformGroup.Children.Add(translateTransform);

                DrawingVisual vis = new DrawingVisual();
                DrawingContext cont = vis.RenderOpen();
                cont.PushTransform(transformGroup);
                cont.DrawImage(sourceImage, new Rect(new Size(sourceImage.PixelWidth, sourceImage.PixelHeight)));
                cont.Close();

                RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
                rtb.Render(vis);

                System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                encoder.Save(stream);
                stream.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
            }
        }

        private double rotateAngle;
        private void SetRectanglePosition(double RotateAngle)
        {
            rotateAngle = RotateAngle;
            if (RotateAngle == 0)
            {
                rectangle.Width = border.Width;
                rectangle.Height = border.Height;

                Canvas.SetLeft(rectangle, Canvas.GetLeft(border));
                Canvas.SetTop(rectangle, Canvas.GetTop(border));
            }
            else
            {
                MaxRectangle = GetLargestRectangle(border.Width, border.Height, rotateAngle);

                rectangle.Width = MaxRectangle[0];
                rectangle.Height = MaxRectangle[1];

                Point imageCenter = border.TranslatePoint(new Point(border.Width / 2, border.Height / 2), canvas);

                Canvas.SetLeft(rectangle, imageCenter.X - (rectangle.Width / 2));
                Canvas.SetTop(rectangle, imageCenter.Y - (rectangle.Height / 2));
            }
        }

        private void TipsMsg()
        {
            string tipsMsg = string.Empty;
            tipsMsg += "圖片:[" + ImageInfo.Image_FileName + "] , ";
            tipsMsg += "旋轉角度:[" + rotateAngle + "] ";
            info.Text = tipsMsg;
        }


        #region Canvas Event(drag image)
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*
            var image = e.Source as Image;

            if (image != null && canvas.CaptureMouse())
            {
                mousePosition = e.GetPosition(canvas);

                draggedImage = image;
                
                //Panel.SetZIndex(draggedImage, 1); // in case of multiple images
            }
            */
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            if (draggedImage != null)
            {
                var position = e.GetPosition(canvas);
                var offset = position - mousePosition;
                mousePosition = position;

                Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + offset.X);
                Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + offset.Y);
            }
            */
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*
            if (draggedImage != null)
            {
                canvas.ReleaseMouseCapture();
                //Panel.SetZIndex(draggedImage, 0);
                draggedImage = null;
            }
            */
        }
        #endregion
    }
}
