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
			Width = SystemParameters.WorkArea.Width * 0.75;
			Height = SystemParameters.WorkArea.Height * 0.75;

			var version = GetType().Assembly.GetName().Version;
			Title = $"HOMM3 HD sprite to Stream Avatars sprite sheet converter {version.Major}.{version.Minor}.{version.Build}";
		}

		
		private void RegisterCollectionSynchronization(IEnumerable collection, object lockObject)
		{
			Dispatcher.Invoke(() => BindingOperations.EnableCollectionSynchronization(collection, lockObject));
		}
	}
}
