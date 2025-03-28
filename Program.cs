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
      #pragma warning disable SYSLIB0011
      BinaryFormatter formatter = new BinaryFormatter();
      #pragma warning disable SYSLIB0011
      formatter.Serialize(fs, this);
    }
  }

  public static TextFile LoadBinary(string path) {
    using (FileStream fs = new FileStream(path, FileMode.Open)) {
      #pragma warning disable SYSLIB0011
      BinaryFormatter formatter = new BinaryFormatter();
      #pragma warning disable SYSLIB0011
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
    return files.Where(pathFile => File.ReadAllText(pathFile).Contains(keyword)).ToList();
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
    Console.WriteLine("=== Тестирование функционала TextFile ===");

    // 1️⃣ Тест: Создание объекта
    TextFile file = new TextFile("test.txt", "Тестовое содержимое");
    if (file.FileName == "test.txt" && file.Content == "Тестовое содержимое")
      Console.WriteLine("[✔] Тест создания объекта прошёл");
    else
      Console.WriteLine("[❌] Ошибка в создании объекта");

    // 2️⃣ Тест: Сохранение в XML
    string filePath = "test_file.xml";
    file.SaveXml(filePath);
    if (File.Exists(filePath))
      Console.WriteLine("[✔] Тест сохранения в XML прошёл");
    else
      Console.WriteLine("[❌] Ошибка при сохранении в XML");

    // 3️⃣ Тест: Загрузка из XML
    TextFile loadedFile = TextFile.LoadXml(filePath);
    if (loadedFile.FileName == "test.txt" && loadedFile.Content == "Тестовое содержимое")
      Console.WriteLine("[✔] Тест загрузки из XML прошёл");
    else
      Console.WriteLine("[❌] Ошибка при загрузке из XML");

    // 4️⃣ Тест: Проверка работы с файлом
    File.Delete(filePath);
    if (!File.Exists(filePath))
      Console.WriteLine("[✔] Тест удаления файла прошёл");
    else
      Console.WriteLine("[❌] Ошибка при удалении файла");

    Console.WriteLine("=== Все тесты завершены ===");
  }
}
