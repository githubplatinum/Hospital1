using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Model.Common;
using Hospital.Repository.Common;
using Hospital.Service.Common;
using Hospital.Common;

namespace Hospital.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<Paging<IDoctorModel>> GetDoctorsAsync(Filter filter, Sorting<IDoctorModel> sorting, Paging<IDoctorModel> paging)
        {
            List<IDoctorModel> doctorModels = await _doctorRepository.GetDoctorsAsync(filter, sorting, paging);
            return new Paging<IDoctorModel>(doctorModels, paging.PageNumber, paging.PageSize, doctorModels.Count);
        }

        public async Task<IDoctorModel> GetDoctorByIdAsync(Guid id)
        {
            DoctorModel doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            return doctor;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            bool result = await _doctorRepository.DeleteDoctorAsync(id);
            return result;
        }

        public async Task<bool> CreateDoctorAsync(IDoctorModel doctor)
        {
            bool result = await _doctorRepository.CreateDoctorAsync((DoctorModel)doctor);
            return result;
        }

        public async Task<bool> UpdateDoctorAsync(Guid id, IDoctorModel doctor)
        {
            bool result = await _doctorRepository.UpdateDoctorAsync(id, (DoctorModel)doctor);
            return result;
        }

        public async Task<bool> UpdateDoctorAsync(IDoctorModel updatedDoctor)
        {
            bool result = await _doctorRepository.UpdateDoctorAsync(updatedDoctor.Id, (DoctorModel)updatedDoctor);
            return result;
        }
    }
}
