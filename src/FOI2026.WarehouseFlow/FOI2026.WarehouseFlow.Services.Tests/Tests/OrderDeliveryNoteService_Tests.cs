using FakeItEasy;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using FOI2026.WarehouseFlow.Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FOI2026.WarehouseFlow.Services.Tests.Tests
{
    public class OrderDeliveryNoteService_Tests
    {

        private readonly IOrderRepository _orderRepository;
        private readonly OrderService _orderService;


        private readonly IDeliveryNoteRepository _deliveryNoteRepository;
        private readonly DeliveryNoteService _deliveryNoteService;

        public OrderDeliveryNoteService_Tests()
        {
            _orderRepository = A.Fake<IOrderRepository>();
            _orderService = new OrderService(_orderRepository);

            _deliveryNoteRepository = A.Fake<IDeliveryNoteRepository>();
            _deliveryNoteService = new DeliveryNoteService(_deliveryNoteRepository);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderId = 1, Code = "ORD1" },
                new Order { OrderId = 2, Code = "ORD2" }
            };
            A.CallTo(() => _orderRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Order>>(orders));

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsOrders_WithOrderItems()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    Code = "ORD1",
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { ArticleId = 1, Quantity = 5, Price = 100 },
                        new OrderItem { ArticleId = 2, Quantity = 3, Price = 50 }
                    }
                }
            };
            A.CallTo(() => _orderRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<Order>>(orders));

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result.First().OrderItems.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 1,
                Code = "ORD1",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ArticleId = 1, Quantity = 10, Price = 200 }
                }
            };
            A.CallTo(() => _orderRepository.GetByIdAsync(1))
                .Returns(Task.FromResult<Order?>(order));

            // Act
            var result = await _orderService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.OrderId);
            Assert.Single(result.OrderItems);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenIdIsInvalid()
        {
            // Arrange

            // Act
            var result = await _orderService.GetByIdAsync(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddOrderAsync_CallsRepository_WhenOrderIsValid()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 1,
                Code = "ORD1",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ArticleId = 1, Quantity = 5, Price = 100 }
                }
            };

            // Act
            await _orderService.AddOrderAsync(order);

            // Assert
            A.CallTo(() => _orderRepository.AddAsync(order))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddOrderAsync_DoesNotCallRepository_WhenOrderIsNull()
        {
            // Arrange
            Order? order = null;

            // Act
            await _orderService.AddOrderAsync(order!);

            // Assert
            A.CallTo(() => _orderRepository.AddAsync(A<Order>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task UpdateOrderAsync_CallsRepository_WhenOrderIsValid()
        {
            // Arrange
            var order = new Order { OrderId = 1, Code = "ORD1" };

            // Act
            await _orderService.UpdateOrderAsync(order);

            // Assert
            A.CallTo(() => _orderRepository.UpdateAsync(order))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteOrderAsync_CallsRepository_WhenIdIsValid()
        {
            // Arrange
            int id = 1;

            // Act
            await _orderService.DeleteOrderAsync(id);

            // Assert
            A.CallTo(() => _orderRepository.DeleteAsync(1))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetDeliveryNotesAsync_ReturnsAllDeliveryNotes()
        {
            // Arrange
            var deliveryNotes = new List<DeliveryNote>
            {
                new DeliveryNote { DeliveryNoteId = 1, Code = "DN1" },
                new DeliveryNote { DeliveryNoteId = 2, Code = "DN2" }
            };
            A.CallTo(() => _deliveryNoteRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<DeliveryNote>>(deliveryNotes));

            // Act
            var result = await _deliveryNoteService.GetDeliveryNotesAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetDeliveryNotesAsync_ReturnsDeliveryNotes_WithItems()
        {
            // Arrange
            var deliveryNotes = new List<DeliveryNote>
            {
                new DeliveryNote
                {
                    DeliveryNoteId = 1,
                    Code = "DN1",
                    DeliveryNoteItems = new List<DeliveryNoteItem>
                    {
                        new DeliveryNoteItem { ArticleId = 1, Quantity = 10, Price = 100 },
                        new DeliveryNoteItem { ArticleId = 2, Quantity = 5, Price = 50 }
                    }
                }
            };
            A.CallTo(() => _deliveryNoteRepository.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<DeliveryNote>>(deliveryNotes));

            // Act
            var result = await _deliveryNoteService.GetDeliveryNotesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result.First().DeliveryNoteItems.Count);
        }

        [Fact]
        public async Task GetDeliveryNoteByIdAsync_ReturnsDeliveryNote_WhenExists()
        {
            // Arrange
            var deliveryNote = new DeliveryNote
            {
                DeliveryNoteId = 1,
                Code = "DN1",
                DeliveryNoteItems = new List<DeliveryNoteItem>
                {
                    new DeliveryNoteItem { ArticleId = 1, Quantity = 10, Price = 200 }
                }
            };
            A.CallTo(() => _deliveryNoteRepository.GetByIdAsync(1))
                .Returns(Task.FromResult<DeliveryNote?>(deliveryNote));

            // Act
            var result = await _deliveryNoteService.GetDeliveryNoteByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result!.DeliveryNoteId);
            Assert.Single(result.DeliveryNoteItems);
        }

        [Fact]
        public async Task GetDeliveryNoteByIdAsync_ReturnsNull_WhenIdIsInvalid()
        {
            // Arrange

            // Act
            var result = await _deliveryNoteService.GetDeliveryNoteByIdAsync(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddDeliveryNoteAsync_CallsRepository_WhenDeliveryNoteIsValid()
        {
            // Arrange
            var deliveryNote = new DeliveryNote
            {
                DeliveryNoteId = 1,
                Code = "DN1",
                DeliveryNoteItems = new List<DeliveryNoteItem>
                {
                    new DeliveryNoteItem { ArticleId = 1, Quantity = 5, Price = 100 }
                }
            };

            // Act
            await _deliveryNoteService.AddDeliveryNoteAsync(deliveryNote);

            // Assert
            A.CallTo(() => _deliveryNoteRepository.AddAsync(deliveryNote))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddDeliveryNoteAsync_DoesNotCallRepository_WhenDeliveryNoteIsNull()
        {
            // Arrange
            DeliveryNote? deliveryNote = null;

            // Act
            await _deliveryNoteService.AddDeliveryNoteAsync(deliveryNote!);

            // Assert
            A.CallTo(() => _deliveryNoteRepository.AddAsync(A<DeliveryNote>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task UpdateDeliveryNoteAsync_CallsRepository_WhenDeliveryNoteIsValid()
        {
            // Arrange
            var deliveryNote = new DeliveryNote { DeliveryNoteId = 1, Code = "DN1" };

            // Act
            await _deliveryNoteService.UpdateDeliveryNoteAsync(deliveryNote);

            // Assert
            A.CallTo(() => _deliveryNoteRepository.UpdateAsync(deliveryNote))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeleteDeliveryNoteAsync_CallsRepository_WhenIdIsValid()
        {
            // Arrange
            int id = 1;

            // Act
            await _deliveryNoteService.DeleteDeliveryNoteAsync(id);

            // Assert
            A.CallTo(() => _deliveryNoteRepository.DeleteAsync(1))
                .MustHaveHappenedOnceExactly();
        }
    }
}