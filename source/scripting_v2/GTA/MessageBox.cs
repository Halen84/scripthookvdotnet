//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MessageBox : MenuBase
	{
		private UIRectangle rectNo = null;
		private UIRectangle rectYes = null;
		private UIRectangle rectBody = null;
		private UIText text = null;
		private UIText textNo = null;
		private UIText textYes = null;
		private bool selection = true;

		public MessageBox(string headerCaption)
		{
			HeaderTextColor = Color.White;
			HeaderFont = Font.HouseScript;
			HeaderTextScale = 0.5f;
			HeaderCentered = true;
			SelectedItemColor = Color.FromArgb(200, 255, 105, 180);
			UnselectedItemColor = Color.FromArgb(200, 176, 196, 222);
			SelectedTextColor = Color.Black;
			UnselectedTextColor = Color.DarkSlateGray;
			ItemFont = Font.ChaletLondon;
			ItemTextScale = 0.4f;
			ItemTextCentered = true;
			Caption = headerCaption;

			Width = 200;
			Height = 50;
			ButtonHeight = 30;
			OkCancel = false;
		}

		public override void Draw()
		{
			Draw(default);
		}
		public override void Draw(Size offset)
		{
			rectBody.Draw(offset);
			text.Draw(offset);
			rectYes.Draw(offset);
			rectNo.Draw(offset);
			textYes.Draw(offset);
			textNo.Draw(offset);
		}

		public override void Initialize()
		{
			rectNo = new UIRectangle(
				new Point(Width / 2, Height),
				new Size(Width / 2, ButtonHeight),
				UnselectedItemColor);
			rectYes = new UIRectangle(
				new Point(0, Height),
				new Size(Width / 2, ButtonHeight),
				UnselectedItemColor);
			rectBody = new UIRectangle(
				default,
				new Size(Width, Height), HeaderColor);
			text = new UIText(
				Caption,
				HeaderCentered ? new Point(Width / 2, 0) : default,
				HeaderTextScale,
				HeaderTextColor,
				HeaderFont,
				HeaderCentered);
			textNo = new UIText(
				OkCancel ? "Cancel" : "No",
				new Point(Width / 4 * 3, Height),
				ItemTextScale,
				UnselectedTextColor,
				ItemFont,
				ItemTextCentered);
			textYes = new UIText(
				OkCancel ? "OK" : "Yes",
				new Point(Width / 4, Height),
				ItemTextScale,
				UnselectedTextColor,
				ItemFont,
				ItemTextCentered);

			OnChangeItem(false);
		}

		public override void OnOpen()
		{
		}
		public override void OnClose()
		{
		}
		public override void OnActivate()
		{
			if (!selection)
			{
				No(this, EventArgs.Empty);
			}
			else
			{
				Yes(this, EventArgs.Empty);
			}

			Parent.PopMenu();
		}

		public override void OnChangeItem(bool right)
		{
			selection = !selection;

			if (selection)
			{
				rectNo.Color = UnselectedItemColor;
				rectYes.Color = SelectedItemColor;
				textNo.Color = UnselectedTextColor;
				textYes.Color = SelectedTextColor;
			}
			else
			{
				rectNo.Color = SelectedItemColor;
				rectYes.Color = UnselectedItemColor;
				textNo.Color = SelectedTextColor;
				textYes.Color = UnselectedTextColor;
			}
		}
		public override void OnChangeSelection(bool down)
		{
		}

		public event EventHandler<EventArgs> No;
		public event EventHandler<EventArgs> Yes;

		public int Width
		{
			get; set;
		}
		public int Height
		{
			get; set;
		}
		public int ButtonHeight
		{
			get; set;
		}

		/** Use Ok and Cancel instead of Yes and No */
		public bool OkCancel
		{
			get; set;
		}
	}
}
