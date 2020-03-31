namespace Dictionary
{

    public class JSONClass
    {
        public Table[] Property1 { get; set; }
    }

    public class Table
    {
        public string type { get; set; }
        public string version { get; set; }
        public string comment { get; set; }
        public string name { get; set; }
        public string database { get; set; }
        public WordData[] data { get; set; }
    }

    public class WordData
    {
        public string id { get; set; }
        public string word { get; set; }
        public object synonym { get; set; }
        public string definition { get; set; }
    }

}