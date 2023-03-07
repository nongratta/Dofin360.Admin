using Dofin360.Admin.Model;
using MongoDB.Bson;
using System.Linq.Expressions;
namespace Dofin360.Admin.Core;
public interface IUserSettingsKeeper : IRepository<D360UserSettings> { }
