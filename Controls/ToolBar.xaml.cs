using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_BingeBox.Models;
using WPF_BingeBox.Managers;

namespace WPF_BingeBox.Controls
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : UserControl
    {
        private Window Win;
        public FileManager FileManager;
        public ToolBar()
        {
            InitializeComponent();
        }

        private void AddSeries_Click(object sender, RoutedEventArgs e)
        {
            Win = new AddSeries(FileManager);
            Win.Show();
        }
    }
}
