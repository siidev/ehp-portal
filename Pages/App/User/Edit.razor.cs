using Microsoft.AspNetCore.Components;
using SSOPortalX.Data.App.Application;
using SSOPortalX.Data.App.User;
using SSOPortalX.Data.App.User.Dto;
using SSOPortalX.Data.App.UserAppAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Masa.Blazor;
using Masa.Blazor.Presets;

namespace SSOPortalX.Pages.App.User
{
    public partial class Edit
    {
        private UserDto? _userData;
        private StringNumber? tab;
        private bool _birthDateMenu;
        private List<SSOPortalX.Data.Models.Application> _allApplications = new();
        private Dictionary<int, bool> _userAppAccess = new();

        [Inject]
        private UserService UserService { get; set; } = default!;

        [Inject]
        private ApplicationService ApplicationService { get; set; } = default!;

        [Inject]
        private UserAppAccessService UserAppAccessService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private CookieStorage CookieStorage { get; set; } = default!;

        [CascadingParameter]
        private IPageTabsProvider? PageTabsProvider { get; set; }

        [Parameter]
        public string? Id { get; set; }

        public UserDto UserData
        {
            get { return _userData ?? new UserDto("", "", DateOnly.FromDateTime(System.DateTime.Now), "", "", new List<PermissionDto>()); }
            set { _userData = value; }
        }

        protected override async Task OnInitializedAsync()
        {
            if (!int.TryParse(Id, out int userId))
            {
                NavigationManager.NavigateTo("/app/user/list");
                return;
            }

            _userData = await UserService.GetUserByIdAsync(userId);
            if (_userData == null)
            {
                NavigationManager.NavigateTo("/app/user/list");
                return;
            }

            _allApplications = await ApplicationService.GetApplicationsAsync();
            var accessibleAppIds = await UserAppAccessService.GetAppIdsForUserAsync(userId);

            _userAppAccess = _allApplications.ToDictionary(app => app.Id, app => accessibleAppIds.Contains(app.Id));

            UpdateTabTitle();
        }

        private async Task HandleSaveChanges()
        {
            if (_userData != null && int.TryParse(_userData.Id, out int userId))
            {
                // Save user core details
                await UserService.UpdateUserAsync(_userData);

                // Save application access rights
                var selectedAppIds = _userAppAccess.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
                var currentUserIdStr = await CookieStorage.GetAsync("CurrentUserId");
                int? adminId = int.TryParse(currentUserIdStr, out var parsed) ? parsed : null;
                await UserAppAccessService.UpdateUserAccessAsync(userId, selectedAppIds, adminId, "Updated via User Edit");

                NavigationManager.NavigateTo("/app/user/list");
            }
        }

        private void UpdateTabTitle()
        {
            //PageTabsProvider?.UpdateTabTitle(NavigationManager.GetAbsolutePath(), () => T("Edit of {0}", UserData.FullName));
        }
    }
}