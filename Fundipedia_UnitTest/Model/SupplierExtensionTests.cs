namespace UnitTests.ModelTests
{
    public class SupplierExtensionsTests
    {
        [Test]
        public void IsActive_ValidActivationDate_ReturnsTrue()
        {
            //Arrange
            var supplier = new Supplier { ActivationDate = DateTime.UtcNow };

            //Act
            var isActive = supplier.IsActive();

            //Assert
            Assert.That(isActive, Is.True);
        }

        [Test]
        [TestCase("nethra123@testdomain.com")]
        [TestCase("test.test@gmail.com")]
        public void IsValidEmail_ValidEmail_ReturnsTrue(string email)
        {
            //Arrange
            var supplier = new Supplier
            {
                Emails =
                {
                    new Email { EmailAddress = email }
                }
            };

            //Act
            var isValidEmail = supplier.IsValidEmail(out _);

            //Assert
            Assert.That(isValidEmail, Is.True);
        }

        [Test]
        [TestCase("nethra123testdomain.com")]
        [TestCase("test.test@gmailcom")]
        public void IsValidEmail_InvalidEmail_ReturnsFalse(string email)
        {
            //Arrange
            var supplier = new Supplier
            {
                Emails =
                {
                    new Email { EmailAddress = email }
                }
            };

            string invalidEmail;
            //Act
            var isValidEmail = supplier.IsValidEmail(out invalidEmail);

            //Assert
            Assert.That(isValidEmail, Is.False);
            Assert.AreEqual(email, invalidEmail);
        }

        [Test]
        [TestCase("912341234")]
        [TestCase("9123456789")]
        public void IsValidPhoneNumber_ValidPhoneNumber_ReturnsTrue(string phoneNumber)
        {
            //Arrange
            var supplier = new Supplier
            {
                Phones =
                {
                    new Phone { PhoneNumber = phoneNumber }
                }
            };

            //Act
            var isValidPhoneNumber = supplier.IsValidPhoneNumber(out _);

            // Assert
            Assert.That(isValidPhoneNumber, Is.True);
        }

        [Test]
        [TestCase("")]
        [TestCase("09123456789")]
        public void IsValidPhoneNumber_InvalidPhoneNumber_ReturnsFalse(string phoneNumber)
        {
            //Arrange
            var supplier = new Supplier
            {
                Phones =
                {
                    new Phone { PhoneNumber = phoneNumber }
                }
            };

            string invalidPhone;
            //Act
            var isValidPhoneNumber = supplier.IsValidPhoneNumber(out invalidPhone);

            // Assert
            Assert.That(isValidPhoneNumber, Is.False);
            Assert.AreEqual(invalidPhone, phoneNumber);
        }
    }
}
