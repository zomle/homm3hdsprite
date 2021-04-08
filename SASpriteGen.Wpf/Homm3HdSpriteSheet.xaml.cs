using SASpriteGen.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SASpriteGen.Wpf
{
	/// <summary>
	/// Interaction logic for SpriteSheet.xaml
	/// </summary>
	public partial class Homm3HdSpriteSheet : UserControl
	{
		public Homm3HdSpriteSheetViewModel ViewModel { get { return (Homm3HdSpriteSheetViewModel)DataContext; } }

		public Homm3HdSpriteSheet()
		{
			InitializeComponent();

			Loaded += SpriteSheet_Loaded;
		}

		private void SpriteSheet_Loaded(object sender, RoutedEventArgs e)
		{
			//if (ViewModel != null)
			//{
			//	BindingOperations.EnableCollectionSynchronization(ViewModel.Sequences, ViewModel.SynchronizationLock);
			//}
		}
	}
}
