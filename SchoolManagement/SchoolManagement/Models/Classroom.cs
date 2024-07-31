namespace SchoolManagement.Models;

public class Classroom : ISchoolRoom
{
    public bool HasComputers { get; set; }
    public bool HasProjector { get; set; }
    public bool HasWhiteboard { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
}