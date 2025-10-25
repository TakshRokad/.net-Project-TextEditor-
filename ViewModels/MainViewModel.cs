using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TextEditor.Helpers;
using TextEditor.Models;
using TextEditor.Services;

namespace TextEditor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DocumentService _docService;

        public ObservableCollection<DocumentViewModel> Documents { get; } = new();

        private DocumentViewModel? _selectedDocument;
        public DocumentViewModel? SelectedDocument
        {
            get => _selectedDocument;
            set { _selectedDocument = value; OnPropertyChanged(); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        private bool _isDark = true;
        public bool IsDark
        {
            get => _isDark;
            set { _isDark = value; OnPropertyChanged(); }
        }

        public ICommand NewCommand { get; }
        public ICommand RenameCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public MainViewModel()
        {
            var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TextEditor");
            var fileService = new FileService(dataPath);
            _docService = new DocumentService(fileService);

            var first = _docService.CreateNew("default");
            var dvm = new DocumentViewModel(first);
            Documents.Add(dvm);
            SelectedDocument = dvm;

            NewCommand = new RelayCommand(_ => NewDocument());
            RenameCommand = new RelayCommand(_ => RenameCurrent(), _ => SelectedDocument != null);
            DeleteCommand = new RelayCommand(_ => DeleteCurrent(), _ => SelectedDocument != null);
            ExportCommand = new RelayCommand(async _ => await ExportCurrentAsync(), _ => SelectedDocument != null);
            ImportCommand = new RelayCommand(async _ => await ImportAsync());
            ToggleThemeCommand = new RelayCommand(_ => IsDark = !IsDark);
        }

        private void NewDocument()
        {
            var model = _docService.CreateNew("untitled");
            var vm = new DocumentViewModel(model);
            Documents.Add(vm);
            SelectedDocument = vm;
        }

        private void RenameCurrent()
        {
            if (SelectedDocument == null) return;
            var input = Microsoft.VisualBasic.Interaction.InputBox("Enter new name", "Rename Document", SelectedDocument.Name);
            if (string.IsNullOrWhiteSpace(input)) return;
            SelectedDocument.Name = _docService.EnsureUniqueName(input.Trim());
        }

        private void DeleteCurrent()
        {
            if (SelectedDocument == null) return;
            var result = MessageBox.Show($"Delete '{SelectedDocument.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            _docService.Remove(new Document { Id = SelectedDocument.Id });
            Documents.Remove(SelectedDocument);
            SelectedDocument = Documents.Count > 0 ? Documents[0] : null;
            if (SelectedDocument == null)
            {
                var model = _docService.CreateNew("untitled");
                var vm = new DocumentViewModel(model);
                Documents.Add(vm);
                SelectedDocument = vm;
            }
        }

        private async Task ExportCurrentAsync()
        {
            if (SelectedDocument == null) return;
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = SelectedDocument.Name,
                Filter = "Text (*.txt)|*.txt|Markdown (*.md)|*.md|JSON (*.json)|*.json|All files (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                var model = new Document { Id = SelectedDocument.Id, Name = SelectedDocument.Name, Content = SelectedDocument.Content };
                await _docService.ExportAsync(model, dlg.FileName);
            }
        }

        private async Task ImportAsync()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text/Markdown/JSON|*.txt;*.md;*.json|All files|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                var doc = await _docService.ImportAsync(dlg.FileName);
                if (doc != null)
                {
                    var vm = new DocumentViewModel(doc);
                    Documents.Add(vm);
                    SelectedDocument = vm;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
