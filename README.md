Cheeseknife
=========

![Logo](logo.png)

Cheeseknife! It's like a weird [Butter Knife][1]!

Inspired by the Java `Butter Knife` library for Android, Cheeseknife is a view injection library for Xamarin.Android to ease the pain of manually resolving each and every one of your Android views and events in your view lifecycles. Injection occurs at runtime rather than compile time and uses C# attributes to mark Android view member fields for injection.

Key coolness features of this library include:

 * No longer need to use `FindViewById` repeatedly in your init lifecycles - simply annotate Android view fields with `[InjectView(Resource.Id.some_resource)] TextView textView`.
 * Easily inject commonly used view events such as `Click`. For example: `[InjectOnClick(Resource.Id.some_resource)] void IWasClicked(object sender, EventArgs e) { ... }`. See the __Supported Cheeseknife Events__ section below for the list of available Cheese Knife events. Use of event injection is completely optional - you can continue to use your own lambdas/event handlers etc if you want.
 * Helps to keep the boilerplate clutter out of your view initialisation/cleanup code.

[Get Cheeseknife on Github][2]

If you like this library or find it useful, feel free to send good thoughts in my general direction!

Note: I no longer use the Xamarin tools for mobile development. This library will continue to be available however I no longer actively develop it.

Android Library Project Compatibility
------------------------------------------------

Cheeseknife is not compatible with code that is inside an Android Library project. This is due to how Android Library resource identifiers __are not final__ (whereas resource identifiers in non-library projects __are__ final), which is a requirement of using Cheeseknife annotations.

The same problem happens in Java Android dev with Android Library projects as well, for example [Butterknife][1] (which Cheeseknife is inspired from) cannot auto inject UI fields in a library project either. Visit this link for more information about non-final resource identifiers in Android Library projects:

[http://tools.android.com/tips/non-constant-fields][3]

Butterknife should work just fine for any regular Android projects though.

Including Cheeseknife in your project
--------------------------------------------------
There are a couple of ways to include Cheeseknife in your own Xamarin.Android project:

* Option 1: Copy the `Cheeseknife` library project from the demo solution and add it as a reference to your Android project.
* Option 2: Copy the `Cheeseknife.cs` source file into your own project and add it into your solution.

Usage - Activity
---------------------
```csharp
public class ExampleActivity : Activity {
	[InjectView(Resource.Id.myTextView)]
	TextView textView;
	
	[InjectOnClick(Resource.Id.myButton)]
	void OnClickMyButton(object sender, EventArgs e) {
		// This code will run when the button is clicked ...
	}

	protected override void OnCreate(Bundle bundle) {
		base.OnCreate(bundle);
		SetContentView(Resource.Layout.main_activity);
		Cheeseknife.Inject(this);
		textView.Text = "This text view reference was injected!";
	}
}
```
Usage - Fragment
-------------------------
```csharp
public class ExampleFragment : Fragment {
	[InjectView(Resource.Id.list_view)]
	ListView listView;
	
	[InjectOnItemClick(Resource.Id.list_view)]
	void OnListViewItemClick(object sender, AdapterView.ItemClickEventArgs e) {
		// This code will run when a list item is clicked ...
	}

	public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
		var view = inflater.Inflate(Resource.Layout.list_sample_fragment, null);
		Cheeseknife.Inject(this, view);
		// Do your fragment initialisation here, all views will be available...
		return view;
	}

	public override void OnDestroyView() {
		base.OnDestroyView();
		Cheeseknife.Reset(this);
	}
}
```
Usage - List adapter with ViewHolder pattern
-----------------------------------------------------------
```csharp
public class ListSampleAdapter : BaseAdapter {
	public override View GetView(int position, View convertView, ViewGroup parent) {
		ViewHolder viewHolder;

		if(convertView == null) {
			convertView = LayoutInflater
				.From(MainApplication.Context)
				.Inflate(Resource.Layout.list_sample_list_item, parent, false);
			viewHolder = new ViewHolder(convertView);
			convertView.Tag = viewHolder;
		} else {
			viewHolder = (ViewHolder)convertView.Tag;
		}

		viewHolder.TitleView.Text = listData[position];
		
		return convertView;
	}

	class ViewHolder : Java.Lang.Object {
		[InjectView(Resource.Id.title_text_view)]
		public TextView TitleView { get; private set; }

		public ViewHolder(View view) {
			Cheeseknife.Inject(this, view);
		}
	}
}
```
Supported Cheeseknife events
------------------------------------------

Cheeseknife supports some of the most common Android view events:

__Click Event - applied to View objects__

```csharp
[InjectOnClick(Resource.Id.some_view)]
void SomeMethodName(object sender, EventArgs e) { ... }
```
__Touch Event - applied to View objects__

```csharp
[InjectOnTouch(Resource.Id.some_view)]
void SomeMethodName(object sender, View.TouchEventArgs e) { ... }
```
__Long Click Event - applied to View objects__

```csharp
[InjectOnLongClick(Resource.Id.some_view)]
void SomeMethodName(object sender, View.LongClickEventArgs e) { ... }
```
__Item Click Event - applied to AdapterView objects__

```csharp
[InjectOnItemClick(Resource.Id.some_list_view)]
void SomeMethodName(object sender, AdapterView.ItemClickEventArgs e) { ... }
```
__Item Long Click Event - applied to AdapterView objects__

```csharp
[InjectOnItemLongClick(Resource.Id.some_list_view)]
void SomeMethodName(object sender, AdapterView.ItemLongClickEventArgs e) { ... }
```
__Focus Change Event - applied to View objects__

```csharp
[InjectOnFocusChange(Resource.Id.some_view)]
void SomeMethodName(object sender, View.FocusChangeEventArgs e) { ... }
```
__Checked Change Event - applied to CompoundButton objects__

```csharp
[InjectOnCheckedChange(Resource.Id.some_compound_button_view)]
void SomeMethodName(object sender, CompoundButton.CheckedChangeEventArgs e) { ... }
```
__Text Changed Event - applied to TextView objects__

```csharp
[InjectOnTextChanged(Resource.Id.some_text_view)]
void SomeMethodName(object sender, Android.Text.TextChangedEventArgs e) { ... }
```
__Text Editor Action Event - applied to TextView objects__

```csharp
[InjectOnEditorAction(Resource.Id.some_text_view)]
void SomeMethodName(object sender, TextView.EditorActionEventArgs e) { ... }
```
Adding more Cheeseknife events
--------------------------------------------

If you would like Cheeseknife to support other Android view events you can edit `Cheeseknife.cs` if you are using the source file version of the library, and add your own injection attributes. There are three steps to include a new event, for this example we will register the `Scroll` event found on `ListView` objects.

Step 1 - Create a new annotation class
----------------------------------------------------

Each injection annotation is its own class. To make a new annotation, you just need to make a new class that has the same structure as the other Cheeseknife annotation classes. For our `Scroll` event, add the following class to `Cheeseknife.cs`:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class InjectOnScroll : BaseInjectionAttribute {
	public InjectOnScroll(int resourceId) : base(resourceId) { }
}
```
Step 2 - Registering the annotation with Cheeseknife
----------------------------------------------------------------------
The annotation and string name of the Xamarin exposed event needs to be registered in the main Cheeseknife class so it can be found via reflection during injection:

```csharp
static Dictionary<Type, string> GetInjectableEvents() {
	var types = new Dictionary<Type, string>();	
	...	
	types.Add(typeof(InjectOnScroll), "Scroll");

	return types;
}
```
Step 3 - Preventing the Xamarin linker from stripping the event
-----------------------------------------------------------------------------------
By default if you don't actually reference an event in your code, the linker will strip it out during a release build which will make your app implode and probably make kittens cry somewhere.

To prevent this, we need to make a dummy reference (that will never actually be used) to `preserve` the event we want to use.

```csharp
[Preserve]
static void InjectionEventPreserver() {
	...
	new ListView(null).Scroll += (s, e) => {};
}
```
All done! How to use your new injection annotation
-------------------------------------------------------------------
You can now use your new injection annotation in your Android app. For our example you could use the following code to inject the `Scroll` event onto a ListView. Note that you need to match the signature of your custom method to be the same as if you had added the event manually via the Xamarin APIs ie  `object sender, AbsListView.ScrollEventArgs e)`:

```csharp
public class ExampleActivity : Activity {
	[InjectOnScroll(Resource.Id.list_view)]
	void OnListViewScroll(object sender, AbsListView.ScrollEventArgs e) {
		// This code will run when the list view scrolls ...
	}

	protected override void OnCreate(Bundle bundle) {
		base.OnCreate(bundle);
		SetContentView(Resource.Layout.main_activity);
		Cheeseknife.Inject(this);
	}
}
```
Licence
-----------
	Copyright 2014 Marcel Braghetto

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or 	implied.
	See the License for the specific language governing permissions and
	limitations under the License.

[1]: http://jakewharton.github.io/butterknife/
[2]: https://github.com/marcelbraghetto/cheeseknife
[3]: http://tools.android.com/tips/non-constant-fields
