using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Views.Behaviors
{
    public static class ListViewBehavior
    {
        public static readonly BindableProperty NoBackgroundSelectionProperty =
            BindableProperty.CreateAttached(
                "NoBackgroundSelection",
                typeof(bool),
                typeof(ListViewBehavior),
                false,
                propertyChanged: OnNoBackgroundSelectionChanged);

        public static bool GetNoBackgroundSelection(BindableObject view)
        {
            return (bool)view.GetValue(NoBackgroundSelectionProperty);
        }

        public static void SetNoBackgroundSelection(BindableObject view, bool value)
        {
            view.SetValue(NoBackgroundSelectionProperty, value);
        }

        static void OnNoBackgroundSelectionChanged(BindableObject view, object oldValue, object newValue)
        {
            var listView = view as ListView;
            if (listView == null)
            {
                return;
            }

            bool NoBackgroundSelection = (bool)newValue;
            if (NoBackgroundSelection)
            {
                listView.ItemSelected += OnListViewItemSelected;
            }
            else
            {
                listView.ItemSelected -= OnListViewItemSelected;
            }
        }

        private static void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                listView.ItemSelected -= OnListViewItemSelected;
                listView.SelectedItem = null;
                listView.ItemSelected += OnListViewItemSelected;
            }
        }
    }
}
