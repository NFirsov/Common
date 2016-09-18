namespace DrawToolsLib
{
	public class RectangleViewModel : RectBaseViewModel, IGliph
	{
		public RectangleViewModel(MainViewModel vm)
			: base(vm)
		{
		}

		public GliphType GliphType { get { return GliphType.Rectangle; } }

		public bool IsSolid { get; set; }
	}
}