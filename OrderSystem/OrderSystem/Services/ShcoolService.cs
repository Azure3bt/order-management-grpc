using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Persistence;
using SchoolSystem;

public class ShcoolService : SchoolService.SchoolServiceBase
{
    private readonly OrderDbContext _context;

    public ShcoolService(OrderDbContext context)
    {
        _context = context;
    }

    public override async Task<StudentsResponse> GetAll(Empty request, ServerCallContext context)
    {
        var response = new StudentsResponse();
        response.Students.AddRange(await _context.Students
            .Select(entity => new Student
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                NationalId = entity.NationalId,
                DateOfBirth = entity.DateOfBirth.ToTimestamp()
            }).ToListAsync());

        return response;
    }

    public override async Task<Int32Value> CreateStudent(Student request, ServerCallContext context)
    {
        _context.Students.Add(new OrderModels.School.Student()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            NationalId = request.NationalId,
            DateOfBirth = request.DateOfBirth.ToDateTime()
        });
        await _context.SaveChangesAsync();
        var int32Value = new Int32Value();
        int32Value.Value = request.Id;
        return int32Value;
    }

    public override async Task<Student> EditStudent(Student request, ServerCallContext context)
    {
        var findStudent = await _context.Students.FindAsync(request.Id);

        findStudent.FirstName = request.FirstName;
        findStudent.LastName = request.LastName;
        findStudent.NationalId  = request.NationalId;
        findStudent.DateOfBirth = request.DateOfBirth.ToDateTime();

        _context.Students.Update(findStudent);
        await _context.SaveChangesAsync();

        return request;

    }

    public override async Task<BoolValue> DeleteStudent(Int32Value request, ServerCallContext context)
    {
        var boolValue = new BoolValue();
        var findStudent = await _context.Students.FindAsync(request.Value);

        _context.Students.Remove(findStudent);
        await _context.SaveChangesAsync();
        boolValue.Value = true;

        return boolValue;
    }
}
