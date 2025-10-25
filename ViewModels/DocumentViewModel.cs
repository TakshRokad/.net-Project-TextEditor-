using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TextEditor.Models;

namespace TextEditor.ViewModels
{
    public class DocumentViewModel : INotifyPropertyChanged
    {
        private readonly Document _model;

        public DocumentViewModel(Document model)
        {
            _model = model;
        }

        public Guid Id => _model.Id;

        public string Name
        {
            get => _model.Name;
            set { if (_model.Name != value) { _model.Name = value; OnPropertyChanged(); } }
        }

        public string Content
        {
            get => _model.Content;
            set { if (_model.Content != value) { _model.Content = value; OnPropertyChanged(); OnPropertyChanged(nameof(CharCount)); OnPropertyChanged(nameof(WordCount)); } }
        }

        public int CharCount => string.IsNullOrEmpty(Content) ? 0 : Content.Length;

        public int WordCount
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Content)) return 0;
                var parts = Content.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Length;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
