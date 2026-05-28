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
        [Fact]
        public async Task GetAllAsync_WhenCalled_ReturnsUsersFromRepository()
        {
            //Arrange
            var korisnici = new List<ApplicationUser>
    {
        new ApplicationUser { Id = "1" },
        new ApplicationUser { Id = "2" }
    };

            var fakeRepo = A.Fake<IUserRepository>();
            A.CallTo(() => fakeRepo.GetAllAsync()).Returns(korisnici);

            var service = new UserService(fakeRepo);

            //Act
            var rezultat = await service.GetAllAsync();

            //Assert
            Assert.Equal(2, rezultat.ToList().Count);
        }

        [Fact]
        public async Task AddAsync_WhenCalled_CallsRepositoryAdd()
        {
            //Arrange
            var korisnik = new ApplicationUser { Id = "1" };
            var fakeRepo = A.Fake<IUserRepository>();
            var service = new UserService(fakeRepo);

            //Act
            await service.AddAsync(korisnik);

            //Assert
            A.CallTo(() => fakeRepo.AddAsync(korisnik)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateAsync_WhenCalled_CallsRepositoryUpdate()
        {
            //Arrange
            var korisnik = new ApplicationUser { Id = "1" };
            var fakeRepo = A.Fake<IUserRepository>();
            var service = new UserService(fakeRepo);

            //Act
            await service.UpdateAsync(korisnik);

            //Assert
            A.CallTo(() => fakeRepo.UpdateAsync(korisnik)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_CallsRepositoryDelete()
        {
            //Arrange
            var fakeRepo = A.Fake<IUserRepository>();
            var service = new UserService(fakeRepo);

            //Act
            await service.DeleteAsync("1");

            //Assert
            A.CallTo(() => fakeRepo.DeleteAsync("1")).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task DeactivateAsync_GivenExistingUser_UserIsDeactivated()
        {
            //Arrange
            var korisnik = new ApplicationUser { Id = "1", IsActive = true };

            var fakeRepo = A.Fake<IUserRepository>();
            A.CallTo(() => fakeRepo.GetByIdAsync("1")).Returns(korisnik);

            var service = new UserService(fakeRepo);

            //Act
            await service.DeactivateAsync("1");

            //Assert
            Assert.False(korisnik.IsActive);
            A.CallTo(() => fakeRepo.UpdateAsync(korisnik)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeactivateAsync_GivenNonExistingUser_UpdateIsNotCalled()
        {
            //Arrange
            var fakeRepo = A.Fake<IUserRepository>();
            A.CallTo(() => fakeRepo.GetByIdAsync("nepostojeci"))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var service = new UserService(fakeRepo);

            //Act
            await service.DeactivateAsync("nepostojeci");

            //Assert
            A.CallTo(() => fakeRepo.UpdateAsync(A<ApplicationUser>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ActivateAsync_GivenExistingUser_UserIsActivated()
        {
            //Arrange
            var korisnik = new ApplicationUser { Id = "2", IsActive = false };

            var fakeRepo = A.Fake<IUserRepository>();
            A.CallTo(() => fakeRepo.GetByIdAsync("2")).Returns(korisnik);

            var service = new UserService(fakeRepo);

            //Act
            await service.ActivateAsync("2");

            //Assert
            Assert.True(korisnik.IsActive);
            A.CallTo(() => fakeRepo.UpdateAsync(korisnik)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ActivateAsync_GivenNonExistingUser_UpdateIsNotCalled()
        {
            //Arrange
            var fakeRepo = A.Fake<IUserRepository>();
            A.CallTo(() => fakeRepo.GetByIdAsync("nepostojeci"))
                .Returns(Task.FromResult<ApplicationUser>(null));

            var service = new UserService(fakeRepo);

            //Act
            await service.ActivateAsync("nepostojeci");

            //Assert
            A.CallTo(() => fakeRepo.UpdateAsync(A<ApplicationUser>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task GetByStatusAsync_GivenActiveStatus_ReturnsUsersFromRepository()
        {
            //Arrange
            var aktivni = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", IsActive = true },
                new ApplicationUser { Id = "2", IsActive = true }
            };

            var fakeRepo = A.Fake<IUserRepository>();
            A.CallTo(() => fakeRepo.GetByStatusAsync(true)).Returns(aktivni);

            var service = new UserService(fakeRepo);

            //Act
            var rezultat = await service.GetByStatusAsync(true);

            //Assert
            Assert.Equal(2, rezultat.ToList().Count);
            Assert.All(rezultat, u => Assert.True(u.IsActive));
        }

        [Fact]
        public async Task GetByStatusAsync_GivenInactiveStatus_ReturnsInactiveUsersFromRepository()
        {
            //Arrange
            var neaktivni = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "3", IsActive = false },
            new ApplicationUser { Id = "4", IsActive = false }
        };

        var fakeRepo = A.Fake<IUserRepository>();
         A.CallTo(() => fakeRepo.GetByStatusAsync(false)).Returns(neaktivni);

        var service = new UserService(fakeRepo);

         //Act
        var rezultat = await service.GetByStatusAsync(false);

         //Assert
         Assert.Equal(2, rezultat.ToList().Count);
         Assert.All(rezultat, u => Assert.False(u.IsActive));
        }


    }
}