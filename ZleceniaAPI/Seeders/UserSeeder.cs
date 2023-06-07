using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Seeders
{
    public class UserSeeder
    {
        private OferiaDbContext _dbContext;

        public UserSeeder(OferiaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.StatusOfUsers.Any())
                {
                    var statuses = GetStatuses();
                    _dbContext.StatusOfUsers.AddRange(statuses);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.TypesOfAccounts.Any())
                {
                    var types = GetTypes();
                    _dbContext.TypesOfAccounts.AddRange(types);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<StatusOfUser> GetStatuses()
        {
            var statuses = new List<StatusOfUser>()
            {
                new StatusOfUser("Firma"),
                new StatusOfUser("Osoba prywatna")
            };

            return statuses;
        }

        private IEnumerable<TypeOfAccount> GetTypes()
        {
            var types = new List<TypeOfAccount>()
            {
                new TypeOfAccount("Zleceniodawca"),
                new TypeOfAccount("Wykonawca")
            };

            return types;
        }
    }
}
