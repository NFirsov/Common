namespace DrawToolsLib
{
	public class EllipseViewModel : RectBaseViewModel, IGliph
	{
		public EllipseViewModel(MainViewModel vm)
			: base(vm)
		{
		}

		public GliphType GliphType { get{return GliphType.Ellipse;} }

		public bool IsSolid { get; set; }
	}
}