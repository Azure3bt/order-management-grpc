using OrderModels.School;

namespace OrderSystem.SDK.Contract;

public interface ISchoolService
{
    Task<IEnumerable<Student>> GetAll(CancellationToken cancellationToken);
    Task<int> CreateStudent(Student request, CancellationToken cancellationToken);
    Task<Student> EditStudent(Student request, CancellationToken cancellationToken);
    Task<bool> DeleteStudent(int studentId, CancellationToken cancellationToken);
}
