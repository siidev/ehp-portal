
using Microsoft.AspNetCore.Components;
using SSOPortalX.Data.App.User;
using SSOPortalX.Data.App.User.Dto;
using SSOPortalX.Data.App.Application;
using SSOPortalX.Data.App.UserAppAccess;
using Masa.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSOPortalX.Pages.App.User
{
    public partial class View
    {
        private UserDto? _userData;
        private StringNumber? tab;
        private List<SSOPortalX.Data.Models.Application> _allApplications = new();
        private Dictionary<int, bool> _userAppAccess = new();

        [Inject]
        private UserService UserService { get; set; } = default!;

        [Inject]
        private ApplicationService ApplicationService { get; set; } = default!;

        [Inject]
        private UserAppAccessService UserAppAccessService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public string? Id { get; set; }

        public UserDto UserData
        {
            get { return _userData ?? new UserDto("", "", System.DateOnly.FromDateTime(System.DateTime.Now), "", "", new()); }
            set { _userData = value; }
        }

        protected override async Task OnInitializedAsync()
        {
            if (int.TryParse(Id, out int userId))
            {
                _userData = await UserService.GetUserByIdAsync(userId);
                await LoadApplicationAccess(userId);
            }

            if (_userData == null)
            {
                // Handle user not found
                NavigationManager.NavigateTo("/app/user/list");
            }
        }

        private async Task LoadApplicationAccess(int userId)
        {
            _allApplications = await ApplicationService.GetApplicationsAsync();
            var userAppIds = await UserAppAccessService.GetAppIdsForUserAsync(userId);
            
            _userAppAccess = new Dictionary<int, bool>();
            foreach (var app in _allApplications)
            {
                _userAppAccess[app.Id] = userAppIds.Contains(app.Id);
            }
        }
    }
}
