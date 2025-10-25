using System;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;

namespace TextEditor.Helpers
{
    public static class AvalonEditTextBinding
    {
        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.RegisterAttached(
                "BindableText",
                typeof(string),
                typeof(AvalonEditTextBinding),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableTextChanged));

        public static string GetBindableText(DependencyObject obj) => (string)obj.GetValue(BindableTextProperty);
        public static void SetBindableText(DependencyObject obj, string value) => obj.SetValue(BindableTextProperty, value);

        private static void OnBindableTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ICSharpCode.AvalonEdit.TextEditor editor)
            {
                // detach to avoid recursive updates
                editor.TextChanged -= Editor_TextChanged;
                editor.Text = e.NewValue as string ?? string.Empty;
                editor.TextChanged += Editor_TextChanged;
            }
        }

        private static void Editor_TextChanged(object? sender, EventArgs e)
        {
            if (sender is ICSharpCode.AvalonEdit.TextEditor editor)
            {
                var current = GetBindableText(editor);
                var newText = editor.Text;
                if (!string.Equals(current, newText))
                {
                    SetBindableText(editor, newText);
                }
            }
        }
    }
}
