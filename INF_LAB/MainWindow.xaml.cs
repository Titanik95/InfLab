using Microsoft.Win32;
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
            { Method.Substitution, "Метод подстановки" },
			{ Method.Transposition, "Метод перестановки" },
			{ Method.Linear, "Линейный метод" }
		};

		Main main;

		public MainWindow()
		{
			InitializeComponent();

			main = new Main();

            methodComboBox.ItemsSource = methods;
            methodComboBox.SelectedValuePath = "Key";
            methodComboBox.DisplayMemberPath = "Value";
            methodComboBox.SelectedIndex = 0;
		}

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var method = (Method)methodComboBox.SelectedIndex;
			var res = main.Encrypt(inputTextBox.Text, method);
			if (res == null)
				return;
			outputTextBox.Text = res[0];

			if (method == Method.Linear)
				gammaTextBox.Text = res[1];
        }

        private void pickKeyButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.ShowDialog();
            keyNameTextBox.Text = ofd.FileName;
        }

        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {
            var method = (Method)methodComboBox.SelectedIndex;
            decryptOutputTextBox.Text = main.Decrypt(encryptInputTextBox.Text, method, keyNameTextBox.Text);
        }

		private void methodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (methodComboBox.SelectedIndex == -1)
				return;
			if ((Method)methodComboBox.SelectedIndex == Method.Linear)
				gammaElementsStackPanel.Visibility = Visibility.Visible;
			else
				gammaElementsStackPanel.Visibility = Visibility.Collapsed;
		}
	}
}
