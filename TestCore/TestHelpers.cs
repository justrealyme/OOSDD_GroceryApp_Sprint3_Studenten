using Grocery.Core.Helpers;

namespace TestCore
{
    public class TestHelpers
    {
        [SetUp]
        public void Setup()
        {
        }

        //Happy flow
        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue(string password, string passwordHash)
        {
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        //Unhappy flow
        [Test]
        public void TestPasswordHelperReturnsFalse()
        {
            // Arrange - Test met verkeerd wachtwoord
            string correctPassword = "user3";
            string wrongPassword = "wrongpassword";
            string correctHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";

            // Act & Assert - Verkeerd wachtwoord zou false moeten geven
            Assert.IsFalse(PasswordHelper.VerifyPassword(wrongPassword, correctHash),
                "Verkeerd wachtwoord zou false moeten retourneren");

            // Controleer ook dat correct wachtwoord nog steeds werkt
            Assert.IsTrue(PasswordHelper.VerifyPassword(correctPassword, correctHash),
                "Correct wachtwoord zou true moeten retourneren");
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA")]
        public void TestPasswordHelperReturnsFalse(string password, string passwordHash)
        {
            // Arrange, Act & Assert - Deze hashes missen het laatste teken, dus zijn ongeldig
            // De test zou false moeten retourneren voor ongeldige hashes
            Assert.IsFalse(PasswordHelper.VerifyPassword(password, passwordHash),
                $"Ongeldige hash zou false moeten retourneren voor password: {password}");
        }

        /// <summary>
        /// Test met null en lege waarden
        /// </summary>
        [Test]
        public void TestPasswordHelper_NullAndEmptyValues()
        {
            // Arrange
            string validPassword = "user3";
            string validHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";

            // Act & Assert - Test null waarden (VerifyPassword zou false moeten retourneren)
            Assert.IsFalse(PasswordHelper.VerifyPassword(null, validHash),
                "Null password zou false moeten retourneren");
            Assert.IsFalse(PasswordHelper.VerifyPassword(validPassword, null),
                "Null hash zou false moeten retourneren");
            Assert.IsFalse(PasswordHelper.VerifyPassword(null, null),
                "Null password en hash zouden false moeten retourneren");

            // Test lege strings
            Assert.IsFalse(PasswordHelper.VerifyPassword("", validHash),
                "Leeg password zou false moeten retourneren");
            Assert.IsFalse(PasswordHelper.VerifyPassword(validPassword, ""),
                "Lege hash zou false moeten retourneren");
            Assert.IsFalse(PasswordHelper.VerifyPassword("", ""),
                "Lege password en hash zouden false moeten retourneren");

            // Test whitespace strings
            Assert.IsFalse(PasswordHelper.VerifyPassword("   ", validHash),
                "Whitespace-only password zou false moeten retourneren");
            Assert.IsFalse(PasswordHelper.VerifyPassword(validPassword, "   "),
                "Whitespace-only hash zou false moeten retourneren");
        }

        /// <summary>
        /// Test HashPassword functionaliteit
        /// </summary>
        [Test]
        public void TestPasswordHelper_HashPassword()
        {
            // Arrange
            string password = "testpassword123";

            // Act
            string hash1 = PasswordHelper.HashPassword(password);
            string hash2 = PasswordHelper.HashPassword(password);

            // Assert
            Assert.IsNotNull(hash1, "Hash zou niet null moeten zijn");
            Assert.IsNotEmpty(hash1, "Hash zou niet leeg moeten zijn");
            Assert.AreNotEqual(hash1, hash2, "Twee hashes van hetzelfde wachtwoord zouden verschillend moeten zijn (door verschillende salt)");
            Assert.IsTrue(hash1.Contains("."), "Hash zou salt en hash gescheiden door punt moeten bevatten");

            // Controleer dat beide hashes het originele wachtwoord kunnen verifiëren
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, hash1),
                "Eerste hash zou originele wachtwoord moeten verifiëren");
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, hash2),
                "Tweede hash zou originele wachtwoord moeten verifiëren");
        }

        /// <summary>
        /// Test HashPassword met null/empty input (zou exception moeten gooien)
        /// </summary>
        [Test]
        public void TestPasswordHelper_HashPassword_NullInput_ThrowsException()
        {
            // Act & Assert - HashPassword zou een ArgumentException moeten gooien bij null input
            Assert.Throws<ArgumentException>(() => PasswordHelper.HashPassword(null),
                "HashPassword zou ArgumentException moeten gooien bij null password");

            Assert.Throws<ArgumentException>(() => PasswordHelper.HashPassword(""),
                "HashPassword zou ArgumentException moeten gooien bij lege string");

            Assert.Throws<ArgumentException>(() => PasswordHelper.HashPassword("   "),
                "HashPassword zou ArgumentException moeten gooien bij whitespace-only string");
        }
    }
}