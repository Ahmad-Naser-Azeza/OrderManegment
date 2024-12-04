using Order.Domain.Dtos;
using Order.Domain.Entities;
using Order.Domain.Models;
using SharedKernel;

namespace Order.Domain.Mapping;

public static class OrdersModelMapping
{
    public static IEnumerable<OrdersDto?> ToDtos(this IEnumerable<Orders> entities)
    {
        return entities.Select(x => x.ToDto());
    }

    public static OrdersDto? ToDto(this Orders entity)
    {
        return entity == null
            ? null
            : new OrdersDto
            {
                Id = entity.Id,
                CustomerName = entity.CustomerName,
                Price = entity.Price,
                ProductName = entity.ProductName,
                Quantity = entity.Quantity,
                Status = entity.Status
            };
    }
    public static List<Orders> ToEntities(this List<OrdersModel> models)
    {
        if (models != null)
            return models.Select(x => x.ToEntity()).ToList();
        return null;
    }
    public static Orders ToEntity(this OrdersModel model)
    {
        if (model != null)
        {
            return new Orders
            {
                Id = model.Id,
                CustomerName = model.CustomerName,
                Price = model.Price,
                ProductName = model.ProductName,
                Quantity = model.Quantity,
                Status = model.Status
            };
        }
        return null;
    }    
    public static List<OrdersModel> ToModels(this List<OrdersDto> models)
    {
        if (models != null)
            return models.Select(x => x.ToModel()).ToList();
        return null;
    }
    public static OrdersModel ToModel(this OrdersDto dto)
    {
        if (dto != null)
        {
            return new OrdersModel
            {
                Id = dto.Id,
                CustomerName = dto.CustomerName,
                Price = dto.Price,
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                Status = dto.Status
            };
        }
        return null;
    }
}
