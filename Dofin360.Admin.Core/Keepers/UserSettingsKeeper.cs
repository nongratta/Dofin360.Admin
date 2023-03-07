using Dofin360.Admin.Model;
namespace Dofin360.Admin.Core;
public class UserSettingsKeeper : Repository<D360UserSettings>, IUserSettingsKeeper { public UserSettingsKeeper(string connectionString) : base(connectionString, "D360UserSettings") { } }