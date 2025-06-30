using System;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Views.Behaviors
{
    public class PickerValidatorBehavior : Behavior<Picker>
    {
        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(PickerValidatorBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public PickerValidatorBehavior()
        {
            IsValid = true;
        }

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }

        protected override void OnAttachedTo(Picker bindable)
        {
            bindable.SelectedIndexChanged += HandlePickerSelected;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Picker bindable)
        {
            bindable.SelectedIndexChanged -= HandlePickerSelected;
            base.OnDetachingFrom(bindable);
        }

        private void HandlePickerSelected(object sender, EventArgs e)
        {
            // ReSharper disable once PossibleNullReferenceException
            IsValid = (sender as Picker).SelectedIndex < 0;
        }
    }
}
