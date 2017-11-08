using SilverzoneERP.Data;
using System.Linq;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities;
using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace SilverzoneERP.Api.api.School
{
    //[Authorize]
    public class answer_sheetRecievingController : ApiController
    {
        private IAnswer_sheetRecieveRepository answer_sheetRecieveRepository { get; set; }
        private IScanned_answerSheetRepository scanned_answerSheetRepository { get; set; }
        private ISchoolRepository schoolRepository { get; set; }
        private IEventManagementRepository eventManagementRepository { get; set; }
        private IErrorLogsRepository errorLogsRepository { get; set; }

        [HttpPost]
        public IHttpActionResult save_ansSheet_Recieve(Answer_sheetRecieve_ViewModel model)
        {
            using (var transaction = answer_sheetRecieveRepository.BeginTransaction())
            {
                try
                {
                    var Answer_sheetRecieve = new Answer_sheetRecieve();
                    Answer_sheetRecieve_ViewModel.parse(model, Answer_sheetRecieve);

                    answer_sheetRecieveRepository.Create(Answer_sheetRecieve);

                    if (model.isEnter_ansSheet_scanInfo)
                    {
                        var scanned_answerSheet = new Scanned_answerSheet();
                        Answer_sheetScanned_ViewModel.parse(
                            model.ansSheet_scanInfo,
                            scanned_answerSheet,
                            Answer_sheetRecieve.Id
                            );

                        scanned_answerSheetRepository.Create(scanned_answerSheet);
                    }

                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpPost]
        public IHttpActionResult get_schoolInfo(schoolSearch_ViewModel model)
        {
            dynamic _result = null;
            if (model.codeType == CodeType.School)
            {
                var r = schoolRepository
                       .findBySchCode(model.Code)
                       .EventManagement
                       .FirstOrDefault(x => x.EventId == model.eventId);

                if (r == null)
                    return Ok(new { result = "" });

                _result = schoolSearch_ViewModel.parse(r);
            }
            else if (model.codeType == CodeType.RegNo)
            {
                var r = eventManagementRepository
                       .FindBy(x => x.RegNo == model.Code)
                       .FirstOrDefault();

                if (r == null)
                    return Ok(new { result = "" });

                _result = schoolSearch_ViewModel.parse(r);
            }

            long eventMgtId = _result.GetType().GetProperty("eventMgtId").GetValue(_result, null);
            var _ansSheet_reciveInfo = schoolSearch_ViewModel
                                      .parse(answer_sheetRecieveRepository
                                            .findBy_EventMgtId(eventMgtId));

            return Ok(new
            {
                schInfo = _result,
                ansSheet_reciveInfo = _ansSheet_reciveInfo,
                bundleNo = answer_sheetRecieveRepository.get_bundleId()
            });
        }

        public IHttpActionResult get_json()
        {
            var _paymentTypes = Enum.GetValues(typeof(Payment_ModeType))
                     .Cast<Payment_ModeType>()
                     .Select(v => new
                     {
                         Id = v.GetHashCode(),
                         Name = v.ToString()
                     }).ToList();

            return Ok(new { result = _paymentTypes });
        }

        [HttpGet]
        public IHttpActionResult GenerateReport(long bundleNo, long eventId)
        {
            try
            {
                HttpContext.Current.Session["ReportName"] = "School/answer_sheetRecive.rpt";
                HttpContext.Current.Session["ProcedureName"] = "get_answer_sheetRecieve";
                HttpContext.Current.Session["TableName"] = "AnswerSheet_Recieve";

                HttpContext.Current.Session["BundleNo"] = bundleNo;
                HttpContext.Current.Session["EventId"] = eventId;

                var domain = HttpContext.Current.Request.Url.Host;
                string url = "http://" + domain + "/FinalReport/School/CrystalReport_Viewer.aspx";
                if (domain == "localhost")
                    url = "http://localhost:55615/FinalReport/School/CrystalReport_Viewer.aspx";

                Uri uri = new System.Uri(url);

                return Redirect(uri);
            }
            catch (Exception ex)
            {
                return Ok(new { result = "error", message = ex.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult exportReport()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SilverzoneERPContext"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("get_answer_sheetRecieve", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            init_sqlCmd("get_answer_sheetRecieve", cmd);

            SqlDataReader sdr = cmd.ExecuteReader();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Load(sdr);
            dt.TableName = "AnswerSheet_Recieve";

            // string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/English_Report.xls");
            WriteDataTableToExcel(
                dt,
                System.Web.Hosting.HostingEnvironment.MapPath("~/Files/English_ReportNew.xlsx"));

            return Ok();
        }

        /// <summary>
        /// FUNCTION FOR EXPORT TO EXCEL
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="worksheetName"></param>
        /// <param name="saveAsLocation"></param>
        /// <returns></returns>
        public bool WriteDataTableToExcel(System.Data.DataTable dataTable, string saveAsLocation)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange;

            try
            {
                // Start Excel and get Application object.
                excel = new Microsoft.Office.Interop.Excel.Application();

                // for making Excel visible
                excel.Visible = false;
                excel.DisplayAlerts = false;

                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Add(Type.Missing);

                // Workk sheet
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;

                // loop through each row and add values to our sheet
                int rowcount = 0;

                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 2)
                        {
                            excelSheet.Cells[1, i] = dataTable.Columns[i - 1].ColumnName;
                            excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                        }

                        excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();

                        //for alternate rows
                        //if (rowcount > 3)
                        //{
                        //    if (i == dataTable.Columns.Count)
                        //    {
                        //        if (rowcount % 2 == 0)
                        //        {
                        //            excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                        //            FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, true);
                        //        }

                        //    }
                        //}

                    }

                }

                // now we resize the columns
                excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                excelCellrange.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;

                //excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dataTable.Columns.Count]];
                //FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel
                excelworkBook.SaveAs(saveAsLocation);
                excelworkBook.Close();
                excel.Quit();
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                excelSheet = null;
                excelCellrange = null;
                excelworkBook = null;
            }

        }

        /// <summary>
        /// FUNCTION FOR FORMATTING EXCEL CELLS
        /// </summary>
        /// <param name="range"></param>
        /// <param name="HTMLcolorCode"></param>
        /// <param name="fontColor"></param>
        /// <param name="IsFontbool"></param>
        public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
            if (IsFontbool == true)
            {
                range.Font.Bold = IsFontbool;
            }
        }

        private void init_sqlCmd(string procName, SqlCommand cmd)
        {
            var session = System.Web.HttpContext.Current.Session;

            if (procName.Equals("get_answer_sheetRecieve"))
            {
                long BundleNo = 1;
                long EventId = 2;

                cmd.Parameters.AddWithValue("@bundleId", BundleNo);
                cmd.Parameters.AddWithValue("@eventId", EventId);
            }
        }
        //*****************  Constructor********************************

        public answer_sheetRecievingController(
            IAnswer_sheetRecieveRepository _answer_sheetRecieveRepository,
            IScanned_answerSheetRepository _scanned_answerSheetRepository,
            ISchoolRepository _schoolRepository,
            IEventManagementRepository _eventManagementRepository,
            IErrorLogsRepository _errorLogsRepository
            )
        {
            answer_sheetRecieveRepository = _answer_sheetRecieveRepository;
            scanned_answerSheetRepository = scanned_answerSheetRepository;
            schoolRepository = _schoolRepository;
            eventManagementRepository = _eventManagementRepository;
            errorLogsRepository = _errorLogsRepository;
        }

    }
}
