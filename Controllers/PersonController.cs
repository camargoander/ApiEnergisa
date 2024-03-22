using ApiEnergisa.Models;
using ApiEnergisa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiEnergisa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [ProducesResponseType(typeof(List<Person>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _personRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(typeof(Person), (int)HttpStatusCode.OK)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var person = await _personRepository.GetByIdAsync(id);
                if (person == null)
                {
                    return NotFound();
                }
                return Ok(person);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Person person)
        {
            try
            {
                bool isSuccess = await _personRepository.AddAsync(person);
                if (isSuccess)
                {
                    return Ok("Pessoa Adicionada!");
                }
                return BadRequest("Falha ao Adicionar Pessoa!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Person person)
        {
            try
            {
                bool isSuccess = await _personRepository.UpdateAsync(person);
                if (isSuccess)
                {
                    return Ok("Pessoa Atualizada!");
                }
                return BadRequest("Falha ao Atualizar Pessoa!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool isSuccess = await _personRepository.DeleteAsync(id);
                if (isSuccess)
                {
                    return Ok("Pessoa Deletada!");
                }
                return BadRequest("Falha ao Deletar Pessoa!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
