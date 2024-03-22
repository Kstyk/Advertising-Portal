namespace ZleceniaAPI.Entities
{
    public class StatusOfUser
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public StatusOfUser(string name)
        {
            Name = name;
        }
    }
}
