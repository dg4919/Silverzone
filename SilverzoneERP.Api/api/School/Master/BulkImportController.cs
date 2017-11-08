using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;
using SilverzoneERP.Entities.ViewModel.School;
using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Api.api.School
{
    public class BulkImportController : ApiController
    {
        ISchoolRepository _schoolRepository;
        ICountryRepository _countryRepository;
        IZoneRepository _zoneRepository;
        IStateRepository _stateRepository;
        IDistrictRepository _districtRepository;
       
        ICityRepository _cityRepository;
        public BulkImportController(ICityRepository _cityRepository, ISchoolRepository _schoolRepository, ICountryRepository _countryRepository, IZoneRepository _zoneRepository, IStateRepository _stateRepository, IDistrictRepository _districtRepository)
        {
            this._cityRepository = _cityRepository;
            this._schoolRepository = _schoolRepository;         
            this._countryRepository=_countryRepository;
            this._zoneRepository = _zoneRepository;
            this._stateRepository = _stateRepository;
            this._districtRepository = _districtRepository;
        }

        #region=============================Upload Excel File====================================
        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> KeyList =new Dictionary<string, string>();
            JArray _header = new JArray();
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["0"];
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments"));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments"));
                var filepath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments"),
                                                Guid.NewGuid().ToString().Replace("-", "")+"_"+httpPostedFile.FileName);
                httpPostedFile.SaveAs(filepath);
               
                if (File.Exists(filepath))
                {                   
                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filepath, false))
                    {
                        WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                        IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                        string relationshipId = sheets.First().Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();
                        
                        foreach (Cell cell in rows.ElementAt(0))
                        {
                            string _headerName = GetCellReference(spreadSheetDocument, cell, ref KeyList);
                            dt.Columns.Add(_headerName);
                            _header.Add(_headerName);
                        }
                        bool firstTime = true;
                        int index = 1;
                        foreach (Row row in rows)
                        {
                            if (!firstTime)
                            {
                                DataRow tempRow = dt.NewRow();
                                for (int j = 0; j < row.Descendants<Cell>().Count(); j++)
                                {
                                    Cell val = row.Descendants<Cell>().ElementAt(j);                                    
                                    foreach (var item in KeyList)
                                    {
                                        var sad = row.Elements<Cell>().Where(x => x.CellReference.InnerText == (item.Key + index)).FirstOrDefault();
                                        if (sad != null)
                                        {
                                            var colname = KeyList[item.Key];                                           
                                            tempRow[colname] = GetCellValue(spreadSheetDocument,sad);
                                        }                                       
                                    }                                    
                                }                                
                                dt.Rows.Add(tempRow);
                            }
                            else
                                firstTime = false;
                            index++;
                        }
                    }
                   
                    File.Delete(filepath);
                }               
            }
            var data =JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
            return Ok(new { result = new {  Header = _header ,Data= data}  });
        }
        private string GetCellReference(SpreadsheetDocument document, Cell cell,ref Dictionary<string, string> KeyList)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                var _innerText = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                KeyList.Add(Regex.Replace(cell.CellReference, @"\d", ""), _innerText);
                return _innerText;
            }
            else
            {
                KeyList.Add(cell.CellReference, value);
                return null;
            }
        }
     
        public  string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue==null?null:cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        #endregion

        #region ============================Verify Data======================
        [HttpPost]
        public IHttpActionResult VerifySchool(List<SchoolVerify>model)
        {
            try
            {
                
                List<SchoolVerify> CorrectData = model.Where(x => x.SchCode != 0 && x.SchPinCode != 0 && x.SchName != null && x.SchAddress != null && x.City != null).ToList();
                List<SchoolVerify> DistinctData = model.GroupBy(x => new { x.SchCode, x.SchPinCode,x.SchName,x.SchAddress,x.City }).Select(grp => grp.First()).ToList();
                var CorrectGuidList = CorrectData.Select(x => x.Guid).ToList();
                var DistinctGuidList = DistinctData.Select(x => x.Guid).ToList();
                List<SchoolVerify> Wrong = model.Where(x => !CorrectGuidList.Contains(x.Guid)).ToList();

                List<SchoolVerify> ExistData = new List<SchoolVerify>();


                List<SchoolVerify> Duplicate = CorrectData.Where(x => !DistinctGuidList.Contains(x.Guid)).ToList();

                foreach (SchoolVerify item in DistinctData)
                {
                    var _Duplicate = _schoolRepository.FindBy(x => x.SchName.Trim().ToLower().Equals(item.SchName.Trim().ToLower()) && x.SchPinCode == item.SchPinCode).ToList();
                    if (_Duplicate.Count != 0)
                    {
                        ExistData.Add(item);
                    }                    
                }
                var ExistGuid = ExistData.Select(x => x.Guid).ToList();
                var Correct = DistinctData.Where(x => !ExistGuid.Contains(x.Guid));

                //var _schoolCodeList = _schoolRepository.GetSchoolCodeList();

                //var ExistData = model.Where(x => _schoolCodeList.Contains(x.SchCode)).ToList();
                //var _ExistData_SchoolCodeList = ExistData.Select(x => x.SchCode).ToList();
                //var Duplicate_Correct = model.Where(x => x.SchCode == 0&&x.SchPinCode != 0 && x.SchName != null && x.SchAddress != null && x.City != null && !_ExistData_SchoolCodeList.Contains(x.SchCode)).ToList();

                //var Correct = model.Where(x => x.SchCode != 0&&x.SchPinCode!=0 && x.SchName != null && x.SchAddress != null && x.City!= null && !_ExistData_SchoolCodeList.Contains(x.SchCode)).ToList();
                //List<SchoolVerify> Duplicate = new List<SchoolVerify>();
                //foreach (SchoolVerify item in Duplicate_Correct)
                //{
                //    var _Duplicate = _schoolRepository.FindBy(x => x.SchName.Trim().ToLower().Equals(item.SchName.Trim().ToLower()) && x.SchPinCode == item.SchPinCode).ToList();
                //    if (_Duplicate.Count != 0)
                //    {
                //        Duplicate.Add(item);
                //        Correct.Add(item);
                //    }

                //}
                //var Dup_Guid_List = Duplicate.Select(x => x.Guid).ToList();
                //var Wrong = model.Where(x => x.SchCode == 0&& !Dup_Guid_List.Contains(x.Guid));

                return Ok(new { Result = "success", Correct = Correct, ExistData = ExistData, Wrong = Wrong, Duplicate= Duplicate });
            }
            catch (Exception ex)
            {
                return Ok(new { Result="error",message=ex.Message });
            }                        
        }

        [HttpPost]
        public IHttpActionResult VerifyCountry(List<Country> model)
        {
            try
            {
                var _CountryNameList = _countryRepository.GetAll().Select(x=>x.CountryName.Trim().ToLower()).ToList();

                var ExistData = model.Where(x => _CountryNameList.Contains(x.CountryName.Trim().ToLower()));
                
                var Correct = model.Where(x => x.CountryName != null &&!_CountryNameList.Contains(x.CountryName.Trim().ToLower())).ToList();

                List<Country> Duplicate = new List<Country>();
                               
                var Wrong = model.Where(x => x.CountryName == null);

                return Ok(new { Result = "success", Correct = Correct, ExistData = ExistData, Wrong = Wrong, Duplicate = Duplicate });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult VerifyZone(List<ZoneVerify> model)
        {
            try
            {
                List<ZoneVerify> CorrectData = model.Where(x => x.ZoneName != null && x.Country != null).ToList();
                List<ZoneVerify> DistinctData = model.GroupBy(x => new { x.ZoneName, x.Country }).Select(grp => grp.First()).ToList();
                var CorrectGuidList = CorrectData.Select(x => x.Guid).ToList();
                var DistinctGuidList = DistinctData.Select(x => x.Guid).ToList();
                List<ZoneVerify> Wrong = model.Where(x=>!CorrectGuidList.Contains(x.Guid)).ToList();

                List<ZoneVerify> ExistData = new List<ZoneVerify>();
                               

                List<ZoneVerify> Duplicate = CorrectData.Where(x => !DistinctGuidList.Contains(x.Guid)).ToList();


                foreach (ZoneVerify item in DistinctData)
                {
                    if(_zoneRepository.Exists(item.Country,item.ZoneName))
                    {
                        ExistData.Add(item);
                    }                    
                }
                var ExistGuid = ExistData.Select(x => x.Guid).ToList();
                var Correct = DistinctData.Where(x => !ExistGuid.Contains(x.Guid));

                return Ok(new { Result = "success", Correct = Correct, ExistData = ExistData, Wrong = Wrong, Duplicate = Duplicate });                
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult VerifyState(List<StateVerify> model)
        {
            try
            {
                List<StateVerify> CorrectData = model.Where(x => x.StateName != null && x.StateCode != null && x.Zone != null && x.Country != null).ToList();
                List<StateVerify> DistinctData = model.GroupBy(x => new { x.StateName, x.StateCode,x.Zone,x.Country }).Select(grp => grp.First()).ToList();
                var CorrectGuidList = CorrectData.Select(x => x.Guid).ToList();
                var DistinctGuidList = DistinctData.Select(x => x.Guid).ToList();
                List<StateVerify> Wrong = model.Where(x => !CorrectGuidList.Contains(x.Guid)).ToList();

                List<StateVerify> ExistData = new List<StateVerify>();
            
                List<StateVerify> Duplicate = CorrectData.Where(x => !DistinctGuidList.Contains(x.Guid)).ToList();
                
                foreach (StateVerify item in DistinctData)
                {
                    if(_stateRepository.Exists(item.StateName,item.StateCode,item.Zone,item.Country))
                    {
                        ExistData.Add(item);
                    }                    
                }
                var ExistGuid = ExistData.Select(x => x.Guid).ToList();
                List<StateVerify> Correct = DistinctData.Where(x => !ExistGuid.Contains(x.Guid)).ToList();

                return Ok(new { Result = "success", Correct = Correct, ExistData = ExistData, Wrong = Wrong, Duplicate = Duplicate });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult VerifyDistrict(List<DistrictVerify> model)
        {
            try
            {
                List<DistrictVerify> CorrectData = model.Where(x => x.DistrictName != null && x.State != null && x.Zone != null && x.Country != null).ToList();
                List<DistrictVerify> DistinctData = model.GroupBy(x => new { x.DistrictName, x.State,  x.Zone, x.Country }).Select(grp => grp.First()).ToList();
                var CorrectGuidList = CorrectData.Select(x => x.Guid).ToList();
                var DistinctGuidList = DistinctData.Select(x => x.Guid).ToList();
                List<DistrictVerify> Wrong = model.Where(x => !CorrectGuidList.Contains(x.Guid)).ToList();

                List<DistrictVerify> ExistData = new List<DistrictVerify>();

                List<DistrictVerify> Duplicate = CorrectData.Where(x => !DistinctGuidList.Contains(x.Guid)).ToList();

                //=====================
    
                foreach (DistrictVerify item in DistinctData)
                {
                    if (_districtRepository.Exists(item.DistrictName, item.Country,item.Zone,item.State))
                        ExistData.Add(item);                    
                }
                var ExistGuid = ExistData.Select(x => x.Guid).ToList();
                var Correct = DistinctData.Where(x => !ExistGuid.Contains(x.Guid));

                return Ok(new { Result = "success", Correct = Correct, ExistData = ExistData, Wrong = Wrong, Duplicate = Duplicate });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }
        [HttpPost]
        public IHttpActionResult VerifyCity(List<CityVerify> model)
        {
            try
            {
                List<CityVerify> CorrectData = model.Where(x =>x.CityName!=null && x.District != null && x.State != null && x.Zone != null && x.Country != null).ToList();
                List<CityVerify> DistinctData = model.GroupBy(x => new {x.CityName, x.District, x.State, x.Zone, x.Country }).Select(grp => grp.First()).ToList();
                var CorrectGuidList = CorrectData.Select(x => x.Guid).ToList();
                var DistinctGuidList = DistinctData.Select(x => x.Guid).ToList();
                List<CityVerify> Wrong = model.Where(x => !CorrectGuidList.Contains(x.Guid)).ToList();

                List<CityVerify> ExistData = new List<CityVerify>();

                List<CityVerify> Duplicate = CorrectData.Where(x => !DistinctGuidList.Contains(x.Guid)).ToList();

                //=====================
                
                foreach (CityVerify item in CorrectData)
                {
                    if (_cityRepository.Exists(item.CityName,item.District, item.State, item.Zone, item.Country))
                        ExistData.Add(item);                    
                }
                var ExistGuid = ExistData.Select(x => x.Guid).ToList();
                var Correct = DistinctData.Where(x => !ExistGuid.Contains(x.Guid)).ToList();

                return Ok(new { Result = "success", Correct = Correct, ExistData = ExistData, Wrong = Wrong, Duplicate = Duplicate });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }

        #endregion

        #region ======================Submit=========================
        [HttpPost]
        public IHttpActionResult SaveSchool(List<SchoolVerify> model)
        {
            try
            {
                List<SchoolVerify> Failed = new List<SchoolVerify>();
                List<SchoolVerify> Success = new List<SchoolVerify>();
                foreach (SchoolVerify _school in model)
                {
                    try
                    {
                        var _city = _cityRepository.FindBy(x => x.CityName.Trim().ToLower().Equals(_school.City)).SingleOrDefault();
                        if(_city!=null)
                        {
                            var _schoolDetail = _schoolRepository.Create(new Entities.Models.School
                            {
                                SchCode = _school.SchCode == 0 ? 9999 : _school.SchCode,
                                SchName = _school.SchName,
                                SchAddress = _school.SchAddress,
                                SchAltAddress = _school.SchAltAddress,
                                CityId = _city.Id,
                                CountryId = _city.CountryId,
                                ZoneId = _city.ZoneId,
                                StateId = _city.StateId,
                                DistrictId = _city.DistrictId,
                                SchEmail = _school.SchEmail,
                                SchWebSite = _school.SchWebSite,
                                SchPhoneNo = _school.SchPhoneNo,
                                SchFaxNo = _school.SchFaxNo,
                                SchPinCode = _school.SchPinCode,
                                SchBoard = _school.SchBoard,
                                SchAffiliationNo = _school.SchAffiliationNo,
                            });
                            if (_school.SchCode == 0)
                            {
                                _schoolDetail.SchCode = _schoolDetail.SchCode + _schoolDetail.Id;
                                _schoolRepository.Update(_schoolDetail);
                                _school.SchCode = _schoolDetail.SchCode;
                            }
                            Success.Add(_school);
                        }
                        else
                            Failed.Add(_school);

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Equals("The underlying provider failed on Open."))
                            return Ok(new { Result = "error", message = ex.Message });
                        else
                            Failed.Add(_school);
                    }
                }
                
                return Ok(new { Result = "success", Success = Success, Failed = Failed });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }            
        }

        [HttpPost]
        public IHttpActionResult SaveCountry(List<Country> model)
        {
            try
            {
                List<Country> Failed = new List<Country>();
                List<Country> Success = new List<Country>();
                foreach (Country _Country in model)
                {
                    try
                    {

                        _countryRepository.Create(new Entities.Models.Country
                        {
                           CountryName=_Country.CountryName,
                           Status=true
                        });
                      
                        Success.Add(_Country);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Equals("The underlying provider failed on Open."))
                            return Ok(new { Result = "error", message = ex.Message });
                        else
                            Failed.Add(_Country);
                    }
                }

                return Ok(new { Result = "success", Success = Success, Failed = Failed });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult SaveZone(List<ZoneVerify> model)
        {
            try
            {
                List<ZoneVerify> Failed = new List<ZoneVerify>();
                List<ZoneVerify> Success = new List<ZoneVerify>();
                foreach (ZoneVerify _zone in model)
                {
                    try
                    {
                        var _country = _countryRepository.FindBy(x => x.CountryName.Trim().ToLower().Equals(_zone.Country.Trim().ToLower())).FirstOrDefault();
                        if (_country != null) {
                            _zoneRepository.Create(new Entities.Models.Zone
                            {
                                ZoneName = _zone.ZoneName,
                                CountryId = _country.Id,
                                Status = true
                            });

                            Success.Add(_zone);
                        }
                        else
                            Failed.Add(_zone);

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Equals("The underlying provider failed on Open."))
                            return Ok(new { Result = "error", message = ex.Message });
                        else
                            Failed.Add(_zone);
                    }

                    
                }
                
                return Ok(new { Result = "success", Success = Success, Failed = Failed });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult SaveState(List<StateVerify> model)
        {
            try
            {
                List<StateVerify> Failed = new List<StateVerify>();
                List<StateVerify> Success = new List<StateVerify>();
                foreach (StateVerify _state in model)
                {
                    try
                    {
                        var _country = _countryRepository.FindBy(x => x.CountryName.Trim().ToLower().Equals(_state.Country.Trim().ToLower())).FirstOrDefault();
                        var _zone = _zoneRepository.FindBy(x => x.ZoneName.Trim().ToLower().Equals(_state.Zone.Trim().ToLower())).FirstOrDefault();
                        if (_country != null && _zone != null)
                        {
                            _stateRepository.Create(new Entities.Models.State
                            {
                                StateName = _state.StateName,
                                StateCode = _state.StateCode,
                                CountryId = _country.Id,
                                ZoneId = _zone.Id,
                                Status = true
                            });

                            Success.Add(_state);
                        }
                        else
                            Failed.Add(_state);

                    }
                    catch (Exception ex)
                    {
                        if(ex.Message.Equals("The underlying provider failed on Open."))
                            return Ok(new { Result = "error", message = ex.Message });
                        else
                            Failed.Add(_state);
                    }
                }

                return Ok(new { Result = "success", Success = Success, Failed = Failed });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }
        
        [HttpPost]
        public IHttpActionResult SaveDistrict(List<DistrictVerify> model)
        {
            try
            {
                List<DistrictVerify> Failed = new List<DistrictVerify>();
                List<DistrictVerify> Success = new List<DistrictVerify>();
                foreach (DistrictVerify item in model)
                {
                    try
                    {
                        var _country = _countryRepository.FindBy(x => x.CountryName.Trim().ToLower().Equals(item.Country.Trim().ToLower())).FirstOrDefault();
                        var _zone = _zoneRepository.FindBy(x => x.ZoneName.Trim().ToLower().Equals(item.Zone.Trim().ToLower())).FirstOrDefault();
                        var _state = _stateRepository.FindBy(x => x.StateName.Trim().ToLower().Equals(item.State.Trim().ToLower())).FirstOrDefault();

                        if (_country != null && _zone != null && _state!=null)
                        {
                            _districtRepository.Create(new Entities.Models.District
                            {
                                DistrictName = item.DistrictName,
                                CountryId = _country.Id,
                                ZoneId = _zone.Id,
                                StateId = _state.Id,
                                Status = true
                            });

                            Success.Add(item);
                        }
                        else
                            Failed.Add(item);

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Equals("The underlying provider failed on Open."))
                            return Ok(new { Result = "error", message = ex.Message });
                        else
                            Failed.Add(item);
                    }
                }

                return Ok(new { Result = "success", Success = Success, Failed = Failed });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult SaveCity(List<CityVerify> model)
        {
            try
            {
                List<CityVerify> Failed = new List<CityVerify>();
                List<CityVerify> Success = new List<CityVerify>();
                foreach (CityVerify item in model)
                {
                    try
                    {
                        var _country = _countryRepository.FindBy(x => x.CountryName.Trim().ToLower().Equals(item.Country.Trim().ToLower())).FirstOrDefault();
                        var _zone = _zoneRepository.FindBy(x => x.ZoneName.Trim().ToLower().Equals(item.Zone.Trim().ToLower())).FirstOrDefault();
                        var _state = _stateRepository.FindBy(x => x.StateName.Trim().ToLower().Equals(item.State.Trim().ToLower())).FirstOrDefault();
                        var _district = _districtRepository.FindBy(x => x.DistrictName.Trim().ToLower().Equals(item.District.Trim().ToLower())).FirstOrDefault();

                        if (_country != null && _zone != null && _state != null&& _district!=null)
                        {
                            _cityRepository.Create(new Entities.Models.City
                            {
                                CityName = item.CityName,
                                DistrictId = _district.Id,
                                CountryId = _country.Id,
                                ZoneId = _zone.Id,
                                StateId = _state.Id,
                                Status = true
                            });

                            Success.Add(item);
                        }
                           else
                            Failed.Add(item);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Equals("The underlying provider failed on Open."))
                            return Ok(new { Result = "error", message = ex.Message });
                        else
                            Failed.Add(item);
                    }
                }

                return Ok(new { Result = "success", Success = Success, Failed = Failed });
            }
            catch (Exception ex)
            {
                return Ok(new { Result = "error", message = ex.Message });
            }
        }
        #endregion

        #region ====================Export Format===================

        [HttpGet]
        public IHttpActionResult ExportFormat(string FileName)
        {
            MemoryStream ms = new MemoryStream();
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Format/" + FileName + ".xlsx");
            using (FileStream source = File.Open(path,FileMode.Open))
            {                
                // Copy source to destination.
                source.CopyTo(ms);
            }
          
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName+".xlsx");
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.End();

            return Ok();
        }
        #endregion
    }
}
