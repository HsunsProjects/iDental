using iDental.Class;
using iDental.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iDental.iDentalClass
{
    public class LoadTemplates_Images
    {
        public void LoadAllTemplatesImages(ObservableCollection<Templates_Images> observableCollection, Grid grid, int templateImagePixelWidth)
        {
            try
            {
                if (observableCollection.Count() > 0)
                {
                    ProgressDialog progressDialog = new ProgressDialog();
                    progressDialog.Dispatcher.Invoke(() =>
                    {
                        progressDialog.PText = "圖片載入中( 0 / " + observableCollection.Count() + " )";
                        progressDialog.PMinimum = 0;
                        progressDialog.PValue = 0;
                        progressDialog.PMaximum = observableCollection.Count();
                        progressDialog.Show();
                    });
                    //multi - thread
                    Task.Factory.StartNew(() =>
                    {
                        foreach (Templates_Images ti in observableCollection)
                        {
                            if (!string.IsNullOrEmpty(ti.Image_Path) && PathCheck.IsFileExist(ti.Image_Path))
                            {
                                grid.Dispatcher.Invoke(() =>
                                {
                                    Image iTarget = (Image)grid.FindName("Image" + ti.Template_Image_Number);
                                    iTarget.Uid = ti.Template_Image_ID.ToString();
                                    iTarget.Source = new CreateBitmapImage().BitmapImageShow(ti.Image_Path, templateImagePixelWidth);
                                });

                            }
                            progressDialog.Dispatcher.Invoke(() =>
                            {
                                progressDialog.PValue++;
                                progressDialog.PText = "圖片載入中( " + progressDialog.PValue + " / " + observableCollection.Count() + " )";
                            });
                        }

                    }).ContinueWith(t =>
                    {
                        progressDialog.Dispatcher.Invoke(() =>
                        {
                            progressDialog.PText = "載入完成";
                            progressDialog.Close();
                        });

                        GC.Collect();
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                MessageBox.Show("載入樣板圖片發生錯誤", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
    }
}
