using System;
using Android.OS;
using Android.Views;
using Com.Lilarcor.Cheeseknife;
using Android.Widget;
using Android.Support.V4.App;

namespace SampleApp {
	/// <summary>
	/// This fragment demonstrates how to use Cheeseknife
	/// injection within the scope of a fragment (rather
	/// than an activity). When using Cheeseknife in fragments
	/// you should 'Inject' in the 'OnCreateView', and 'Reset'
	/// in the 'OnDestroyView' to match the fragment lifecycle.
	/// You would need to do this even if you weren't using
	/// Cheeseknife anyway ... This example shows how to inject
	/// and populate a simply list of items.
	/// </summary>
	public class ListSampleFragment : Fragment {
		// Register our listView field to be injected via the 'list_view' resource ID.
		[InjectView(Resource.Id.list_view)] ListView listView;

		// Register the 'list_view' resource ID to also have the ItemClick event applied.
		[InjectOnItemClick(Resource.Id.list_view)]
		void OnListViewItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			// This code will run when an item in the listView is selected
			Toast.MakeText(Activity, "Selected list item: " + listAdapter.GetItemAtPosition(e.Position), ToastLength.Short).Show();
		}
			
		// Simple list adapter implementation (also uses Cheeseknife internally
		// to resolve its ViewHolder pattern - look in the ListSampleAdapter.cs
		// to review the code.
		ListSampleAdapter listAdapter;

		/// <summary>
		/// Static factory method to create an instance of this fragment
		/// </summary>
		/// <returns>A new instance of this fragment type.</returns>
		public static ListSampleFragment NewInstance() {
			var fragment = new ListSampleFragment {
				RetainInstance = true
			};
			return fragment;
		}

		/// <summary>
		/// Initializes the fragment, this should
		/// be called in the 'OnCreateView' method
		/// *after* Cheeseknife injection is complete.
		/// </summary>
		void InitFragment() {
			// We have set this fragment to RetainInstance = true
			// so no need to recreate our listAdapter after
			// config changes etc.
			if(listAdapter == null) {
				listAdapter = new ListSampleAdapter();
			}
			// Set our listView adapter
			listView.Adapter = listAdapter;
		}

		/// <summary>
		/// Called to inflate the fragment's view. In addition to this, we
		/// will also use this method to inject all our view fields and run
		/// any custom initialisation code ...
		/// </summary>
		/// <param name="inflater">Inflater.</param>
		/// <param name="container">Container.</param>
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			// Inflate the required fragment layout into 'view'
			var view = inflater.Inflate(Resource.Layout.list_sample_fragment, null);

			// Ask Cheeseknife to inject all our attributed fields, properties
			// and events, based on the view we just inflated.
			Cheeseknife.Inject(this, view);

			// At this point, all the Android views should be ready for use, so
			// you can safely run any other init code you need to run.
			InitFragment();

			// Hand back the inflated view.
			return view;
		}

		/// <summary>
		/// IMPORTANT: You should call Cheeseknife.Reset when
		/// the fragment view is destroyed, to null out the field
		/// references and clean them up. This is no different than
		/// having to manually null out your view fields if you
		/// weren't using Cheeseknife.
		/// </summary>
		public override void OnDestroyView() {
			base.OnDestroyView();
			Cheeseknife.Reset(this);
		}
	}
}

