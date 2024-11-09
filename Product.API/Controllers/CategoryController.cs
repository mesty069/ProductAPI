using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Product.Core.Entities;
using Product.Core.Interface;
using Product.Infrastructure.Data;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork Uow, IMapper mapper)
        {
            _uow = Uow;
            _mapper = mapper;
        }

        /// <summary>
        /// 取得全部資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-category")]
        public async Task<ActionResult> Get()
        {
            var all_category = await _uow.CategoryRepository.GetAllAsync();
            if (all_category != null)
            {
                var res = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<ListCategoryDto>>(all_category);
                return Ok(res);
            }
            return BadRequest("Not Found");
        }

        /// <summary>
        /// 取得指定資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _uow.CategoryRepository.GetAsync(id);
            if (category == null)
                return BadRequest($"沒有找到這個編號：[{id}]");
            return Ok(_mapper.Map<Category,ListCategoryDto>(category));
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("add-new-category")]
        public async Task<ActionResult> Post(CategoryDto category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = _mapper.Map<Category>(category);
                    await _uow.CategoryRepository.AddAsync(res);
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update-exiting-category-by-id/{id}")]
        public async Task<ActionResult> Put(int id, CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exiting_category = await _uow.CategoryRepository.GetAsync(id);
                    if (exiting_category != null)
                    {
                        _mapper.Map(categoryDto, exiting_category);
                    }
                    await _uow.CategoryRepository.UpdateAsync(id, exiting_category);
                    return Ok(exiting_category);
                }
                return BadRequest($"沒有找到類別編號：{id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("update-exiting-category-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var exiting_category = await _uow.CategoryRepository.GetAsync(id);
                if (exiting_category != null)
                {
                    await _uow.CategoryRepository.DeleteAsync(id);
                    return Ok($"類別[{exiting_category.Name}]已被刪除");
                }
                return BadRequest($"沒有找到類別編號：{id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
