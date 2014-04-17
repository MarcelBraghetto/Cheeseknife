using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace SampleApp {
	[Activity(Label = "ListSampleActivity")]			
	public class ListSampleActivity : ActionBarActivity {
		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.list_sample_activity);

			if(savedInstanceState == null) {
				SupportFragmentManager
					.BeginTransaction()
					.Add(Resource.Id.list_sample_fragment, ListSampleFragment.NewInstance())
					.Commit();
			}
		}
	}
}