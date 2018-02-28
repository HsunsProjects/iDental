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
    /// ImageEditorBase.xaml 的互動邏輯
    /// </summary>
    public partial class ImageEditorBase : UserControl
    {
        private ObservableCollection<ImageInfo> ImagesCollection { get; set; }

        private ImageInfo imageInfo;

        public ImageInfo ImageInfo
        {
            get { return imageInfo; }
            set
            {
                imageInfo = value;
                image.Source = new CreateBitmapImage().SettingBitmapImage(imageInfo.Image_FullPath, 0);
                textFileName.Text = imageInfo.Image_FileName;
                textTips.Text = " [ " + (ImagesCollection.IndexOf(ImageInfo) + 1) + " / " + ImagesCollection.Count + " ] ";
            }
        }
        
        Image image;

        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;

        public ImageEditorBase(ObservableCollection<ImageInfo> imagesCollection, ImageInfo imageInfo)
        {
            InitializeComponent();

            image = new Image();

            ImagesCollection = imagesCollection;
            ImageInfo = imageInfo;

            //zoom in/out
            slider.ValueChanged += slider_ValueChanged;
            scrollViewer.PreviewMouseWheel += scrollViewer_PreviewMouseWheel;
            //drag image
            scrollViewer.ScrollChanged += scrollViewer_ScrollChanged;
            scrollViewer.PreviewMouseLeftButtonDown += scrollViewer_PreviewMouseLeftButtonDown;
            scrollViewer.MouseMove += scrollViewer_MouseMove;
            scrollViewer.PreviewMouseLeftButtonUp += scrollViewer_PreviewMouseLeftButtonUp;
            //scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewbox.Child = image;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //儲存新影像
            //    BitmapSource bitmapSource = BitmapImage;
            //    bitmapSource = new TransformedBitmap(bitmapSource, layoutTransformGroup);
            //    ImageHelper.SaveUsingEncoder(bitmapSource, ImageInfo.Image_FullPath, ImageInfo.Image_Extension);
            //    ImageInfo.BitmapImage = new CreateBitmapImage().SettingBitmapImage(ImageInfo.Image_FullPath, 800);
            //    MessageBox.Show("儲存成功", "提示", MessageBoxButton.OK);
            //}
            //catch (Exception ex)
            //{
            //    ErrorLog.ErrorMessageOutput(ex.ToString());
            //    MessageBox.Show("寫入失敗", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void Button_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    SaveFileDialog sfd = new SaveFileDialog();
            //    sfd.DefaultExt = ".png";
            //    sfd.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            //    if (sfd.ShowDialog() == true)
            //    {
            //        //儲存的副檔名
            //        string extension = sfd.SafeFileName.Substring(sfd.SafeFileName.IndexOf('.'), (sfd.SafeFileName.Length - sfd.SafeFileName.IndexOf('.')));
            //        //另存新影像
            //        BitmapSource bitmapSource = BitmapImage;
            //        bitmapSource = new TransformedBitmap(bitmapSource, layoutTransformGroup);
            //        ImageHelper.SaveUsingEncoder(bitmapSource, sfd.FileName, extension);
            //        MessageBox.Show("檔案建立成功，存放位置於" + sfd.FileName, "提示", MessageBoxButton.OK);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorLog.ErrorMessageOutput(ex.ToString());
            //    MessageBox.Show("寫入失敗", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        
        private void Button_RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            //修改完儲存
            SaveImage(new RotateTransform(-90));

            #region useless
            ////編輯與存檔分開
            //ScaleTransform newScaleTransform = (ScaleTransform)((TransformGroup)ImageEdi.LayoutTransform).Children.First(st => st is ScaleTransform);
            //RotateTransform newRotateTransform = (RotateTransform)((TransformGroup)ImageEdi.LayoutTransform).Children.First(rt => rt is RotateTransform);

            //if (newScaleTransform.ScaleY == 1)
            //{
            //    if (newScaleTransform.ScaleX == 1)
            //    {
            //        if (newRotateTransform.Angle == -360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle -= 90;
            //    }
            //    else
            //    {
            //        if (newRotateTransform.Angle == 360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle += 90;
            //    }
            //}
            //else
            //{
            //    if (newScaleTransform.ScaleX == 1)
            //    {
            //        if (newRotateTransform.Angle == 360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle += 90;
            //    }
            //    else
            //    {
            //        if (newRotateTransform.Angle == -360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle -= 90;
            //    }
            //}
            #endregion
        }
        private void Button_RotateRight_Click(object sender, RoutedEventArgs e)
        {
            //修改完儲存
            SaveImage(new RotateTransform(90));

            #region useless
            ////編輯與存檔分開
            //ScaleTransform newScaleTransform = (ScaleTransform)((TransformGroup)ImageEdi.LayoutTransform).Children.First(st => st is ScaleTransform);
            //RotateTransform newRotateTransform = (RotateTransform)((TransformGroup)ImageEdi.LayoutTransform).Children.First(rt => rt is RotateTransform);
            ////先判斷有沒有鏡射過
            ////鏡射過在旋轉結果會不一樣
            //if (newScaleTransform.ScaleY == 1)
            //{
            //    if (newScaleTransform.ScaleX == 1)
            //    {
            //        if (newRotateTransform.Angle == 360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle += 90;
            //    }
            //    else
            //    {
            //        if (newRotateTransform.Angle == -360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle -= 90;
            //    }
            //}
            //else
            //{
            //    if (newScaleTransform.ScaleX == 1)
            //    {
            //        if (newRotateTransform.Angle == -360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle -= 90;
            //    }
            //    else
            //    {
            //        if (newRotateTransform.Angle == 360)
            //            newRotateTransform.Angle = 0;
            //        newRotateTransform.Angle += 90;
            //    }
            //}
            #endregion
        }

        private void Button_MirrorHorizontal_Click(object sender, RoutedEventArgs e)
        {
            //修改完儲存
            SaveImage(new ScaleTransform(-1, 1));

            #region useless
            ////編輯與存檔分開
            //ScaleTransform newScaleTransform = (ScaleTransform)((TransformGroup)ImageEdi.LayoutTransform).Children.First(st => st is ScaleTransform);
            //if (newScaleTransform.ScaleX == 1)
            //{
            //    if (newScaleTransform.ScaleY == 1)
            //    {
            //        newScaleTransform.ScaleY = 1;
            //    }
            //    else
            //    {
            //        newScaleTransform.ScaleY = -1;
            //    }
            //    newScaleTransform.ScaleX = -1;
            //}
            //else
            //{
            //    if (newScaleTransform.ScaleY == 1)
            //    {
            //        newScaleTransform.ScaleY = 1;
            //    }
            //    else
            //    {
            //        newScaleTransform.ScaleY = -1;
            //    }
            //    newScaleTransform.ScaleX = 1;
            //}
            #endregion
        }

        private void Button_MirrorVertical_Click(object sender, RoutedEventArgs e)
        {
            //修改完儲存
            SaveImage(new ScaleTransform(1, -1));

            #region useless
            ////編輯與存檔分開
            //ScaleTransform newScaleTransform = (ScaleTransform)((TransformGroup)ImageEdi.LayoutTransform).Children.First(st => st is ScaleTransform);
            //if (newScaleTransform.ScaleY == 1)
            //{
            //    if (newScaleTransform.ScaleX == 1)
            //    {
            //        newScaleTransform.ScaleX = 1;
            //    }
            //    else
            //    {
            //        newScaleTransform.ScaleX = -1;
            //    }
            //    newScaleTransform.ScaleY = -1;
            //}
            //else
            //{
            //    if (newScaleTransform.ScaleX == 1)
            //    {
            //        newScaleTransform.ScaleX = 1;
            //    }
            //    else
            //    {
            //        newScaleTransform.ScaleX = -1;
            //    }
            //    newScaleTransform.ScaleY = 1;
            //}
            #endregion
        }

        private void Button_LastPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (ImagesCollection.IndexOf(ImageInfo) > 0)
            {
                ImageInfo = ImagesCollection[ImagesCollection.IndexOf(ImageInfo) - 1];

                GC.Collect();

                GC.WaitForPendingFinalizers();

                GC.Collect();
            }
        }

        private void Button_NextPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (ImagesCollection.IndexOf(ImageInfo) < ImagesCollection.Count - 1)
            {
                ImageInfo = ImagesCollection[ImagesCollection.IndexOf(ImageInfo) + 1];

                GC.Collect();

                GC.WaitForPendingFinalizers();

                GC.Collect();
            }
    }

        private void Button_Crop_Click(object sender, RoutedEventArgs e)
        {
            Content = new ImageEditorCrop(ImagesCollection, ImageInfo);
            GC.Collect();
        }

        private void Button_Rotate_Click(object sender, RoutedEventArgs e)
        {
            Content = new ImageEditorRotate(ImagesCollection, ImageInfo);
            GC.Collect();
        }
        private void Button_Advance_Click(object sender, RoutedEventArgs e)
        {
            Content = new ImageEditorAdvanced(ImagesCollection, ImageInfo);
        }

        private void Button_ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            slider.Value += 0.1;
        }

        private void Button_ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            slider.Value -= 0.1;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scaleTransform.ScaleX = e.NewValue;
            scaleTransform.ScaleY = e.NewValue;

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                             scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid);
            textZoomRate.Text = (e.NewValue * 100) + "%";
        }

        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(grid);

            if (e.Delta > 0)
            {
                slider.Value += 0.1;
            }
            else
            {
                slider.Value -= 0.1;
            }

            e.Handled = true;
        }

        private void scrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y <
                scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }

        private void scrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        private void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                                         scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow =
                              scrollViewer.TranslatePoint(centerOfViewport, grid);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(grid);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / grid.Width;
                    double multiplicatorY = e.ExtentHeight / grid.Height;

                    double newOffsetX = scrollViewer.HorizontalOffset -
                                        dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset -
                                        dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }

        private void SaveImage(Transform transform)
        {
            //載入原圖至BitmapSource
            BitmapSource bitmapSource = new CreateBitmapImage().SettingBitmapImage(ImageInfo.Image_FullPath, 0);
            bitmapSource = new TransformedBitmap(bitmapSource, transform);
            ImageHelper.SaveUsingEncoder(bitmapSource, ImageInfo.Image_FullPath, ImageInfo.Image_Extension);

            ImageInfo.BitmapImage = new CreateBitmapImage().SettingBitmapImage(ImageInfo.Image_FullPath, 800);
            image.Source = bitmapSource;
            bitmapSource = null;
        }
    }
}
