using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.ViewModel.TeacherApp
{
    public class TeacherQuestionViewModel
    {
        public int Id { set; get; }


        public string QuestionText { get; set; }

        public string ImageName { get; set; }
        public int SubjectId { get; set; }
           
               
        [Required]
        public List<Options> Options { set; get;}

        //[Required]
        //public TeacherQuestionOptionViewModel optionModel { get; set; }

      //public static dynamic Parse(IQueryable<TeacherQuestion> modelList)
      //{
      //    return modelList.Select(quiz => new
      //    {
      //        quiz.Id,
      //        quiz.QuestionText,
      //        quiz.ImageName,
      //        quiz.IsActive,
      //        quiz.SubjectId,
      //        quiz.UserId
      //    });
      //}
      //
      // public static dynamic Parse(TeacherQuestion quiz)
      // {
      //     return new
      //     {
      //         Id = quiz.Id,
      //         Question = quiz.QuestionText,
      //         AnswerId = quiz.AnswerId,
      //         ImageName = quiz.ImageName,
      //         Subject = quiz.SubjectId,
      //         UserId = quiz.UserId,
      //         optionModel = quiz.TeacherQuestionOption.Select(x => new
      //         {
      //             Id = x.Id,
      //             options = x.OptionText,
      //             optionsUrl = x.ImageName,
      //             IsAnswer = x.IsAnswer,
      //             questionId = x.QuestionId
      //         }),
      //     };
      // }
      //

        //public class TeacherQuestionOptionViewModel
        //{
        //    public IList<string> options { get; set; }
        //    public IList<string> optionsUrl { get; set; }
        //}

    }
    public class Options
    {      
        public string option { get; set; }
        public string optionsUrl { get; set; }
        public bool IsAnswer { set; get; }
    }
}