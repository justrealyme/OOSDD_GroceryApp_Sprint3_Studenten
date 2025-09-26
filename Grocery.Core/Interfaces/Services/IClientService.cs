using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IClientService
    {
        /// <summary>
        /// Haalt een client op basis van e-mailadres
        /// </summary>
        /// <param name="email">Het e-mailadres van de client</param>
        /// <returns>De client of null als deze niet bestaat</returns>
        public Client? Get(string email);

        /// <summary>
        /// Haalt een client op basis van ID
        /// </summary>
        /// <param name="id">Het ID van de client</param>
        /// <returns>De client of null als deze niet bestaat</returns>
        public Client? Get(int id);

        /// <summary>
        /// Haalt alle clients op
        /// </summary>
        /// <returns>Lijst van alle clients</returns>
        public List<Client> GetAll();

        /// <summary>
        /// Registreert een nieuwe client
        /// </summary>
        /// <param name="name">Volledige naam van de client</param>
        /// <param name="email">E-mailadres van de client</param>
        /// <param name="password">Wachtwoord (wordt gehashed opgeslagen)</param>
        /// <returns>De nieuwe client of null als registratie mislukt</returns>
        public Client? Register(string name, string email, string password);
    }
}