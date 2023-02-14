namespace Model
{
    public class User
    {
        public int id { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.Now;      
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";        
        public string UserName { get; set; } = "";
    }
}
