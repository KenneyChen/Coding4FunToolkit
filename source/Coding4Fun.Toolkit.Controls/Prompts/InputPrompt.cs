﻿using System;
using System.Threading;
#if WINDOWS_STORE || WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;
using Windows.System;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
#endif

using Coding4Fun.Toolkit.Controls.Binding;
using Coding4Fun.Toolkit.Controls.Common;
using System.Threading.Tasks;

namespace Coding4Fun.Toolkit.Controls
{
    public class InputPrompt : UserPrompt
    {
        protected TextBox InputBox;
        
        private const string InputBoxName = "inputBox";

        public InputPrompt()
        {
            DefaultStyleKey = typeof (InputPrompt);
        }

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        protected override async void OnApplyTemplate()
#elif WINDOWS_PHONE
		public override async void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

            InputBox = GetTemplateChild(InputBoxName) as TextBox;

            if (InputBox != null)
            {
                // manually adding
                // GetBindingExpression doesn't seem to respect TemplateBinding
                // so TextBoxBinding's code doesn't fire

#if WINDOWS_STORE || WINDOWS_PHONE_APP
                var binding = new Windows.UI.Xaml.Data.Binding();
#elif WINDOWS_PHONE
			    var binding = new System.Windows.Data.Binding();
#endif

                binding.Source = InputBox;
                binding.Path = new PropertyPath("Text");

                SetBinding(ValueProperty, binding);

                TextBinding.SetUpdateSourceOnChange(InputBox, true);
                
                HookUpEventForIsSubmitOnEnterKey();

				if (!ApplicationSpace.IsDesignMode)
	                await DelayInputSelect();
            }
        }

		#region Control Events
		#endregion

		#region helper methods
		private async Task DelayInputSelect()
		{
			await Task.Delay(250);
#if WINDOWS_STORE || WINDOWS_PHONE_APP
            await ApplicationSpace.CurrentDispatcher.RunAsync(CoreDispatcherPriority.Normal,
#elif WINDOWS_PHONE
            ApplicationSpace.CurrentDispatcher.BeginInvoke(
#endif
            () =>
			{
				InputBox.Focus
                    (
#if WINDOWS_STORE || WINDOWS_PHONE_APP
                        FocusState.Programmatic
#endif
                    );
				InputBox.SelectAll();
			});
		}

		private void HookUpEventForIsSubmitOnEnterKey()
		{
			InputBox = GetTemplateChild(InputBoxName) as TextBox;

			if (InputBox == null)
				return;

			InputBox.KeyDown -= InputBoxKeyDown;

			if (IsSubmitOnEnterKey)
				InputBox.KeyDown += InputBoxKeyDown;
		}

#if WINDOWS_STORE || WINDOWS_PHONE_APP
        private void InputBoxKeyDown(object sender, KeyRoutedEventArgs e)

#elif WINDOWS_PHONE
		void InputBoxKeyDown(object sender, KeyEventArgs e)
#endif
		{
			if (e.Key == 
#if WINDOWS_STORE || WINDOWS_PHONE_APP
                VirtualKey.Enter
#elif WINDOWS_PHONE
                Key.Enter
#endif
                )
				OnCompleted(new PopUpEventArgs<string, PopUpResult> { Result = Value, PopUpResult = PopUpResult.Ok });
		}
		#endregion

		#region Dependency Property Callbacks
		private static void OnIsSubmitOnEnterKeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var inputPrompt = d as InputPrompt;

			if (inputPrompt != null)
				inputPrompt.HookUpEventForIsSubmitOnEnterKey();
		}
		#endregion

		#region Dependency Properties / Properties
		#region public bool IsSubmitOnEnterKey
		public bool IsSubmitOnEnterKey
		{
			get { return (bool)GetValue(IsSubmitOnEnterKeyProperty); }
			set { SetValue(IsSubmitOnEnterKeyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsSubmitOnEnterKey.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsSubmitOnEnterKeyProperty =
			DependencyProperty.Register("IsSubmitOnEnterKey", typeof(bool), typeof(InputPrompt), new PropertyMetadata(true, OnIsSubmitOnEnterKeyPropertyChanged));
		#endregion
		#region public TextWrapping MessageTextWrapping
		public TextWrapping MessageTextWrapping
		{
			get { return (TextWrapping)GetValue(MessageTextWrappingProperty); }
			set { SetValue(MessageTextWrappingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MessageTextWrapping.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MessageTextWrappingProperty =
			DependencyProperty.Register("MessageTextWrapping", typeof(TextWrapping), typeof(InputPrompt), new PropertyMetadata(TextWrapping.NoWrap));
		#endregion
		#region public InputScope InputScope
		/// <summary>
		/// Gets or sets the
		/// <see cref="T:System.Windows.Input.InputScope"/>
		/// used by the Text template part.
		/// </summary>
		public InputScope InputScope
		{
			get { return (InputScope)GetValue(InputScopeProperty); }
			set { SetValue(InputScopeProperty, value); }
		}

		/// <summary>
		/// Identifies the
		/// <see cref="T:System.Windows.Input.InputScope"/>
		/// dependency property.
		/// </summary>
		public static readonly DependencyProperty InputScopeProperty =
			DependencyProperty.Register("InputScope", typeof(InputScope), typeof(InputPrompt), null);
		#endregion
		#endregion
	}
}
