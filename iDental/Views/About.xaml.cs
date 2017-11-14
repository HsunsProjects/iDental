using System;
using System.Reflection;
using System.Windows;

namespace iDental.Views
{
    /// <summary>
    /// About.xaml 的互動邏輯
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            string Product = string.Empty;
            string Version = string.Empty;
            string Copyright = string.Empty;
            string Company = string.Empty;
            foreach (Attribute attr in assembly.GetCustomAttributes(false))
            {
                if (attr.GetType() == typeof(AssemblyProductAttribute))
                {
                    Product = ((AssemblyProductAttribute)attr).Product;
                }
                else if (attr.GetType() == typeof(AssemblyFileVersionAttribute))
                {
                    Version = ((AssemblyFileVersionAttribute)attr).Version;
                }
                else if (attr.GetType() == typeof(AssemblyCopyrightAttribute))
                {
                    Copyright = ((AssemblyCopyrightAttribute)attr).Copyright;
                }
                else if (attr.GetType() == typeof(AssemblyCompanyAttribute))
                {
                    Company = ((AssemblyCompanyAttribute)attr).Company;
                }
            }

            ProductVersion.Text = Product + @"　" + Version;
            CompanyCopyright.Text = Company + @"　" + Copyright;            
        }
    }
}
