using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Wrappers;
using Type = Howest.MagicCards.DAL.Models.Type;

namespace HWebAPI.Controllers
{
    [Route("api/types")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeRepository _typeRepo;
        private readonly IMapper _mapper;

        public TypeController(ITypeRepository repo, IMapper mapper)
        {
            _typeRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IQueryable<TypeDTO>> GetNormalTypes([FromServices] IConfiguration config)
        {
            return (_typeRepo.GetNormalTypes() is IQueryable<Type>allTypes )
                ? Ok(allTypes.ProjectTo<TypeDTO>(_mapper.ConfigurationProvider))
                : NotFound(new Response<TypeDTO>()
                {
                    Succeeded = false,
                    Errors = new string[] { "404" },
                    Message = $"No types where found"
                });
        }
    }
}