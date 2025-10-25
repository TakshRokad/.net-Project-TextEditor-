using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TextEditor.Models;

namespace TextEditor.Services
{
    public class FileService
    {
        private readonly string _basePath;

        public FileService(string basePath)
        {
            _basePath = basePath;
            Directory.CreateDirectory(_basePath);
        }

        public string GetDocumentPath(Guid id) => Path.Combine(_basePath, $"{id}.json");

        public async Task SaveDocumentAsync(Document doc)
        {
            doc.LastModified = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(GetDocumentPath(doc.Id), json, Encoding.UTF8);
        }

        public async Task<Document?> LoadDocumentAsync(Guid id)
        {
            var path = GetDocumentPath(id);
            if (!File.Exists(path)) return null;
            var json = await File.ReadAllTextAsync(path, Encoding.UTF8);
            return JsonSerializer.Deserialize<Document>(json);
        }

        public async Task<Document?> ImportAsync(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            var name = Path.GetFileNameWithoutExtension(filePath);
            string content;
            if (ext == ".json")
            {
                var json = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                var doc = JsonSerializer.Deserialize<Document>(json);
                if (doc != null)
                {
                    if (doc.Id == Guid.Empty) doc.Id = Guid.NewGuid();
                    return doc;
                }
                return null;
            }
            content = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
            return new Document { Name = name, Content = content };
        }

        public async Task ExportAsync(Document doc, string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            switch (ext)
            {
                case ".txt":
                case ".md":
                    await File.WriteAllTextAsync(filePath, doc.Content ?? string.Empty, Encoding.UTF8);
                    break;
                case ".json":
                    var json = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
                    break;
                default:
                    await File.WriteAllTextAsync(filePath, doc.Content ?? string.Empty, Encoding.UTF8);
                    break;
            }
        }
    }
}
