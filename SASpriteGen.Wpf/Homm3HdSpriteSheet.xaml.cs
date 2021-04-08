using SASpriteGen.ViewModel;
using System.Windows.Controls;

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
		}
	}
}
