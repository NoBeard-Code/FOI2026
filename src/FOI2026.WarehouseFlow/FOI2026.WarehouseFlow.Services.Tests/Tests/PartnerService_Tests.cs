using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using FOI2026.WarehouseFlow.Services.Services;
using Xunit;

namespace FOI2026.WarehouseFlow.Services.Tests.Tests
{
    public class PartnerService_Tests
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly PartnerService _service;

        public PartnerService_Tests()
        {
            _partnerRepository = A.Fake<IPartnerRepository>();
            _service = new PartnerService(_partnerRepository);
        }

        // GET ALL

        [Fact]
        public async Task GetAllSuppliersAsync_CallsRepository_Once()
        {
            // ARRANGE
            A.CallTo(() => _partnerRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Partner>>(new List<Partner>()));

            // ACT
            await _service.GetAllSuppliersAsync();

            // ASSERT
            A.CallTo(() => _partnerRepository.GetAllAsync())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAllSuppliersAsync_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
        {
            // ARRANGE
            A.CallTo(() => _partnerRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Partner>>(new List<Partner>()));

            // ACT
            var result = await _service.GetAllSuppliersAsync();

            // ASSERT
            Assert.Empty(result);
        }

        // ADD

        [Fact]
        public async Task AddSupplierAsync_SetsIsSupplierTrue_AndCallsRepository()
        {
            // ARRANGE
            var partner = new Partner
            {
                Name = "Test Supplier",
                IsSupplier = false
            };

            // ACT
            await _service.AddSupplierAsync(partner);

            // ASSERT
            Assert.True(partner.IsSupplier);

            A.CallTo(() => _partnerRepository.AddAsync(partner))
                .MustHaveHappenedOnceExactly();
        }

        // UPDATE

        [Fact]
        public async Task UpdateSupplierAsync_SetsIsSupplierTrue_AndCallsRepository()
        {
            // ARRANGE
            var partner = new Partner
            {
                PartnerId = 1,
                Name = "Updated Supplier",
                IsSupplier = false
            };

            // ACT
            await _service.UpdateSupplierAsync(partner);

            // ASSERT
            Assert.True(partner.IsSupplier);

            A.CallTo(() => _partnerRepository.UpdateAsync(partner))
                .MustHaveHappenedOnceExactly();
        }

        // DELETE

        [Fact]
        public async Task DeleteSupplierAsync_CallsRepository_WithCorrectId()
        {
            // ARRANGE
            int id = 10;

            // ACT
            await _service.DeleteSupplierAsync(id);

            // ASSERT
            A.CallTo(() => _partnerRepository.DeleteAsync(id))
                .MustHaveHappenedOnceExactly();
        }

        // SEARCH

        [Fact]
        public async Task SearchSuppliersAsync_ReturnsAll_WhenSearchTermIsEmpty()
        {
            // ARRANGE
            var partners = new List<Partner>
            {
                new Partner { Name = "Alpha" },
                new Partner { Name = "Beta" }
            };

            A.CallTo(() => _partnerRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Partner>>(partners));

            // ACT
            var result = await _service.SearchSuppliersAsync("");

            // ASSERT
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task SearchSuppliersAsync_FiltersByName()
        {
            // ARRANGE
            var partners = new List<Partner>
            {
                new Partner { Name = "Steel Company" },
                new Partner { Name = "Wood Company" }
            };

            A.CallTo(() => _partnerRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Partner>>(partners));

            // ACT
            var result = await _service.SearchSuppliersAsync("steel");

            // ASSERT
            Assert.Single(result);
            Assert.Contains(result, p => p.Name == "Steel Company");
        }

        [Fact]
        public async Task SearchSuppliersAsync_NullSearch_ReturnsAll()
        {
            // ARRANGE
            var partners = new List<Partner>
            {
                new Partner { Name = "Test" }
            };

            A.CallTo(() => _partnerRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Partner>>(partners));

            // ACT
            var result = await _service.SearchSuppliersAsync(null);

            // ASSERT
            Assert.Single(result);
        }
    }
}
