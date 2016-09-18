using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DrawToolsLib
{
	public class Model
	{
		public event Action<IEnumerable<IBaseGliph>> GliphsLoaded;

		private readonly Dictionary<int, BaseGliph> _gliphs = new Dictionary<int, BaseGliph>();

		/// <summary>
		/// Save model
		/// </summary>
		public void Save()
		{
			try
			{
				var xml = new XmlSerializer(typeof(SerializableModel));

				using (var stream = new FileStream("model.xml", FileMode.Create, FileAccess.Write, FileShare.None))
				{
					xml.Serialize(stream, new SerializableModel() {Gliphs = _gliphs.Values.ToList()});
				}
			}
			catch (IOException e)
			{
				throw new Exception(e.Message, e);
			}
			catch (InvalidOperationException e)
			{
				throw new Exception(e.Message, e);
			}
		}

		/// <summary>
		/// Load model
		/// </summary>
		public void Load()
		{
			try
			{
				var model = new SerializableModel();

				if (File.Exists("model.xml"))
				{
					var xml = new XmlSerializer(typeof (SerializableModel));
					using (var stream = new FileStream("model.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						model = (SerializableModel) xml.Deserialize(stream);
					}
				}

				if (model.Gliphs == null || model.Gliphs.Count == 0)
					return;

				_gliphs.Clear();
				model.Gliphs.ForEach(g => _gliphs.Add(g.Id, g));

				if (GliphsLoaded != null)
					GliphsLoaded(model.Gliphs);
			}
			catch (IOException e)
			{
				throw new Exception(e.Message, e);
			}
			catch (InvalidOperationException e)
			{
				throw new Exception(e.Message, e);
			}
			catch (ArgumentNullException e)
			{
				throw new Exception(e.Message, e);
			}
		}

		public void OnCreate(IBaseGliph bg)
		{
			RectangleBaseGliph gliph;

			var gl = bg as IGliph;
			if (gl != null)
			{
				gliph = new Gliph()
				{
					GliphType = gl.GliphType,
					IsSolid = gl.IsSolid
				};

				SetRectBaseProperties(gliph, gl);
				_gliphs.Add(gliph.Id,gliph);
				return;
			}

			var text = bg as ITextGliph;
			if (text != null)
			{
				gliph = SetTextProperties(new TextGliph(), text);
				SetRectBaseProperties(gliph, text);
				_gliphs.Add(gliph.Id, gliph);
				return;
			}

			var image = bg as IImageGliph;
			if (image != null)
			{
				gliph = new ImageGliph()
				{
					Path = image.Path
				};

				SetRectBaseProperties(gliph, image); 
				_gliphs.Add(gliph.Id, gliph);
				return;
			}
		}

		public void OnChange(IBaseGliph bg)
		{
			var g = _gliphs[bg.Id]; //throws Exception if no such key 

			var gl = bg as IGliph;
			if (gl != null)
			{
				var gliph = g as Gliph;
				gliph.GliphType = gl.GliphType; //throws Exception if not Gliph
				gliph.IsSolid = gl.IsSolid;
				SetRectBaseProperties(gliph, gl);
				return;
			}

			var text = bg as ITextGliph;
			if (text != null)
			{
				var tGliph = g as TextGliph;
				SetTextProperties(tGliph, text);
				SetRectBaseProperties(tGliph, text);
				return;
			}

			var image = bg as IImageGliph;
			if (image != null)
			{
				var imGliph = g as ImageGliph;
				imGliph.Path = image.Path;
				SetRectBaseProperties(imGliph, image);
				return;
			}
		}

		public void OnDelete(IBaseGliph bg)
		{
			_gliphs.Remove(bg.Id);
		}

		private static void SetRectBaseProperties(RectangleBaseGliph gliph, IRectangleBaseGliph igliph)
		{
			gliph.Id = igliph.Id;
			gliph.LineWidth = igliph.LineWidth;
			gliph.A = igliph.A;
			gliph.R = igliph.R;
			gliph.G = igliph.G;
			gliph.B = igliph.B;

			gliph.Top = igliph.Top;
			gliph.Left = igliph.Left;
			gliph.Height = igliph.Height;
			gliph.Width = igliph.Width;
		}

		private static TextGliph SetTextProperties(TextGliph gliph, ITextGliph text)
		{
			gliph.Text = text.Text;
			gliph.TextFontFamilyName = text.TextFontFamilyName;
			gliph.TextFontStyle = text.TextFontStyle;
			gliph.TextFontWeight = text.TextFontWeight;
			gliph.TextFontStretch = text.TextFontStretch;
			gliph.TextFontSize = text.TextFontSize;

			return gliph;
		}

		public class SerializableModel
		{
			public List<BaseGliph> Gliphs;
		}
	}

	#region Interfaces

	public interface IBaseGliph
	{
		int Id { get; }

		double LineWidth { get; }
		byte A { get; }
		byte R { get; }
		byte G { get; }
		byte B { get; }
	}

	public interface IRectangleBaseGliph : IBaseGliph
	{
		double Top { get; }
		double Left { get; }
		double Height { get; }
		double Width { get; }
	}

	public interface IGliph : IRectangleBaseGliph
	{
		GliphType GliphType { get; }
		bool IsSolid { get; }
	}

	public interface ITextGliph : IRectangleBaseGliph
	{
		string Text { get; }
		string TextFontFamilyName { get; }
		FontStyle TextFontStyle { get; }
		FontWeight TextFontWeight { get; }
		FontStretch TextFontStretch { get; }
		double TextFontSize { get; }
	}

	public interface IImageGliph : IRectangleBaseGliph
	{
		string Path { get; }
	}

	#endregion

	[XmlInclude(typeof(Gliph))]
	[XmlInclude(typeof(TextGliph))]
	[XmlInclude(typeof(ImageGliph))]
	public abstract class BaseGliph : IBaseGliph
	{
		public int Id { get; set; }

		public double LineWidth { get; set; }

		public byte A { get; set; }
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }
	}

	public abstract class RectangleBaseGliph : BaseGliph, IRectangleBaseGliph
	{
		public double Top { get; set; }
		public double Left { get; set; }
		public double Height { get; set; }
		public double Width { get; set; }
	}

	public class Gliph : RectangleBaseGliph, IGliph
	{
		public GliphType GliphType { get; set; }
		public bool IsSolid { get; set; }
	}

	public class TextGliph : RectangleBaseGliph, ITextGliph
	{
		public string Text { get; set; }
		public string TextFontFamilyName { get; set; }
		public FontStyle TextFontStyle { get; set; }
		public FontWeight TextFontWeight { get; set; }
		public FontStretch TextFontStretch { get; set; }
		public double TextFontSize { get; set; }
	}

	public class ImageGliph : RectangleBaseGliph, IImageGliph
	{
		public string Path { get; set; }
	}

	public enum GliphType
	{
		Triangle,
		Rectangle,
		Ellipse
	}

}
