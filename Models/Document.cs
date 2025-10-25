using System;

namespace TextEditor.Models
{
    public class Document
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "default";
        public string Content { get; set; } = string.Empty;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}
