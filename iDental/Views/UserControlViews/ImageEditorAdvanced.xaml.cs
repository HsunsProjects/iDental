using EffectsLibrary;
using iDental.Class;
using iDental.iDentalClass;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        /// 原圖比例
        /// </summary>
        private double ratio;
        /// <summary>
        /// 預設旋轉角度
        /// </summary>
        private const double defaultRotateAngle = 0;
        /// <summary>
        /// 預設亮度
        /// </summary>
        private const double defaultBrightness = 0;
        /// <summary>
        /// 預設對比
        /// </summary>
        private const double defaultConstrast = 1;
        /// <summary>
        /// 預設銳化
        /// </summary>
        private const double defaultSharpen = 0;
        /// <summary>
        /// 預設除霧
        /// </summary>
        private const double defaultDefog = 0;
        /// <summary>
        /// 預設曝光
        /// </summary>
        private const double defaultExposure = 0;
        /// <summary>
        /// 預設Gamma
        /// </summary>
        private const double defaultGamma = 1;

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

                    DealNewImage(bitmapImage, rotateAngle, (int)oriStartPoint.X, (int)oriStartPoint.Y, (int)oriWidth, (int)oriHeight, ImageInfo.Image_FullPath);

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
        /// 灰階Effect
        /// </summary>
        private GrayScaleEffect grayScaleEffect = new GrayScaleEffect();
        /// <summary>
        /// 顏色反轉Effect
        /// </summary>
        private InvertColorEffect invertColorEffect = new InvertColorEffect();

        private void CheckBox_FilterGrayScale_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                filterGrayScale.Effect = grayScaleEffect;
            }
        }

        private void CheckBox_FilterGrayScale_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == false)
            {
                filterGrayScale.Effect = null;
            }
        }

        private void CheckBox_FilterInvertColor_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                filterInvertColor.Effect = invertColorEffect;
            }
        }

        private void CheckBox_FilterInvertColor_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == false)
            {
                filterInvertColor.Effect = null;
            }
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

        /// <summary>
        /// 恢復圖片預設
        /// </summary>
        /// <param name="FileName"></param>
        private void SetImageDefault(string FileName)
        {
            bitmapImage = new CreateBitmapImage().SettingBitmapImage(ImageInfo.Image_FullPath, 0);
            image.Source = bitmapImage;

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

            rectangle.Width = image.Width;
            rectangle.Height = image.Height;
            
            //rotate angle default
            sliderRotate.Value = defaultRotateAngle;

            //filter default
            sliderBrightness.Value = defaultBrightness;

            sliderContrast.Value = defaultConstrast;

            sliderSharpen.Value = defaultSharpen;
            ((SharpenEffect)filterSharpen.Effect).InputSize = new Size(border.Width, border.Height);

            sliderDefog.Value = defaultDefog;

            sliderExposure.Value = defaultExposure;

            sliderGamma.Value = defaultGamma;

            checkboxGrayScale.IsChecked = false;

            checkboxInvertColor.IsChecked = false;

            SetRectanglePosition(0);

            TipsMsg();
        }

        /// <summary>
        /// 處理變動後的Image
        /// </summary>
        /// <param name="sourceImage">圖片來源</param>
        /// <param name="angle">旋轉角度</param>
        /// <param name="startX">裁切起始點X</param>
        /// <param name="startY">裁切起始點Y</param>
        /// <param name="width">裁切寬</param>
        /// <param name="height">裁切高</param>
        /// <param name="filePath">圖片路徑</param>
        public void DealNewImage(BitmapImage sourceImage, double angle,
                              int startX, int startY, int width, int height,
                              string filePath)
        {
            try
            {

                DrawingVisual vis = new DrawingVisual();
                RenderTargetBitmap rtb = new RenderTargetBitmap(sourceImage.PixelWidth, sourceImage.PixelHeight, 96d, 96d, PixelFormats.Default);
                using (DrawingContext cont = vis.RenderOpen())
                {
                    if (sliderRotate.Value != defaultRotateAngle || border.Width != rectangle.Width || border.Height != rectangle.Height)
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

                        cont.PushTransform(transformGroup);
                        cont.DrawImage(sourceImage, new Rect(new Size(sourceImage.PixelWidth, sourceImage.PixelHeight)));
                        cont.Close();
                        rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
                        SaveNewImage(rtb, vis, filePath);
                    }
                    else
                    {
                        cont.DrawImage(sourceImage, new Rect(new Size(sourceImage.PixelWidth, sourceImage.PixelHeight)));
                        cont.Close();
                        SaveNewImage(rtb, vis, filePath);
                    }

                    if (sliderBrightness.Value != defaultBrightness || sliderContrast.Value != defaultConstrast)
                    {
                        vis.Effect = filterBrightnessContrast.Effect;
                        SaveNewImage(rtb, vis, filePath);
                    }

                    if (sliderSharpen.Value != defaultSharpen)
                    {
                        vis.Effect = filterSharpen.Effect;
                        SaveNewImage(rtb, vis, filePath);
                    }

                    if (sliderDefog.Value != defaultDefog || sliderExposure.Value != defaultExposure || sliderGamma.Value != defaultGamma)
                    {
                        vis.Effect = filterExposureGamma.Effect;
                        SaveNewImage(rtb, vis, filePath);
                    }

                    if (checkboxGrayScale.IsChecked == true)
                    {
                        vis.Effect = filterGrayScale.Effect;
                        SaveNewImage(rtb, vis, filePath);
                    }

                    if (checkboxInvertColor.IsChecked == true)
                    {
                        vis.Effect = filterInvertColor.Effect;
                        SaveNewImage(rtb, vis, filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
            }
        }

        /// <summary>
        /// 儲存新影像
        /// </summary>
        /// <param name="renderTargetBitmap">儲存Bitmap來源</param>
        /// <param name="drawingVisual">新圖</param>
        /// <param name="fileName">寫入檔案名稱</param>
        private void SaveNewImage(RenderTargetBitmap renderTargetBitmap, DrawingVisual drawingVisual, string fileName)
        {
            renderTargetBitmap.Render(drawingVisual);

            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                encoder.Save(stream);
                stream.Close();
            }
        }

        /// <summary>
        /// 旋轉角度
        /// </summary>
        private double rotateAngle;
        /// <summary>
        /// 放Rectangle位置
        /// </summary>
        /// <param name="RotateAngle">旋轉角度</param>
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

        /// <summary>
        /// 提示
        /// </summary>
        private void TipsMsg()
        {
            string tipsMsg = string.Empty;
            tipsMsg += "圖片:[" + ImageInfo.Image_FileName + "] , ";
            tipsMsg += "旋轉角度:[" + rotateAngle + "] ";
            info.Text = tipsMsg;
        }
    }
}
