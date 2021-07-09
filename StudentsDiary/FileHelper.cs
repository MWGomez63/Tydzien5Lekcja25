using System.IO;
using System.Xml.Serialization;

namespace StudentsDiary
{
    // zamieniamy wszystkie "List<Student>" na "T"
    // dodatkowo klauzula  where T : new()
    // tylko dla typów (klasy), która ma konstruktor bezparametrowy
    // zamiast "T" mozna użyć dowolnego słowa np "Generic", ale konwencja jest taka, ze
    // raczej dajemy T
    public class FileHelper<T> where T : new()
    {
        private string _filePath;

        // aby utworzyć konstruktor ctor TAB TAB
        public FileHelper(string filePath)
        {
            _filePath = filePath;
        }
 
        public void SerializeToFile(T students)
        {
            // ta metoda jest lepsza niż SerializeToFile1
            var serializer = new XmlSerializer(typeof(T));

            // w using na koniec jest automaycznie wywoływane Dispose
            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public T DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
            {
                // wtedy zwracamy nową pustą listę
                return new T();
            }

            var serializer = new XmlSerializer(typeof(T));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (T)serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }
        }
    }
}
