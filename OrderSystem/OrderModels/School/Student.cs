namespace OrderModels.School;

public record Student
{
    public int Id { get; set; }
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string NationalId { get; set; } = default!;
	public DateTime DateOfBirth { get; set; }
}