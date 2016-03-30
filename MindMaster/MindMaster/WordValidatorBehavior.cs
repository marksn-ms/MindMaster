using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MindMaster
{
    public class ExactLengthWordValidatorBehavior : Behavior<Entry>
    {
        const string wordRegex = @"^[0-9a-z]*$";
        public int ExactLength { get; set; }

        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(ExactLengthWordValidatorBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get
            {
                return (bool)base.GetValue(IsValidProperty);
            }
            private set
            {
                base.SetValue(IsValidPropertyKey, value);
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = e.NewTextValue.Trim();
            IsValid = (Regex.IsMatch(newValue, wordRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                && (ExactLength == 0 || newValue.Length == ExactLength);
            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
        }

        public ExactLengthWordValidatorBehavior(int exactLength)
            : base()
        {
            ExactLength = exactLength;
        }
    }
}
