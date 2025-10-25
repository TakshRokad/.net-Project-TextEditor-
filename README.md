# Text Editor Application

A modern, feature-rich text editor built with .NET 8 WPF, AvalonEdit, and Material Design.

## Features

### Core Functionality
- **Multiple Documents**: Create, switch between, rename, and delete documents
- **Rich Text Editing**: Powered by AvalonEdit with line numbers and monospace font (Consolas)
- **File Operations**: Import/Export documents in `.txt`, `.md`, and `.json` formats
- **Auto-save**: Automatic saving every 30 seconds
- **Undo/Redo**: Full undo/redo support (Ctrl+Z, Ctrl+Y)
- **Search**: Find text functionality (Ctrl+F) with case-insensitive search
- **Statistics**: Real-time character and word count display

### UI Features
- **Dark/Light Theme**: Toggle between dark and light themes
- **Material Design**: Modern UI with Material Design components
- **Responsive Layout**: Clean three-section layout (top bar, editor, bottom bar)
- **Keyboard Shortcuts**:
  - `Ctrl+Z` - Undo
  - `Ctrl+Y` - Redo
  - `Ctrl+F` - Find
  - `Enter` (in search box) - Find next

### UI Layout

#### Top Bar (Navy Blue #1a1f3a)
- Document dropdown selector
- Action buttons: New, Rename, Delete, Export, Import

#### Main Editor Area (Black #000000)
- Large text editing area with AvalonEdit
- Line numbers on the left
- White text on black background
- Placeholder text: "Start typing..."

#### Bottom Bar (Dark #121528)
- Left: Undo/Redo buttons
- Center: Search box with Find button
- Right: Character count, word count, dark mode toggle

## Technical Stack

- **Framework**: .NET 8 WPF
- **Language**: C# 12
- **Architecture**: MVVM Pattern
- **UI Components**:
  - AvalonEdit 6.3.1 - Text editor component
  - MaterialDesignThemes 5.3.0 - Material Design UI
  - MaterialDesignColors 5.3.0 - Color themes
- **Additional Libraries**:
  - Markdig 0.43.0 - Markdown processing (for future enhancements)
  - Microsoft.VisualBasic 10.3.0 - Input dialogs

## Project Structure

```
TextEditor/
├── Models/
│   └── Document.cs                 # Document data model
├── ViewModels/
│   ├── MainViewModel.cs            # Main window view model
│   └── DocumentViewModel.cs        # Document view model wrapper
├── Views/
│   └── MainWindow.xaml             # Main window UI
├── Services/
│   ├── FileService.cs              # File I/O operations
│   └── DocumentService.cs          # Document management & auto-save
├── Helpers/
│   ├── RelayCommand.cs             # ICommand implementation
│   └── AvalonEditTextBinding.cs    # Two-way binding for AvalonEdit
└── App.xaml                        # Application resources & theme
```

## Getting Started

### Prerequisites
- .NET 8 SDK or later
- Windows OS (WPF application)

### Building the Project
```bash
dotnet build TextEditor.sln
```

### Running the Application
```bash
dotnet run --project TextEditor/TextEditor.csproj
```

Or simply open `TextEditor.sln` in Visual Studio and press F5.

## Usage

### Creating Documents
1. Click the "+ New" button in the top bar
2. A new untitled document will be created and selected

### Switching Documents
- Use the dropdown selector on the left of the top bar to switch between documents

### Renaming Documents
1. Select a document
2. Click "Rename" button
3. Enter the new name in the dialog

### Deleting Documents
1. Select a document
2. Click "Delete" button
3. Confirm the deletion

### Importing Files
1. Click "Import" button
2. Select a `.txt`, `.md`, or `.json` file
3. The document will be added to your collection

### Exporting Documents
1. Select a document
2. Click "Export" button
3. Choose format and location
4. Supported formats: `.txt`, `.md`, `.json`

### Searching Text
1. Enter search term in the bottom search box
2. Click "Find" or press Enter
3. The editor will highlight and scroll to the next match

### Theme Toggle
- Click the dark mode toggle in the bottom-right to switch between dark and light themes

## Data Storage

Documents are automatically saved to:
```
%APPDATA%\TextEditor\
```

Each document is stored as a JSON file with its unique ID as the filename.

## Future Enhancements

Potential features for future versions:
- Markdown preview using Markdig
- Export to PDF
- Syntax highlighting for code
- Font size adjustment
- Recent documents list
- Find and replace
- Line wrapping toggle
- Custom color schemes

## License

This project is provided as-is for educational and personal use.

## Author

Created with Windsurf Cascade AI
