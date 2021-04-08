using SASpriteGen.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace SASpriteGen.Wpf
{
	/// <summary>
	/// Interaction logic for SpriteFrameCollection.xaml
	/// </summary>
	public partial class SpriteFrameSequence : UserControl
	{
		private DispatcherTimer Timer;
		private SpriteFrameSequenceViewModel ViewModel { get { return (SpriteFrameSequenceViewModel)DataContext;  } }

		public SpriteFrameSequence()
		{
			InitializeComponent();

			Timer = new DispatcherTimer(DispatcherPriority.Render);
			Timer.Interval = TimeSpan.FromMilliseconds(100.0);
			Timer.Tick += (sender, args) =>
			{
				ViewModel.AnimationTick();
			};

			Timer.Start();
		}
	}
}
