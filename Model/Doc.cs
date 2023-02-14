namespace Model
{
    public class Doc
    {
        public int id { get; set; } = 0;
        public string Phone { get; set; } = "";
        public string IIN_BIN { get; set; } = "";
        public string FIO { get; set; } = "";
        public string Url { get; set; } = "";
        public byte[]? Data { get; set; }
        public int XqrCode { get; set; } = 10;
        public int YqrCode { get; set; } = 10;
        public string FileName { get; set; } = "";
    }
}