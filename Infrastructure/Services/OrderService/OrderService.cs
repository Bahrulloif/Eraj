using AutoMapper;
using Domain.DTOs.OrderDTO;
using Domain.Entities;
using Domain.Enum;
using Domain.Filters.OrderFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public OrderService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<Response<List<GetOrderDTO>>> GetOrder(OrderFilter filter, string currentUserId, bool isPrivileged)
    {
        var query = _context.Orders.AsQueryable();
        if (!isPrivileged)
        {
            query = query.Where(o => o.ApplicationUserId == currentUserId);
        }
        if (filter.Id != null)
        {
            query = query.Where(o => o.Id == filter.Id);
        }
        var orders = await query.ToListAsync();
        var result = _mapper.Map<List<GetOrderDTO>>(orders);
        return new Response<List<GetOrderDTO>>(result);
    }

    public async Task<Response<GetOrderDTO>> GetOrderById(int orderId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (find == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this order");
        }
        var mapped = _mapper.Map<GetOrderDTO>(find);
        return new Response<GetOrderDTO>(mapped);
    }
    public async Task<Response<GetOrderDTO>> AddOrder(AddOrderDTO order, string currentUserId)
    {
        // Always the caller's own order — never trust a client-supplied UserId.
        order.UserId = currentUserId;

        // Never trust a client-supplied Price - previously taken as-is, letting anyone order
        // anything at any price they named (confirmed live: a 24000 car ordered for 0.01).
        // Look up the product's real, current price server-side and use that instead.
        var realPrice = await GetRealPrice(order.ProductType, order.ProductId, order.SubCategoryId);
        if (realPrice == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Product not found");
        }
        order.Price = realPrice.Value;

        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (find == null)
        {
            var mapped = _mapper.Map<Order>(order);
            await _context.Orders.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order added successfully");
        }
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Conflict, "Order already exist");

    }

    public async Task<Response<GetOrderDTO>> UpdateOrder(AddOrderDTO order, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (find == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this order");
        }
        if (!isPrivileged)
        {
            // Never let a non-privileged caller reassign the order to someone else.
            order.UserId = currentUserId;
        }

        // Same as AddOrder - re-derive the real price server-side rather than trusting whatever
        // the update payload sent, otherwise an owner could "update" their own order down to any
        // price after the fact just as easily as spoofing it at creation time.
        var realPrice = await GetRealPrice(order.ProductType, order.ProductId, order.SubCategoryId);
        if (realPrice == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Product not found");
        }
        order.Price = realPrice.Value;

        _mapper.Map(order, find);
        _context.Orders.Update(find);
        await _context.SaveChangesAsync();
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order updated successfully");
    }

    // Looks up the product's actual, current price directly from its own table - the only
    // authoritative source. (ProductType, ProductId, SubCategoryId) has to match a real row;
    // ProductType is client-declared (the caller already knows it - they fetched the listing from
    // a type-specific endpoint before ordering it), but that's safe: whichever type they claim,
    // this looks up that exact table by id and uses its real price, so a caller can't manipulate
    // the price by lying about the type - at worst a wrong claim just fails to resolve to a row
    // and the order is rejected below (Product not found), not silently mispriced.
    private async Task<decimal?> GetRealPrice(ProductType productType, int productId, int subCategoryId) =>
        productType switch
        {
            ProductType.Car => await _context.Cars.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.Motorbike => await _context.Motorbikes.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.Truck => await _context.Trucks.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.SpareAccessorTransp => await _context.SpareAccessorTransps.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.NoteBook => await _context.NoteBooks.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.SmartPhone => await _context.SmartPhones.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.Tablet => await _context.Tablets.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.SpareAccessorKomp => await _context.SpareAccessorKomps.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.Apartment => await _context.Apartments.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.CommercialRealEstate => await _context.CommercialRealEstates.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            ProductType.Cottage => await _context.Cottages.Where(x => x.Id == productId && x.SubCategoryId == subCategoryId).Select(x => (decimal?)x.Price).FirstOrDefaultAsync(),
            _ => null
        };
    public async Task<Response<GetOrderDTO>> DeleteOrder(int orderId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (find == null)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.NotFound, "Order Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetOrderDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this order");
        }
        _context.Orders.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetOrderDTO>(System.Net.HttpStatusCode.OK, "Order deleted successfully");
    }
}
