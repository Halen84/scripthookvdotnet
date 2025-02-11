//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.IO;
using System.Windows.Forms;

namespace GTA
{
	public abstract class Script : IDisposable
	{
		#region Fields

		private Viewport _viewport;
		private ScriptSettings _settings;
		#endregion

		public Script()
		{
			Filename = SHVDN.ScriptDomain.CurrentDomain.LookupScriptFilename(GetType());
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
		}

		public event EventHandler Tick
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Tick += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Tick -= value;
				}
			}
		}
		public event EventHandler Aborted
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Aborted += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Aborted -= value;
				}
			}
		}

		public event KeyEventHandler KeyUp
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyUp += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyUp -= value;
				}
			}
		}
		public event KeyEventHandler KeyDown
		{
			add
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyDown += value;
				}
			}
			remove
			{
				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.KeyDown -= value;
				}
			}
		}

		public string Name
		{
			get => GetType().FullName;
		}
		public string Filename
		{
			get; private set;
		}

		public Keys ActivateKey = Keys.NumPad5;
		public Keys BackKey = Keys.NumPad0;
		public Keys LeftKey = Keys.NumPad4;
		public Keys RightKey = Keys.NumPad6;
		public Keys UpKey = Keys.NumPad8;
		public Keys DownKey = Keys.NumPad2;

		public Viewport View
		{
			get
			{
				if (_viewport != null)
				{
					return _viewport;
				}

				_viewport = new Viewport();

				Tick += (sender, e) =>
				{
					_viewport.Draw();
				};
				KeyUp += (sender, e) =>
				{
					if (e.KeyCode == ActivateKey)
					{
						_viewport.HandleActivate();
					}
					else if (e.KeyCode == BackKey)
					{
						_viewport.HandleBack();
					}
					else if (e.KeyCode == LeftKey)
					{
						_viewport.HandleChangeItem(false);
					}
					else if (e.KeyCode == RightKey)
					{
						_viewport.HandleChangeItem(true);
					}
					else if (e.KeyCode == UpKey)
					{
						_viewport.HandleChangeSelection(false);
					}
					else if (e.KeyCode == DownKey)
					{
						_viewport.HandleChangeSelection(true);
					}
				};

				return _viewport;
			}
		}
		public ScriptSettings Settings
		{
			get
			{
				if (_settings != null)
				{
					return _settings;
				}

				string path = Path.ChangeExtension(Filename, ".ini");

				_settings = ScriptSettings.Load(path);

				return _settings;
			}
		}

		protected int Interval
		{
			get => SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Interval;
			set
			{
				if (value < 0)
				{
					value = 0;
				}

				SHVDN.Script script = SHVDN.ScriptDomain.CurrentDomain.LookupScript(this);
				if (script != null)
				{
					script.Interval = value;
				}
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public void Abort()
		{
			SHVDN.ScriptDomain.CurrentDomain.LookupScript(this).Abort();
		}

		public static void Wait(int ms)
		{
			SHVDN.Script script = SHVDN.ScriptDomain.ExecutingScript;
			if (script == null || !script.IsRunning)
			{
				throw new InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");
			}

			script.Wait(ms);
		}
		public static void Yield()
		{
			Wait(0);
		}
	}
}
