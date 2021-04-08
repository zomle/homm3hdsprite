using Microsoft.Win32;
using SASpriteGen.Model.Def;
using SASpriteGen.Model.Pak;
using SASpriteGen.ViewModel;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

namespace SASpriteGen.Wpf
{
	/// <summary>
	/// Interaction logic for OpenHdFilesDialog.xaml
	/// </summary>
	public partial class OpenHdFilesDialog : Window
	{
		public OpenHdFilesDialogViewModel ViewModel { get => (OpenHdFilesDialogViewModel)DataContext; }

		public OpenHdFilesDialog()
		{
			InitializeComponent();

			DataContext = new OpenHdFilesDialogViewModel(RegisterCollectionSynchronization);
		}

		private void RegisterCollectionSynchronization(IEnumerable collection, object lockObject)
		{
			Dispatcher.Invoke(() => BindingOperations.EnableCollectionSynchronization(collection, lockObject));
		}

		public (DefFile, HdAssetCatalogItem) GetSelection()
		{
			return ViewModel.AssetCatalogViewModel.GetSelection();
		}

		private void UseSelectedButton_Click(object sender, RoutedEventArgs e)
		{
			if (ViewModel.AssetCatalogViewModel.SelectedItem == null)
			{
				MessageBox.Show("Nothing is selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			DialogResult = true;
			Close();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}		
	}
}
