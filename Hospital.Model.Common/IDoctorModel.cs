using System;

namespace Hospital.Model.Common
{
    public interface IDoctorModel
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime Dob { get; set; }
        Guid SpecializationId { get; set; }
        Guid AddressId { get; set; }
        Guid DepartmentId { get; set; }
    }
}
