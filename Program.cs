using System;

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


class Program {
    static void Main() {

    }
}