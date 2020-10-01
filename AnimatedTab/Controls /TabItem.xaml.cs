using System;
using Xamarin.Forms;

namespace AnimatedTab.Controls
{
    public partial class TabItem : ContentView
    {
        public Label Current { get; set; }
        public Label Next { get; set; }
        public Point AnimationOffset { get; set; }
        private bool tapped;

        public static readonly BindableProperty CurrentTextProperty = BindableProperty.Create(nameof(CurrentText), typeof(string), typeof(TabItem), default(string), propertyChanged: OnTextChanged);

        public string CurrentText
        {
            get
            {
                return (string)GetValue(CurrentTextProperty);
            }

            set
            {
                SetValue(CurrentTextProperty, value);
            }
        }

        public static readonly BindableProperty NextTextProperty = BindableProperty.Create(nameof(NextText), typeof(string), typeof(TabItem), default(string), propertyChanged: OnNextTextChanged);

        public string NextText
        {
            get
            {
                return (string)GetValue(NextTextProperty);
            }

            set
            {
                SetValue(NextTextProperty, value);
            }
        }

        static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var control = (TabItem)bindable;

                var value = (string)newValue;

                control.CurrentLabel.Text = value;
            }
            catch (Exception ex)
            {
                // TODO: Handle exception.
            }
        }

        static void OnNextTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var control = (TabItem)bindable;

                var value = (string)newValue;

                control.NextLabel.Text = value;
            }
            catch (Exception ex)
            {
                // TODO: Handle exception.
            }
        }

        public TabItem()
        {
            InitializeComponent();
            Current = CurrentLabel;
            Next = NextLabel;

            CurrentLabel.Text = CurrentText;
            NextLabel.Text = NextText;

            MessagingCenter.Subscribe<TabItem>(this, "tapped", (sender) =>
            {
                if (tapped)
                {
                    AnimateDownText();
                    tapped = false;
                }
            });
        }

        async void AnimateUpText()
        {
            // update the labels
            Current.Text = CurrentLabel.Text;
            Current.TranslationY = 0;
            Current.TranslationX = 0;
            Current.Opacity = 1;

            // set the starting positions
            Current.TranslationY = 0;
            _ = Current.TranslateTo(0, -25);
            _ = Current.FadeTo(0);

            // animate in the next label
            Next.Text = NextLabel.Text;
            Next.TranslationY = 25;
            Next.TranslationX = AnimationOffset.X;
            Next.Opacity = 0;
            _ = Next.TranslateTo(0, 0);
            await Next.FadeTo(1);

            // recycle the views
            //Current = NextLabel;
            //Next = CurrentLabel;
        }

        async void AnimateDownText()
        {
            // update the labels
            Next.Text = NextLabel.Text;
            Next.TranslationY = 0;
            Next.TranslationX = 0;
            Next.Opacity = 1;

            // set the starting positions
            Next.TranslationY = 0;
            _ = Next.TranslateTo(0, 25);
            _ = Next.FadeTo(0);

            // animate in the next label
            Current.Text = CurrentLabel.Text;
            Current.TranslationY = -25;
            Current.TranslationX = AnimationOffset.X;
            Current.Opacity = 0;
            _ = Current.TranslateTo(0, 0);
            await Current.FadeTo(1);
        }

        void Menu_Tapped(System.Object sender, System.EventArgs e)
        {
            MessagingCenter.Send<TabItem>(this, "tapped");
            try
            {
                if (tapped)
                {
                    AnimateDownText();
                    tapped = false;
                }
                else
                {
                    AnimateUpText();
                    tapped = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
