using System;
using Hospital.Model;
using System.Collections.Generic;
using Hospital.Model.Common;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Common;

namespace Hospital.Service.Common
{
    public interface IDoctorService
    {
        Task<Paging<IDoctorModel>> GetDoctorsAsync(Filter filter, Sorting<IDoctorModel> sorting, Paging<IDoctorModel> paging);
        Task<IDoctorModel> GetDoctorByIdAsync(Guid id);
        Task<bool> DeleteDoctorAsync(Guid id);
        Task<bool> CreateDoctorAsync(IDoctorModel doctor);
        Task<bool> UpdateDoctorAsync(Guid id, IDoctorModel doctor);
        Task<bool> UpdateDoctorAsync(IDoctorModel updatedDoctor);
    }

}
