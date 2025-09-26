using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client? Get(string email)
        {
            return _clientRepository.Get(email);
        }

        public Client? Get(int id)
        {
            return _clientRepository.Get(id);
        }

        public List<Client> GetAll()
        {
            return _clientRepository.GetAll();
        }

        public Client? Register(string name, string email, string password)
        {
            // Check if email already exists
            if (_clientRepository.Get(email) != null)
                return null;

            // Validate input parameters
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return null;

            // Hash the password using the existing PasswordHelper
            string hashedPassword = PasswordHelper.HashPassword(password);

            // Generate new ID (get highest existing ID + 1)
            var allClients = _clientRepository.GetAll();
            int newId = allClients.Any() ? allClients.Max(c => c.Id) + 1 : 1;

            // Create new client
            var newClient = new Client(newId, name, email, hashedPassword);

            // Add to repository
            return _clientRepository.Add(newClient);
        }
    }
}