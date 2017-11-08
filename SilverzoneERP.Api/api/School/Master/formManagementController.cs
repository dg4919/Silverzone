using SilverzoneERP.Data;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
     [Authorize]
    public class formManagementController : ApiController
    {             
        IRolePermissionRepository _rolePermissionRepository;
        IFormManagementRepository _formManagementRepository;
        IUserPermissionRepository _userPermissionRepository;
        //*****************  Constructor********************************

        public formManagementController( IRolePermissionRepository _rolePermissionRepository, IFormManagementRepository _formManagementRepository, IUserPermissionRepository _userPermissionRepository)
        {     
            this._rolePermissionRepository = _rolePermissionRepository;
            this._formManagementRepository = _formManagementRepository;
            this._userPermissionRepository = _userPermissionRepository;
        }        
        [HttpGet]
        public IHttpActionResult GetFormGroupWise()
        {

            var data = from frm in _formManagementRepository.FindBy(x => x.Status == true && x.FormParentId == null).OrderBy(x=>x.FormOrder)
                       select new {
                           Header=frm.FormName,
                           Forms = frm.ChildFormManagement.Where(x=>x.FormName.ToLower()!="divider"&& x.Status==true ).OrderBy(x=>x.FormOrder).Select(x => new { FormId=x.Id, x.FormName, Permission = new { Add = false, Edit = false, Read = false, Print = false, Delete = false } })
                       };                      
            return Ok(new { result = data });
        }
    }
}
