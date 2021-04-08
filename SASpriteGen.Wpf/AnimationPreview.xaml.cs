using SASpriteGen.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SASpriteGen.Wpf
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class AnimationPreview : UserControl
    {
		private DispatcherTimer Timer;
		private AnimationPreviewViewModel ViewModel { get { return (AnimationPreviewViewModel)DataContext; } }

		public AnimationPreview()
        {
            InitializeComponent();

			Timer = new DispatcherTimer(DispatcherPriority.Render);
			Timer.Interval = TimeSpan.FromMilliseconds(50.0);
			Timer.Tick += (sender, args) =>
			{
				ViewModel?.AnimationTick();
			};
			Timer.Start();
		}
    }
}
