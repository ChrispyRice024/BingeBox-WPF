using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using WPF_BingeBox.Models;
using WPF_BingeBox.Managers;

namespace WPF_BingeBox.Windows
{
    /// <summary>
    /// Interaction logic for DeleteSeries.xaml
    /// </summary>
    public partial class DeleteSeries : Window
    {

        private ObservableCollection<Series> _seriesList;
        public ObservableCollection<Series> SeriesList
        {

        }
        public DeleteSeries()
        {
            InitializeComponent();
        }

        private void DeleteLst_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
