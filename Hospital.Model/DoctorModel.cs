using System;
using Hospital.Model.Common;
using Hospital.Model;
namespace Hospital.Model
{
    public class DoctorModel : IDoctorModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public Guid SpecializationId { get; set; }
        public Guid AddressId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
