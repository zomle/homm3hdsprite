using ImageMagick;
using SASpriteGen.Model;
using SASpriteGen.Model.Def;
using SASpriteGen.ViewModel;
using System;
using System.Collections;
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

namespace SASpriteGen.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindowViewModel ViewModel { get { return (MainWindowViewModel)DataContext; } }

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel(RegisterCollectionSynchronization);
		}

		private void RegisterCollectionSynchronization(IEnumerable collection, object lockObject)
		{
			Dispatcher.Invoke(() => BindingOperations.EnableCollectionSynchronization(collection, lockObject));
		}
	}
}
