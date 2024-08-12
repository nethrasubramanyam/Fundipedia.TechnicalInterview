namespace UnitTests.ControllerTests
{
    [TestFixture]
    public class SupplierControllerTests
    {
        private Mock<ISupplierService> _mockSupplierService;
        private SuppliersController _supplierController;
        private static List<Supplier> _sampleSuppliers;

        [SetUp]
        public void SetUp()
        {
            _mockSupplierService = new Mock<ISupplierService>();
            _supplierController = new SuppliersController(_mockSupplierService.Object);

            InitialiseSampleData();
        }

        [Test]
        public async Task GetSuppliers_ReturnSuppliers()
        {
            //Arrange
            var countOfSuppliers = _sampleSuppliers.Count;

            _mockSupplierService.Setup(service => service.GetSuppliers()).ReturnsAsync(_sampleSuppliers);

            //Act
            var result = await _supplierController.GetSupplier();

            //Assert
            var serviceResponseResult = result?.Value as List<Supplier>;

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(serviceResponseResult);
                Assert.That(serviceResponseResult?.Count, Is.EqualTo(countOfSuppliers));
            });
        }

        [Test]
        [TestCase("6e6a8fb5-8847-403a-b3cb-586d6a6c26ec")]
        [TestCase("dc23ca3f-0aeb-4fa9-8272-f309f72e662e")]
        public async Task GetSupplierById_ValidSupplierGuid_ReturnSupplier(string supplierGuidId)
        {
            //Arrange
            var supplierId = Guid.Parse(supplierGuidId);

            _mockSupplierService.Setup(service => service.GetSupplier(supplierId)).ReturnsAsync(_sampleSuppliers.FirstOrDefault(s => s.Id == supplierId));

            var supplier = _sampleSuppliers.FirstOrDefault(s => s.Id == supplierId);

            //Act
            var result = await _supplierController.GetSupplier(supplierId);

            //Assert
            var serviceResponseResult = result?.Value;

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(serviceResponseResult);
                Assert.That(serviceResponseResult?.Id, Is.EqualTo(supplier?.Id));
            });
        }

        [Test]
        public async Task PostSupplier_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _supplierController.ModelState.AddModelError("Name", "Required"); // Simulate an invalid model state
            var supplier = new Supplier();

            // Act
            var result = await _supplierController.PostSupplier(supplier);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }

        [Test]
        public async Task PostSupplier_ValidModelState_ReturnsCreatedAtAction()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.Parse("6e6a8fb5-8847-403a-b3cb-586d6a6c26ee"),
                Title = "Mr",
                FirstName = "Nethra",
                LastName = "Nethra",
                ActivationDate = DateTime.Today.AddDays(10),
                Emails = new List<Email>(),
                Phones = new List<Phone>()
            };
            _mockSupplierService.Setup(s => s.InsertSupplier(supplier)).ReturnsAsync(supplier);

            // Act
            var result = await _supplierController.PostSupplier(supplier);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetSupplier", createdAtActionResult.ActionName);
            Assert.AreEqual(supplier.Id, ((Supplier)createdAtActionResult.Value).Id);
        }

        [Test]
        public async Task DeleteSupplier_SupplierExists_ReturnsSupplier()
        {
            // Arrange
            var supplierId = Guid.Parse("6e6a8fb5-8847-403a-b3cb-586d6a6c26ee");
            var supplier = new Supplier
            {
                Id = supplierId,
                Title = "Mr",
                FirstName = "Nethra",
                LastName = "Nethra",
                ActivationDate = DateTime.Today.AddDays(10),
                Emails = new List<Email>(),
                Phones = new List<Phone>()
            };
            _mockSupplierService.Setup(s => s.DeleteSupplier(supplierId)).ReturnsAsync(supplier);

            // Act
            var result = await _supplierController.DeleteSupplier(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<Supplier>>(result);
            Assert.AreEqual(supplier, result.Value);
        }

        [Test]
        public async Task DeleteSupplier_SupplierDoesNotExist_ReturnsNull()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            _mockSupplierService.Setup(s => s.DeleteSupplier(supplierId)).ReturnsAsync((Supplier)null);

            // Act
            var result = await _supplierController.DeleteSupplier(supplierId);

            // Assert
            Assert.IsNull(result.Value);
        }

        [Test]
        public async Task DeleteSupplier_SupplierThrowsException_ReturnsProblemResult()
        {
            // Arrange
            var supplierId = Guid.NewGuid();
            _mockSupplierService.Setup(s => s.DeleteSupplier(supplierId)).ThrowsAsync(new Exception("Supplier cannot be deleted"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _supplierController.DeleteSupplier(supplierId));
            Assert.That(ex.Message, Is.EqualTo("Supplier cannot be deleted"));
        }


        private static void InitialiseSampleData()
        {
            var emails = new List<Email>
            {
                new Email
                {
                    Id = Guid.NewGuid(),
                    EmailAddress = "test1@test.com",
                    IsPreferred = true
                },
                new Email
                {
                    Id = Guid.NewGuid(),
                    EmailAddress = "test2@test.com",
                    IsPreferred = false
                }
            };

            var phones = new List<Phone>
            {
                new Phone
                {
                    Id = Guid.NewGuid(),
                    PhoneNumber = "12341234",
                    IsPreferred = true
                },
                new Phone
                {
                    Id = Guid.NewGuid(),
                    PhoneNumber = "09870987",
                    IsPreferred = false
                }
            };

            _sampleSuppliers = new List<Supplier>
            {
                new Supplier
                {
                    Id = new Guid("6e6a8fb5-8847-403a-b3cb-586d6a6c26ec"),
                    FirstName = "Nethra",
                    LastName ="LastName",
                    Emails = new List<Email>{emails.First() },
                    Phones =  new List<Phone>{phones.First() }
                },
                new Supplier
                {
                    Id = new Guid("dc23ca3f-0aeb-4fa9-8272-f309f72e662e"),
                    FirstName = "Sub",
                    LastName ="LastName",
                    Emails = new List<Email>{emails.Skip(1).First() },
                    Phones =  new List<Phone>{phones.Skip(1).First() }
                }
            };
        }
    }
}

