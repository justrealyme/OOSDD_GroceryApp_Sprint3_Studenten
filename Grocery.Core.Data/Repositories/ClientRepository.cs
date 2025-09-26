using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly List<Client> clientList;

        public ClientRepository()
        {
            clientList = [
                new Client(1, "M.J. Curie", "user1@mail.com", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08="),
                new Client(2, "H.H. Hermans", "user2@mail.com", "dOk+X+wt+MA9uIniRGKDFg==.QLvy72hdG8nWj1FyL75KoKeu4DUgu5B/HAHqTD2UFLU="),
                new Client(3, "A.J. Kwak", "user3@mail.com", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")
            ];
        }

        public Client? Get(string email)
        {
            return clientList.FirstOrDefault(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public Client? Get(int id)
        {
            return clientList.FirstOrDefault(c => c.Id == id);
        }

        public List<Client> GetAll()
        {
            return clientList;
        }

        public Client Add(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            // Check if client with same email already exists
            if (clientList.Any(c => c.EmailAddress.Equals(client.EmailAddress, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Een client met dit e-mailadres bestaat al.");

            // Check if ID already exists
            if (clientList.Any(c => c.Id == client.Id))
                throw new InvalidOperationException("Een client met dit ID bestaat al.");

            clientList.Add(client);
            return client;
        }
    }
}