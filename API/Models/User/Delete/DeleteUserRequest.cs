namespace API.Models.User.Delete
{
    public class DeleteUserRequest
    {
        public Guid UserToDeleteGuid { get; set; }
        public bool IsSoft { get; set; }
    }
}
