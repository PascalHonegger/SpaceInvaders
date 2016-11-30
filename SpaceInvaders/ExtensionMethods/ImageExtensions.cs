using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32.SafeHandles;

namespace SpaceInvaders.ExtensionMethods
{
	/// <summary>
	///     Here all Extensions Methods are stored
	/// </summary>
	public static class NativeMethods
	{
		private static readonly Dictionary<string, BitmapSource> CachedBitmapSources = new Dictionary<string, BitmapSource>();

		/// <summary>
		///     Transforms a Bitmap to a BitmapSource
		/// </summary>
		/// <param name="source">Bitmap to be transformed</param>
		/// <param name="key">String Key used for identifing the image (caching)</param>
		/// <returns>BitmapSource equivalent to the Bitmap-Input</returns>
		public static BitmapSource ToBitmapSource(this Bitmap source, string key)
		{
			if (CachedBitmapSources.TryGetValue(key, out BitmapSource value))
			{
				return value;
			}

			using (var handle = new SafeHBitmapHandle(source))
			{
				var bitMapSource = Imaging.CreateBitmapSourceFromHBitmap(handle.DangerousGetHandle(),
					IntPtr.Zero, Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());

				CachedBitmapSources.Add(key, bitMapSource);

				return bitMapSource;
			}
		}

		[DllImport("gdi32")]
		private static extern int DeleteObject(IntPtr o);

		private sealed class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			[SecurityCritical]
			public SafeHBitmapHandle(Bitmap bitmap)
				: base(true)
			{
				SetHandle(bitmap.GetHbitmap());
			}

			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			protected override bool ReleaseHandle()
			{
				return DeleteObject(handle) > 0;
			}
		}
	}
}