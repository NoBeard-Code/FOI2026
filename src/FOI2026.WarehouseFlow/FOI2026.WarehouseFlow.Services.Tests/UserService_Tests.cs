using FakeItEasy;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using FOI2026.WarehouseFlow.Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FOI2026.WarehouseFlow.Services.Tests
{
    public class UserService_Tests
    {
        private (UserService service, IUserRepository fakeRepo) CreateService()
        {
            var fakeRepo = A.Fake<IUserRepository>();
            var service = new UserService(fakeRepo);
            return (service, fakeRepo);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsUsersFromRepository()
        {
            //Arrange
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1" },
                new ApplicationUser { Id = "2" }
            };

            var (service, fakeRepo) = CreateService();
            A.CallTo(() => fakeRepo.GetAllAsync()).Returns(users);

            //Act
            var result = await service.GetAllAsync();

            //Assert
            Assert.Equal(2, result.ToList().Count);
        }

        [Fact]
        public async Task AddAsync_WhenCalled_CallsRepositoryAdd()
        {
            //Arrange
            var user = new ApplicationUser { Id = "1" };
            var (service, fakeRepo) = CreateService();

            //Act
            await service.AddAsync(user);

            //Assert
            A.CallTo(() => fakeRepo.AddAsync(user)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateAsync_WhenCalled_CallsRepositoryUpdate()
        {
            //Arrange
            var user = new ApplicationUser { Id = "1" };
            var (service, fakeRepo) = CreateService();

            //Act
            await service.UpdateAsync(user);

            //Assert
            A.CallTo(() => fakeRepo.UpdateAsync(user)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_CallsRepositoryDelete()
        {
            //Arrange
            var (service, fakeRepo) = CreateService();

            //Act
            await service.DeleteAsync("1");

            //Assert
            A.CallTo(() => fakeRepo.DeleteAsync("1")).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeactivateAsync_GivenExistingUser_UserIsDeactivated()
        {
            //Arrange
            var user = new ApplicationUser { Id = "1", IsActive = true };
            var (service, fakeRepo) = CreateService();
            A.CallTo(() => fakeRepo.GetByIdAsync("1")).Returns(user);

            //Act
            await service.DeactivateAsync("1");

            //Assert
            Assert.False(user.IsActive);
            A.CallTo(() => fakeRepo.UpdateAsync(user)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeactivateAsync_GivenNonExistingUser_UpdateIsNotCalled()
        {
            //Arrange
            var (service, fakeRepo) = CreateService();
            A.CallTo(() => fakeRepo.GetByIdAsync("nonexistent"))
                .Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            await service.DeactivateAsync("nonexistent");

            //Assert
            A.CallTo(() => fakeRepo.UpdateAsync(A<ApplicationUser>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ActivateAsync_GivenExistingUser_UserIsActivated()
        {
            //Arrange
            var user = new ApplicationUser { Id = "2", IsActive = false };
            var (service, fakeRepo) = CreateService();
            A.CallTo(() => fakeRepo.GetByIdAsync("2")).Returns(user);

            //Act
            await service.ActivateAsync("2");

            //Assert
            Assert.True(user.IsActive);
            A.CallTo(() => fakeRepo.UpdateAsync(user)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ActivateAsync_GivenNonExistingUser_UpdateIsNotCalled()
        {
            //Arrange
            var (service, fakeRepo) = CreateService();
            A.CallTo(() => fakeRepo.GetByIdAsync("nonexistent"))
                .Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            await service.ActivateAsync("nonexistent");

            //Assert
            A.CallTo(() => fakeRepo.UpdateAsync(A<ApplicationUser>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task GetByStatusAsync_GivenActiveStatus_ReturnsActiveUsersFromRepository()
        {
            //Arrange
            var activeUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", IsActive = true },
                new ApplicationUser { Id = "2", IsActive = true }
            };

            var (service, fakeRepo) = CreateService();
            A.CallTo(() => fakeRepo.GetByStatusAsync(true)).Returns(activeUsers);

            //Act
            var result = await service.GetByStatusAsync(true);

            //Assert
            Assert.Equal(2, result.ToList().Count);
            Assert.All(result, u => Assert.True(u.IsActive));
        }

        [Fact]
        public async Task GetByStatusAsync_GivenInactiveStatus_ReturnsInactiveUsersFromRepository()
        {
            //Arrange
            var inactiveUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "3", IsActive = false },
                new ApplicationUser { Id = "4", IsActive = false }
            };

            var (service, fakeRepo) = CreateService();
            A.CallTo(() => fakeRepo.GetByStatusAsync(false)).Returns(inactiveUsers);

            //Act
            var result = await service.GetByStatusAsync(false);

            //Assert
            Assert.Equal(2, result.ToList().Count);
            Assert.All(result, u => Assert.False(u.IsActive));
        }
    }
}