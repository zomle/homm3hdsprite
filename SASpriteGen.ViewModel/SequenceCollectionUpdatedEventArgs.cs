using System.Collections.Generic;

namespace SASpriteGen.ViewModel
{
	public class SequenceCollectionUpdatedEventArgs
	{
		public IEnumerable<SpriteFrameSequenceViewModel> NewSequences { get; }

		public SequenceCollectionUpdatedEventArgs(IEnumerable<SpriteFrameSequenceViewModel> newSequences)
		{
			NewSequences = newSequences;
		}
	}
}
