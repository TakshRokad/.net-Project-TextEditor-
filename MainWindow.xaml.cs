using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit;
using MaterialDesignThemes.Wpf;
using TextEditor.ViewModels;

namespace TextEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        try
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            ApplyTheme(isDark: true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Initialization Error: {ex.Message}\n\nStack: {ex.StackTrace}", 
                "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }

    private void Undo_Click(object sender, RoutedEventArgs e)
    {
        Editor?.Undo();
    }

    private void Redo_Click(object sender, RoutedEventArgs e)
    {
        Editor?.Redo();
    }

    private void Find_Click(object sender, RoutedEventArgs e)
    {
        DoFind();
    }

    private void DoFind()
    {
        if (Editor == null) return;
        var text = (DataContext as MainViewModel)?.SearchText;
        if (string.IsNullOrEmpty(text)) return;
        var content = Editor.Text ?? string.Empty;
        var start = Editor.CaretOffset;
        var index = content.IndexOf(text, start, System.StringComparison.OrdinalIgnoreCase);
        if (index < 0 && start > 0)
        {
            index = content.IndexOf(text, 0, System.StringComparison.OrdinalIgnoreCase);
        }
        if (index >= 0)
        {
            Editor.Select(index, text.Length);
            Editor.ScrollToLine(Editor.TextArea.Caret.Line);
            Editor.TextArea.Focus();
        }
    }

    private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
    {
        ApplyTheme(true);
    }

    private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
    {
        ApplyTheme(false);
    }

    private void ApplyTheme(bool isDark)
    {
        var helper = new PaletteHelper();
        var theme = helper.GetTheme();
        theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
        helper.SetTheme(theme);
    }
}