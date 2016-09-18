using System;
using System.Collections.Generic;
using System.Text;

namespace DrawToolsLib
{
	/// <summary>
	/// Defines drawing tool
	/// </summary>
	public enum ToolType
	{
		None,
		Pointer,
		Rectangle,
		Ellipse,
		//Line,
		//PolyLine,
		Text,
		Max
	};

	/// <summary>
	/// Context menu command types
	/// </summary>
	internal enum ContextMenuCommandType
	{
		SelectAll,
		UnselectAll,
		Delete, 
		DeleteAll,
		MoveToFront,
		MoveToBack,
		SetProperties
	};
}
