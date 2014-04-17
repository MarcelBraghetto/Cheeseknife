using Android.Widget;
using System.Collections.Generic;
using Android.Views;
using Com.Lilarcor.Cheeseknife;

namespace SampleApp {
	/// <summary>
	/// List sample adapter - mainly to show how to
	/// use Cheeseknife injection via the ViewHolder
	/// list adapter pattern.
	/// </summary>
	public class ListSampleAdapter : BaseAdapter {
		List<string> data;

		/// <summary>
		/// Initializes a new instance of the <see cref="SampleApp.ListSampleAdapter"/> class.
		/// For this example it just creates a bunch of dummy list item data.
		/// </summary>
		public ListSampleAdapter() {
			data = new List<string>();
			for(var i = 1; i <= 100; i++) {
				data.Add("List item " + i);
			}
		}
			
		public string GetItemAtPosition(int position) { return data[position]; }
		public override int Count { get { return data.Count; } }
		public override Java.Lang.Object GetItem(int position) { return data[position]; }
		public override long GetItemId(int position) { return position; }

		/// <summary>
		/// Gets the view for the list item, by using the
		/// ViewHolder pattern and Cheeseknife to inject
		/// the ViewHolder views.
		/// </summary>
		/// <returns>The list item view.</returns>
		/// <param name="position">List item position.</param>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent view group.</param>
		public override View GetView(int position, View convertView, ViewGroup parent) {
			// Use a recycled ViewHolder instance
			ViewHolder viewHolder;

			// If convertView is null, we need to initialise both the
			// list item view, and instantiate the ViewHolder
			if(convertView == null) {
				// Inflate the layout for the list item
				convertView = LayoutInflater.From(MainApplication.Context).Inflate(Resource.Layout.list_sample_list_item, parent, false);
				// Create the ViewHolder, passing in the inflated view
				viewHolder = new ViewHolder(convertView);
				// Set the list item view tag to the view holder
				convertView.Tag = viewHolder;
			} else {
				// If the list item view is being recycled, simply
				// grab its tag which should be the recycled
				// ViewHolder ...
				viewHolder = (ViewHolder)convertView.Tag;
			}

			// To populate the list item, simply manipulate
			// the public properties of the view holder instance
			viewHolder.TitleView.Text = data[position];

			// Hand back the resulting list item view
			return convertView;
		}
			
		/// <summary>
		/// Class implementation of the ViewHolder pattern.
		/// The constructor expects to be passed the parent
		/// view from which it can resolve member views
		/// from for the list item.
		/// </summary>
		class ViewHolder : Java.Lang.Object {
			// Declare all the injectable view properties,
			// you could also include injectable events here too.
			[InjectView(Resource.Id.title_text_view)] public TextView TitleView { get; private set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="SampleApp.ListSampleAdapter+ViewHolder"/> class.
			/// </summary>
			/// <param name="view">View that represents the list item.</param>
			public ViewHolder(View view) {
				// Simply call Cheeseknife to resolve all the
				// child views of this ViewHolder object ...
				// sweet huh?
				Cheeseknife.Inject(this, view);
			}
		}
	}
}