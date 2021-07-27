using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Com.Lilarcor.Cheeseknife;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.Widget;
using System.Text;

namespace CheeseKnifeObserverSampleApp {
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity, ICheeseKnifeObserver {

		[InjectView(Resource.Id.eventHandler1Indicator)]
		public View EventHandlerRunningIndicator1;
		[InjectView(Resource.Id.eventHandler2Indicator)]
		public View EventHandlerRunningIndicator2;
		[InjectView(Resource.Id.log)]
		public TextView LogView;


		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			Cheeseknife.Inject(this);
			EventHandlerRunningIndicator1.Visibility = ViewStates.Invisible;
			EventHandlerRunningIndicator2.Visibility = ViewStates.Invisible;
		}


		[InjectOnClick(Resource.Id.clickMe1)]
		public async Task ClickMe1_OnClick(object sender, EventArgs args) {
			EventHandlerRunningIndicator1.Visibility = ViewStates.Visible;
			await Task.Delay(5000);
			EventHandlerRunningIndicator1.Visibility = ViewStates.Invisible;
		}


		// notice that async event handlers MUST be declared as "async Task" and not "async void"
		// in order to have the "AfterCheeseKnife" event being fired when the handler has really finished
		[InjectOnClick(Resource.Id.clickMe2)]
		public async Task ClickMe2_OnClick(object sender, EventArgs args) {
			EventHandlerRunningIndicator1.Visibility = ViewStates.Visible;
			await Task.Delay(5000);
			EventHandlerRunningIndicator1.Visibility = ViewStates.Invisible;
		}

		private string logText = "";
		private void WriteLog(string line) {
			logText = line + "\n\n" + logText;
			LogView.Text = logText;
		}

		private string EventBeingRun = null;


		#region ICheeseKnifeObserver implementation
		// this is an example of how the ICheeseKnifeObserver interface can be implemented in order to log
		// all clicks performed by the user or how to even block some event handlers to be fired
		public void BeforeCheeseKnifeEvent(string eventType, int resourceID, string MethodName, ref bool Continue) {
			if (eventType != "Click")
				return;

			var msg = "User has clicked view for resourceid = " + resourceID + "\n";

			if (EventBeingRun != null) {
				msg +=
					"  **** NOT launching " + MethodName + "****\n" +
					"  **** because " + EventBeingRun + "is still running! ****";
				Continue = false;
			}
			else {
				EventBeingRun = MethodName;
				msg += "starting " + MethodName + "()";
			}
			WriteLog(msg);
		}
		// this callback could be used to implement things like:
		// - measure the execution time of all event handlers
		// - log exceptions happening during event handlers execution


		public void AfterCheeseKnifeEvent(string eventType, int resourceID, string MethodName, Exception exception) {
			if (eventType != "Click")
				return;

			WriteLog("Event handler " + MethodName + "() has finished");
			EventBeingRun = null;
		}
		#endregion
	}
}
