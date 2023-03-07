using Microsoft.Extensions.Configuration;
using Dofin360.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dofin360.Admin.Core;
public interface IUserSettingsService
{
    Task<object> Get(int skip = 0, int take = 20);
    Task<object> Get(Guid id);
    Task Post(D360UserSettingsSetViewModel model);
    Task Delete(Guid id);
    Task Put(Guid id, D360UserSettingsSetViewModel model);
}
public class UserSettingsService : IUserSettingsService
{
    private readonly UserSettingsKeeper _keeper;
    public UserSettingsService(IConfiguration config) => _keeper = new UserSettingsKeeper(config["DofinDB"]);

    public async Task<object> Get(Guid id) => 
        await _keeper.GetById(id);

    public async Task<object> Get(int skip = 0, int take = 20) => await _keeper.Find(x => true, skip, take);    

    public async Task Post(D360UserSettingsSetViewModel model)
    {
        var id = Guid.NewGuid();

        var setting = new D360UserSettings {
            Id = id,
            FirstName = model.FirstName,
            SecondName = model.SecondName
        };

        await _keeper.Add(setting);
    }

    public async Task Delete(Guid id) =>
        await _keeper.Delete(id);

    public async Task Put(Guid id, D360UserSettingsSetViewModel model) =>
        await _keeper.Update(new D360UserSettings { Id = id, FirstName = model.FirstName, SecondName = model.SecondName }, id);
}
