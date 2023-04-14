using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using Project.Managers;
using UnityEngine;

public class QuizController
{
    public List<QuizModel> QuizModels { get; private set; }
    FirebaseFirestore Db => DatabaseManager.FirebaseFireStore;
    public const string Semester = "QuizSemester";
    public const string Chapter = "QuizChapter";
    public const string Unit = "QuizUnit";
    private async void QueryQuizzes(string field1, object fieldVal1, string field2, object fieldVal2)
    {
        QuizModels = new();
        Query query_Quizzes = Db.Collection("quizzes").WhereEqualTo(field1, fieldVal1);
        if (fieldVal2 != null)
        {
            query_Quizzes = query_Quizzes.WhereEqualTo(field2, fieldVal2);
        }

        var quizSnapshot = await query_Quizzes.GetSnapshotAsync();
     
        foreach (DocumentSnapshot quiz in quizSnapshot.Documents)
        {
            var quizModel = quiz.ConvertTo<QuizModel>(); 
            QuizModels.Add(quizModel);
            Debug.Log(quizModel.QuizTitle);
        }
    }
    public async Task<QuizModel[]> GetQuizModelsAsync(object unit, object chapter, object semester){
        bool result = await NetworkManager.Instance.CheckInternetConnectionAsync();
        if(!result){
            return null;
        }
        
        QuizModels = new();
        Query query_Quizzes = Db.Collection("quizzes");
        if (semester != null)
        {
            query_Quizzes = query_Quizzes.WhereEqualTo(Semester, semester);
        }
        if (chapter != null)
        {
            query_Quizzes = query_Quizzes.WhereEqualTo(Chapter, chapter);
        }
        if(unit != null) {
            query_Quizzes = query_Quizzes.WhereEqualTo(Unit, unit);
        }
        Debug.Log((unit == null ? "null" : unit.ToString(), chapter.ToString(), semester.ToString()));
        
        var quizSnapshot = await query_Quizzes.GetSnapshotAsync();
     
        foreach (DocumentSnapshot quiz in quizSnapshot.Documents)
        {
            var quizModel = quiz.ConvertTo<QuizModel>(); 
            QuizModels.Add(quizModel);
            Debug.Log(quizModel.QuizSemester);
        }
        return QuizModels.ToArray();
    }
    public void GetQuizzesByLesson(int unit, int chapter)
    {
        QueryQuizzes(Unit, unit, Chapter, chapter);
    }

    public void GetQuizzesByChapter(int chapter)
    {
        QueryQuizzes(Chapter, chapter, null, null);
    }

    public void GetQuizzesBySemester(int semester)
    {
        QueryQuizzes(Semester, semester, null, null);
    }

}