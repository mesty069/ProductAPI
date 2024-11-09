using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product.API.Errors;
using Product.API.Extensions;
using Product.Core.Dto;
using Product.Core.Entities;
using Product.Core.Interface;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUsers> userManager,ITokenServices tokenServices, SignInManager<AppUsers> signInManger, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManger;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null) return Unauthorized(new BaseCommonResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (result is null || result.Succeeded == false) return Unauthorized(new BaseCommonResponse(401));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenServices.CreateToken(user)
            });
        }

        /// <summary>
        /// 驗證Email是否已註冊
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("check-email-exist")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result is not null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 註冊帳號
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(ReqisterDto dto)
        {
            if (CheckEmailExist(dto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "該信箱已被註冊過" }
                });
            }
            var user = new AppUsers
            {
                DisplayName = dto.DisplayName,
                UserName = dto.Email,
                Email = dto.Email
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded == false) return BadRequest(new BaseCommonResponse(400));
            return Ok(new UserDto
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                Token = _tokenServices.CreateToken(user),
            });
        }

        [Authorize]
        [HttpGet("Test")]
        public ActionResult<string> Test()
        {
            return "hi";
        }

        /// <summary>
        /// 取得當前用戶資訊
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.FindEmailByClaimPirincipal(HttpContext.User);
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenServices.CreateToken(user),
            });
        }

        /// <summary>
        /// 取得用戶地址資訊
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get-user-address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);
            var _result = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(_result);
        }

        /// <summary>
        /// 修改用戶地址資訊
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Update-user-address")]
        public async Task<IActionResult> GetUserAddress(AddressDto dto)
        {
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(dto);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            return BadRequest($"修改發生錯誤：{HttpContext.User}");
        }
    }
}
