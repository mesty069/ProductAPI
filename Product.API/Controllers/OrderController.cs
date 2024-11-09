using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.API.Errors;
using Product.Core.Dto;
using Product.Core.Entities.Order;
using Product.Core.Interface;
using Product.Core.Services;
using Product.Infrastructure.Repository;
using System.Security.Claims;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        //private readonly IMapper _mapper;
        //private readonly IUnitOfWork _uOW;
        //private readonly IOrderServices _orderServices;

        //public OrderController(IMapper mapper, IUnitOfWork uOW, IOrderServices orderServices)
        //{
        //    _mapper = mapper;
        //    _uOW = uOW;
        //    _orderServices = orderServices;
        //}

        ///// <summary>
        ///// 創建訂單
        ///// </summary>
        ///// <param name="orderDto"></param>
        ///// <returns></returns>
        //[HttpPost("Create-Order")]
        //public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        //{ 
        //    var Email = HttpContext.User?.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
        //    var Address = _mapper.Map<AddressDto, ShipAddress>(orderDto.ShipToAddress);
        //    var order = await _orderServices.CreateOrderAsync(Email, orderDto.DeliveryMethodId, orderDto.BasketId, Address);
        //    if (order is null) return BadRequest(new BaseCommonResponse(400, "創建訂單時出錯"));
        //    return Ok(order);
        //}

        ///// <summary>
        ///// 查詢用戶訂單
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("get-order-for-user")]
        //public async Task<ActionResult<IReadOnlyList<OrderToretunrnDto>>> GetOrderForUser()
        //{
        //    var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        //    var order = await _orderServices.GetOrdersForUserAsync(email);
        //    var result = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToretunrnDto>>(order);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// 根據訂單ID查詢訂單
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost("get-order-by-id/{id}")]
        //public async Task<ActionResult<IReadOnlyList<OrderToretunrnDto>>> GetorderById(int  id)
        //{
        //    var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        //    var order = await _orderServices.GetOrderByIdAsync(id,email);
        //    if (order is null) return NotFound(new BaseCommonResponse(404));
        //    var result = _mapper.Map<Order,OrderToretunrnDto>(order);
        //    return Ok(result);
        //}

        ///// <summary>
        ///// 取得配送方式列表
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("get-delivery-method")]
        //public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        //{
        //    return Ok(await _orderServices.GetDeliveryMethodsAsync());
        //}
    }
}
