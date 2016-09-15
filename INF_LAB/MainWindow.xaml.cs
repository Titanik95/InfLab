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

namespace INF_LAB
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        Dictionary<Method, string> methods = new Dictionary<Method, string>()
        {
            { Method.Substitution, "Метод подстановки" }
        };

		Main main;

		public MainWindow()
		{
			InitializeComponent();

			main = new Main();

            methodComboBox.ItemsSource = methods;
            methodComboBox.SelectedValuePath = "Key";
            methodComboBox.DisplayMemberPath = "Value";
		}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            main.Encrypt("123", Method.Substitution);
        }
    }
}
