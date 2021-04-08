using Microsoft.Win32;
using SASpriteGen.ViewModel;
using System.Windows.Controls;

namespace SASpriteGen.Wpf
{
	/// <summary>
	/// Interaction logic for StreamAvatarsSpriteSheet.xaml
	/// </summary>
	public partial class StreamAvatarsSpriteSheet : UserControl
    {
		public StreamAvatarsSpriteSheetViewModel ViewModel { get => (StreamAvatarsSpriteSheetViewModel)DataContext; }

		public StreamAvatarsSpriteSheet()
        {
            InitializeComponent();
        }

		private void ExportAsSpriteSheet_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (!ViewModel.ValidateExportSettings())
			{
				return;
			}

			SaveFileDialog ofd = new SaveFileDialog()
			{
				Filter = "Sprite sheet file (*.png)|*.png|All files (*.*)|*.*",
				Title = "Save sprite sheet",
				OverwritePrompt = true,
			};

			if (ofd.ShowDialog() == true)
			{
				ViewModel.ExportAsSpriteSheet(ofd.FileName);
			}
		}
	}
}
