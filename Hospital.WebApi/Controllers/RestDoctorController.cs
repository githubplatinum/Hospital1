using System;
using Hospital.Repository;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hospital.Model.Common;
using Hospital.Service.Common;
using Hospital.Model;
using Hospital.Service;
using System.Threading.Tasks;
using Hospital.Common;
namespace Hospital.WebApi.Controllers
{
    public class RestDoctorController : ApiController
    {
        private readonly IDoctorService doctorService;

        public RestDoctorController()
        {
            doctorService = new DoctorService(new DoctorRepository());
        }

        public RestDoctorController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }
        /*------------------------------------------------------------------------------*/
        public async Task<HttpResponseMessage> GetDoctorsAsync(Guid? specializationId = null, DateTime? dob = null, string searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            Filter filter = new Filter();
            filter.SearchQuery = searchQuery;
            filter.Dob = dob;
            filter.SpecializationId = specializationId;

            Sorting<IDoctorModel> sorting = new Sorting<IDoctorModel>("LastName", true);

            Paging<IDoctorModel> paging = await GetDoctorsAsync(filter, sorting, new Paging<IDoctorModel>(null, pageNumber, pageSize, 0));

            if (paging.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, paging);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No doctors found.");
        }


        /*------------------------------------------------------------------------------*/



        [HttpGet]
        [Route("api/doctor/{id}")]
        public async Task<HttpResponseMessage> GetDoctorAsync(Guid id)
        {
            IDoctorModel doctor = await doctorService.GetDoctorByIdAsync(id);
            if (doctor != null)
                return Request.CreateResponse(HttpStatusCode.OK, doctor);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Doctor with Id {id} not found.");
        }

        [HttpPost]
        [Route("api/doctor")]
        public async Task<HttpResponseMessage> CreateDoctorAsync([FromBody] IDoctorModel newDoctor)
        {
            if (newDoctor == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid doctor data");

            bool result = await doctorService.CreateDoctorAsync(newDoctor);
            if (result)
                return Request.CreateResponse(HttpStatusCode.Created, newDoctor);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to create doctor.");
        }

        [HttpPut]
        [Route("api/doctor/{id}")]
        public async Task<HttpResponseMessage> UpdateDoctorAsync(Guid id, [FromBody] IDoctorModel updatedDoctor)
        {
            if (updatedDoctor == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid doctor data");

            updatedDoctor.Id = id;
            bool result = await doctorService.UpdateDoctorAsync(updatedDoctor);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, "Doctor's data updated");
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Doctor with Id {id} not found.");
        }

        [HttpDelete]
        [Route("api/doctor/{id}")]
        public async Task<HttpResponseMessage> DeleteDoctorAsync(Guid id)
        {
            bool result = await doctorService.DeleteDoctorAsync(id);
            if (result)
                return Request.CreateResponse(HttpStatusCode.OK, $"Doctor with Id {id} successfully deleted");
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Doctor with Id {id} not found.");
        }
    }
}
