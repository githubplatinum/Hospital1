using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Common;

namespace Hospital.Repository.Common
{
    public interface IDoctorRepository
    {
        Task<Paging<DoctorModel>> GetDoctorsAsync(Filter filtering, Sorting<DoctorModel> sorting, Paging<DoctorModel> paging);
        Task<DoctorModel> GetDoctorByIdAsync(Guid id);
        Task<bool> CreateDoctorAsync(DoctorModel doctor);
        Task<bool> UpdateDoctorAsync(Guid id, DoctorModel doctor);
        Task<bool> DeleteDoctorAsync(Guid id);
    }
}
