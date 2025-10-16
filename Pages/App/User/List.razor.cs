
using SSOPortalX.Data.App.User;
using SSOPortalX.Data.App.User.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SSOPortalX.Pages.App.User
{
    public partial class List
    {
        [Inject]
        private UserService UserService { get; set; } = default!;

        public bool? _visible;
        public UserPage _userPage = new(new List<UserDto>()); // Initialize with empty list
        private List<int> _pageSizes = new() { 10, 25, 50, 100 };
        private readonly List<string> _roleList = new() { "Admin", "User" };
        private readonly List<string> _statusList = new() { "Active", "Inactive" };
        private bool _deleteDialogVisible;
        private UserDto? _userToDelete;
        private readonly List<DataTableHeader<UserDto>> _headers = new()
        {
            new() { Text = "USER", Value = nameof(UserDto.UserName) },
            new() { Text = "EMAIL", Value = nameof(UserDto.Email) },
            new() { Text = "ROLE", Value = nameof(UserDto.Role) },
            new() { Text = "STATUS", Value = nameof(UserDto.Status) },
            new() { Text = "ACTIONS", Value = "Action", Sortable = false }
        };
        private readonly Dictionary<string, string> _roleIconMap = SSOPortalX.Data.App.User.UserService.GetRoleIconMap();

        protected override async Task OnInitializedAsync()
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            var users = await UserService.GetListAsync();
            _userPage = new UserPage(users);
            StateHasChanged();
        }

        private async Task HandleAddUserAsync(SSOPortalX.Data.Models.User newUser)
        {
            await UserService.CreateUserAsync(newUser);
            _visible = false;
            await LoadUsers();
        }

        private void OpenDeleteDialog(UserDto user)
        {
            _userToDelete = user;
            _deleteDialogVisible = true;
        }

        private async Task HandleDeleteUserAsync()
        {
            if (_userToDelete != null && int.TryParse(_userToDelete.Id, out int id))
            {
                await UserService.DeleteUserAsync(id);
                await LoadUsers();
                _deleteDialogVisible = false;
                _userToDelete = null;
            }
        }

        private void NavigateToDetails(string id)
        {
            Nav.NavigateTo($"/app/user/view/{id}");
        }

        private void NavigateToEdit(string id)
        {
            Nav.NavigateTo($"/app/user/edit/{id}");
        }
    }
}
