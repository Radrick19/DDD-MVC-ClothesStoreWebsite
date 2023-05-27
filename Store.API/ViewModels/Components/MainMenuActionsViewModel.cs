using Store.Domain.Enums;

namespace Store.API.ViewModels.Components
{
    public class MainMenuActionsViewModel
    {
        public UserRole UserRole { get; set; } = UserRole.Unloged;
        public string Username { get; set; }
        public int ItemsInCart { get; set; } = 0;
    }
}
