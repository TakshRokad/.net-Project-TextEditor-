using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TextEditor.Models;

namespace TextEditor.Services
{
    public class DocumentService : IDisposable
    {
        private readonly FileService _fileService;
        private readonly Timer _autoSaveTimer;
        private readonly object _lock = new();

        public ObservableCollection<Document> Documents { get; } = new();
        public List<string> RecentFiles { get; } = new();

        public DocumentService(FileService fileService)
        {
            _fileService = fileService;
            _autoSaveTimer = new Timer(async _ => await AutoSaveAsync(), null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
        }

        public Document CreateNew(string name = "untitled")
        {
            var n = EnsureUniqueName(name);
            var doc = new Document { Name = n, Content = string.Empty };
            Documents.Add(doc);
            return doc;
        }

        public void Remove(Document doc)
        {
            Documents.Remove(doc);
            var path = _fileService.GetDocumentPath(doc.Id);
            if (File.Exists(path))
            {
                try { File.Delete(path); } catch { /* ignore */ }
            }
        }

        public string EnsureUniqueName(string baseName)
        {
            var name = baseName;
            int i = 1;
            while (Documents.Any(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                name = $"{baseName} ({i++})";
            }
            return name;
        }

        public async Task SaveAsync(Document doc) => await _fileService.SaveDocumentAsync(doc);

        public async Task<Document?> ImportAsync(string path)
        {
            var doc = await _fileService.ImportAsync(path);
            if (doc != null)
            {
                doc.Name = EnsureUniqueName(doc.Name);
                Documents.Add(doc);
                RecentFiles.Add(path);
            }
            return doc;
        }

        public async Task ExportAsync(Document doc, string path) => await _fileService.ExportAsync(doc, path);

        private async Task AutoSaveAsync()
        {
            List<Document> snapshot;
            lock (_lock)
            {
                snapshot = Documents.ToList();
            }
            foreach (var d in snapshot)
            {
                try { await _fileService.SaveDocumentAsync(d); } catch { /* ignore autosave failures */ }
            }
        }

        public void Dispose()
        {
            _autoSaveTimer.Dispose();
        }
    }
}
