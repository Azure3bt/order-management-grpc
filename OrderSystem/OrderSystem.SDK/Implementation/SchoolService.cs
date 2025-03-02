using Google.Protobuf.WellKnownTypes;
using OrderModels.School;
using OrderSystem.SDK.Contract;

namespace OrderSystem.SDK.Implementation;

internal class SchoolService : ISchoolService
{
    private readonly SchoolSystem.SchoolService.SchoolServiceClient _schoolServiceClient;

    public SchoolService(SchoolSystem.SchoolService.SchoolServiceClient schoolServiceClient)
    {
        _schoolServiceClient = schoolServiceClient;
    }

    public async Task<int> CreateStudent(Student request, CancellationToken cancellationToken)
    {
        var result = await _schoolServiceClient.CreateStudentAsync(new SchoolSystem.Student()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            NationalId = request.NationalId,
            DateOfBirth = new Timestamp() { Seconds = DateTime.Now.Second }
        });
        return result.Value;
    }

    public async Task<bool> DeleteStudent(int studentId, CancellationToken cancellationToken)
    {
        return (await _schoolServiceClient.DeleteStudentAsync(new Int32Value() { Value = studentId })).Value;
    }

    public async Task<Student> EditStudent(Student request, CancellationToken cancellationToken)
    {
        var result = await _schoolServiceClient.EditStudentAsync(new SchoolSystem.Student()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            NationalId = request.NationalId,
            DateOfBirth = new Timestamp() { Seconds = DateTime.Now.Second }
        });

        return new Student()
        {
            FirstName = result.FirstName,
            LastName = result.LastName,
            NationalId = result.NationalId,
            DateOfBirth = result.DateOfBirth.ToDateTime()
        };
    }

    public async Task<IEnumerable<Student>> GetAll(CancellationToken cancellationToken)
    {
        var data = await _schoolServiceClient.GetAllAsync(new Empty());

        return data.Students.Select(x => new Student()
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            NationalId = x.NationalId,
            DateOfBirth = x.DateOfBirth.ToDateTime()
        });
    }
}
