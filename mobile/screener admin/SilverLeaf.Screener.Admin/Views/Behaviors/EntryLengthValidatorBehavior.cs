using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Views.Behaviors
{
    public class EntryLengthValidatorBehavior : Behavior<Entry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
            bindable.BindingContextChanged +=
                (sender, _) => BindingContext = ((BindableObject)sender).BindingContext;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text == null || entry.Text.Length <= MaxLength)
            {
                return;
            }

            var unused = entry.Text;
            entry.TextChanged -= OnEntryTextChanged;
            entry.Text = e.OldTextValue;
            entry.TextChanged += OnEntryTextChanged;
        }
    }
}
