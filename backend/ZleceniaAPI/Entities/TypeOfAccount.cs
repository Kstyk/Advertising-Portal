namespace ZleceniaAPI.Entities
{
    public class TypeOfAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TypeOfAccount(string name)
        {
            Name = name;
        }
    }
}
