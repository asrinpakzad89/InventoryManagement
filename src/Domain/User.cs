namespace Domain;

public class User
    : BaseEntity<int>
{
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
