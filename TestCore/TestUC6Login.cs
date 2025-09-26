using NUnit.Framework;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Services;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Interfaces.Repositories;

namespace TestCore
{
    [TestFixture]
    public class TestUC6Login
    {
        private IAuthService _authService;
        private IClientService _clientService;
        private IClientRepository _clientRepository;

        [SetUp]
        public void Setup()
        {
            // Arrange - Setup voor alle tests
            _clientRepository = new ClientRepository();
            _clientService = new ClientService(_clientRepository);
            _authService = new AuthService(_clientService);
        }

        /// <summary>
        /// UC6 - Test succesvol inloggen met geldige gegevens (A3 methode)
        /// </summary>
        [Test]
        public void TestUC6_SuccesvolInloggen()
        {
            // Arrange - Geldige inloggegevens van bestaande gebruiker
            string validEmail = "user3@mail.com";
            string validPassword = "user3";

            // Act - Probeer in te loggen (UC6 stap: "App controleert de gegevens")
            var result = _authService.Login(validEmail, validPassword);

            // Assert - Inloggen zou moeten slagen
            Assert.IsNotNull(result, "Inloggen met geldige gegevens zou moeten slagen");
            Assert.AreEqual(validEmail, result.EmailAddress, "Email van ingelogde gebruiker klopt niet");
            Assert.AreEqual("A.J. Kwak", result.Name, "Naam van ingelogde gebruiker klopt niet");
        }

        /// <summary>
        /// UC6 - Test inloggen met ongeldige gegevens (A3 methode)
        /// </summary>
        [Test]
        public void TestUC6_OngeldigeGegevens()
        {
            // Arrange - Ongeldige inloggegevens
            string validEmail = "user3@mail.com";
            string invalidPassword = "verkeerd_wachtwoord";

            // Act - Probeer in te loggen met verkeerd wachtwoord
            var result = _authService.Login(validEmail, invalidPassword);

            // Assert - Inloggen zou moeten falen (UC6: "Ongeldige inloggegevens")
            Assert.IsNull(result, "Inloggen met ongeldige gegevens zou moeten falen");
        }

        /// <summary>
        /// UC6 - Test inloggen met onbestaand account (A3 methode)
        /// </summary>
        [Test]
        public void TestUC6_OnbestaandAccount()
        {
            // Arrange - Email die niet bestaat in het systeem
            string nonExistentEmail = "onbekend@mail.com";
            string anyPassword = "password123";

            // Act - Probeer in te loggen met onbestaand account
            var result = _authService.Login(nonExistentEmail, anyPassword);

            // Assert - Inloggen zou moeten falen
            Assert.IsNull(result, "Inloggen met onbestaand account zou moeten falen");
        }

        /// <summary>
        /// UC6 - Test alle bestaande gebruikers kunnen inloggen (A3 methode)
        /// </summary>
        [TestCase("user1@mail.com", "user1", "M.J. Curie")]
        [TestCase("user2@mail.com", "user2", "H.H. Hermans")]
        [TestCase("user3@mail.com", "user3", "A.J. Kwak")]
        public void TestUC6_AlleBestaandeGebruikers(string email, string password, string expectedName)
        {
            // Arrange - Testgegevens via TestCase parameters

            // Act - Probeer in te loggen
            var result = _authService.Login(email, password);

            // Assert - Inloggen zou moeten slagen met correcte gebruiker
            Assert.IsNotNull(result, $"Gebruiker {email} zou moeten kunnen inloggen");
            Assert.AreEqual(expectedName, result.Name, $"Naam zou {expectedName} moeten zijn");
            Assert.AreEqual(email, result.EmailAddress, $"Email zou {email} moeten zijn");
        }
    }
}