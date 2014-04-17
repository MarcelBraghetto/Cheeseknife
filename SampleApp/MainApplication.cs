using Android.App;
using System;
using Android.Runtime;

namespace SampleApp {
	[Application]
	public class MainApplication : Application {
		public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }
	}
}