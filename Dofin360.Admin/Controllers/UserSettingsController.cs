using Dofin360.Admin.Core;
using Dofin360.Admin.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using System.Text.Json;

namespace Dofin360.Admin.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class UserSettingsController : Controller
    {
        private readonly IUserSettingsService _service;

        public UserSettingsController(IUserSettingsService service) => _service = service;

        /// <summary>
        /// Получение списка свойств пользователя
        /// </summary>    
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get(int skip = 0, int take = 20) => await _service.Get(skip, take);

        /// <summary>
        /// Получение свойств пользователя
        /// </summary>    
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<object> Get(Guid id) => await _service.Get(id);

        /// <summary>
        /// Создание свойств пользователя
        /// </summary>    
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] D360UserSettingsSetViewModel model) => await _service.Post(model);

        /// <summary>
        /// Изменение свойств пользователя
        /// </summary>    
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody] D360UserSettingsSetViewModel model) => await _service.Put(id, model);

        /// <summary>
        /// Удаление свойств пользователя
        /// </summary>    
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(Guid id) => await _service.Delete(id);
    }