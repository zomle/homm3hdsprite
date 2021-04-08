using Microsoft.Win32;
using SASpriteGen.ViewModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SASpriteGen.Wpf
{
	/// <summary>
	/// Interaction logic for HdAssetCatalogBrowser.xaml
	/// </summary>
	public partial class HdAssetCatalogBrowser : UserControl
	{
		private const string Homm3HdSteamDefaultDir = @"C:\Program Files (x86)\Steam\steamapps\common\Heroes of Might & Magic III - HD Edition\data";

		public HdAssetCatalogBrowserViewModel ViewModel { get => (HdAssetCatalogBrowserViewModel)DataContext; }

		public HdAssetCatalogBrowser()
		{
			InitializeComponent();
		}

		private void BrowseLodFileButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog()
			{
				Filter = "HOMM3 sprite lod file (h3sprite.lod)|h3sprite.lod|HOMM3 HD Edition lod file (*.lod)|*.lod|All files (*.*)|*.*",
				Title = "Open lod file",
				CheckFileExists = true,
			};

			if (Directory.Exists(Homm3HdSteamDefaultDir))
			{
				ofd.InitialDirectory = Homm3HdSteamDefaultDir;
			}

			if (ofd.ShowDialog() == true)
			{
				ViewModel.LodFilePath = ofd.FileName;
				ViewModel.LoadLodFile();
			}
		}

		private void BrowsePakx2FileButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog()
			{
				Filter = "HOMM3 sprite pak file (sprite_DXT_com_x2.pak)|sprite_DXT_com_x2.pak|HOMM3 HD Edition pak file (*.pak)|*.pak|All files (*.*)|*.*",
				Title = "Open pak file",
				CheckFileExists = true
			};

			if (ofd.ShowDialog() == true)
			{
				ViewModel.Pakx2FilePath = ofd.FileName;
				ViewModel.LoadPakx2File();
			}
		}

		private void BrowsePakx3FileButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog()
			{
				Filter = "HOMM3 sprite pak file (sprite_DXT_com_x3.pak)|sprite_DXT_com_x3.pak|HOMM3 HD Edition pak file (*.pak)|*.pak|All files (*.*)|*.*",
				Title = "Open pak file",
				CheckFileExists = true
			};

			if (ofd.ShowDialog() == true)
			{
				ViewModel.Pakx3FilePath = ofd.FileName;
				ViewModel.LoadPakx3File();
			}
		}
	}
}
