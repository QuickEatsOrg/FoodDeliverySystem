using FoodDelivery.API.DTOs.Tushar;
using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Models;
using FoodDelivery.API.Repositories.Interfaces.Tushar;
using FoodDelivery.API.Services.Interfaces.Tushar;

namespace FoodDelivery.API.Services.Implementations.Tushar
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<IEnumerable<DeliveryDto>> GetAllDeliveriesAsync()
        {
            var orders = await _deliveryRepository.GetAllDeliveriesAsync();

            return orders.Select(MapToDeliveryDto);
        }

        public async Task<DeliveryDto> GetDeliveryByOrderIdAsync(int orderId)
        {
            var order = await _deliveryRepository.GetDeliveryByOrderIdAsync(orderId);

            if (order == null)
            {
                throw new NotFoundException("Delivery/order not found");
            }

            return MapToDeliveryDto(order);
        }

        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesByDriverIdAsync(int driverId)
        {
            var orders = await _deliveryRepository.GetDeliveriesByDriverIdAsync(driverId);

            return orders.Select(MapToDeliveryDto);
        }

        public async Task<DeliveryDto> AssignDriverAsync(AssignDriverDto dto)
        {
            var order = await _deliveryRepository.AssignDriverAsync(dto.OrderId, dto.DriverId);

            if (order == null)
            {
                throw new NotFoundException("Order or driver not found");
            }

            return MapToDeliveryDto(order);
        }

        public async Task<DeliveryDto> UpdateDeliveryStatusAsync(int orderId, UpdateDeliveryStatusDto dto)
        {
            var order = await _deliveryRepository.UpdateDeliveryStatusAsync(orderId, dto.OrderStatus);

            if (order == null)
            {
                throw new NotFoundException("Delivery/order not found");
            }

            return MapToDeliveryDto(order);
        }

        private static DeliveryDto MapToDeliveryDto(Order order)
        {
            return new DeliveryDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,

                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,

                DeliveryDriverId = order.DeliveryDriverId,

                DriverName = order.DeliveryDriver?.DriverName,
                DriverPhone = order.DeliveryDriver?.DriverPhone,
                DriverVehicle = order.DeliveryDriver?.DriverVehicle,
                DriverEmail = order.DeliveryDriver?.DriverEmail,

                OrderStatus = order.OrderStatus
            };
        }
    }
}