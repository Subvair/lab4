using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

[Serializable]
public class TextFile {
    public string FileName { get; set; }
    public string Content { get; set; }

    public TextFile() { }
    public TextFile(string fileName, string content) {
        FileName = fileName;
        Content = content;
    }

    public void SaveBinary(string path) {
        using (FileStream fs = new FileStream(path, FileMode.Create)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, this);
        }
    }

    public static TextFile LoadBinary(string path) {
        using (FileStream fs = new FileStream(path, FileMode.Open)) {
            BinaryFormatter formatter = new BinaryFormatter();
            return (TextFile)formatter.Deserialize(fs);
        }
    }

    public void SaveXml(string path) {
        using (FileStream fs = new FileStream(path, FileMode.Create)) {
            XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
            serializer.Serialize(fs, this);
        }
    }

    public static TextFile LoadXml(string path) {
        using (FileStream fs = new FileStream(path, FileMode.Open)) {
            XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
            return (TextFile)serializer.Deserialize(fs);
        }
    }
    
}

public class TextFileSearcher {
    public static List<string> Search(string directory, string keyword) {
        var files = Directory.GetFiles(directory, "*.txt");
        return files.Where(f => File.ReadAllText(f).Contains(keyword)).ToList();
    }
}

public class TextEditor {
    private string _filePath;
    private Stack<string> _history = new Stack<string>();

    public TextEditor(string filePath) {
        _filePath = filePath;
        if (File.Exists(filePath)) {
            _history.Push(File.ReadAllText(filePath));
        }
    }

    public void Edit(string newContent) {
        _history.Push(newContent);
        File.WriteAllText(_filePath, newContent);
    }

    public void Undo() {
        if (_history.Count > 1) {
            _history.Pop();
            File.WriteAllText(_filePath, _history.Peek());
        }
    }
}

public class TextFileIndexer {
    private Dictionary<string, List<string>> _index = new Dictionary<string, List<string>>();

    public void IndexDirectory(string directory, string[] keywords) {
        foreach (var keyword in keywords) {
            _index[keyword] = TextFileSearcher.Search(directory, keyword);
        }
    }

    public List<string> GetFilesForKeyword(string keyword) {
        return _index.ContainsKey(keyword) ? _index[keyword] : new List<string>();
    }
}

class Program {
    static void Main() {

    }
}