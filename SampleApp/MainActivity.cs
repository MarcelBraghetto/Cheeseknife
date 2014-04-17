using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Com.Lilarcor.Cheeseknife;
using Android.Support.V7.App;

namespace SampleApp {
	[Activity(Label = "SampleApp", MainLauncher = true)]
	public class MainActivity : ActionBarActivity {
		// Include the 'InjectView' attribute for any Android
		// view fields you would like to resolve with Cheeseknife.
		[InjectView(Resource.Id.myTextView)] TextView textView;

		// Include the 'InjectOnXXXXX' attributes for any Android
		// view fields you would like to attach common events to.
		// For example, the following attribute attaches the
		// 'Click' event to a resource with the 'myButton' ID.
		// Check the Cheeseknife documentation or source code
		// to see event types that Cheeseknife currently supports.
		[InjectOnClick(Resource.Id.myButton)]
		void OnClickMyButton(object sender, EventArgs e) {
			// This code will run when the button is clicked ...
			StartActivity(typeof(ListSampleActivity));
		}

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Inflate the activity layout resource
			SetContentView(Resource.Layout.main_activity);

			// Use Cheeseknife to inject all attributed view
			// fields and events. For an activity injection,
			// simply pass in the reference to this activity.
			Cheeseknife.Inject(this);

			// After the Cheeseknife injection, you can acces
			// all your views as you would normally do.
			textView.Text = "This text view reference was injected!";
		}
	}
}