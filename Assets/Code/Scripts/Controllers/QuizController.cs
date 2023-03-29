using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class QuizController
{
    List<QuizModel> quizModel;
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    public const string Semester = "QuizSemester";
    public const string Chapter = "QuizChapter";
    public const string Unit = "QuizUnit";
    private void QueryQuizzes(string field1, object fieldVal1, string field2, object fieldVal2)
    {
        quizModel = new();
        if (fieldVal2 == null)
        {
            Query query_Quizzes = db.Collection("quizzes").WhereEqualTo(field1, fieldVal1);
            _ = query_Quizzes.GetSnapshotAsync().ContinueWith((quizSnapshot) =>
            {
                int i = 0;
                foreach (DocumentSnapshot quiz in quizSnapshot.Result.Documents)
                {
                    quizModel.Add(quiz.ConvertTo<QuizModel>());
                    Debug.Log(quizModel[i].QuizTitle);
                    ++i;
                }
            });
        }
        else
        {   
            Query query_Quizzes = db.Collection("quizzes").WhereEqualTo(field1, fieldVal1).WhereEqualTo(field2,fieldVal2);
            _ = query_Quizzes.GetSnapshotAsync().ContinueWith((quizSnapshot) =>
            {
                int i = 0;
                foreach (DocumentSnapshot quiz in quizSnapshot.Result.Documents)
                {
                    quizModel.Add(quiz.ConvertTo<QuizModel>());
                    Debug.Log(quizModel[i].QuizTitle);
                    ++i;
                }
            });
        }
    }
    public void GetQuizzesByLesson(int unit, int chapter)
    {
        QueryQuizzes(Unit, unit,Chapter,chapter);
    }



    public void GetQuizzesByChapter(int chapter)
    {
        QueryQuizzes(Chapter, chapter,null,null);
    }

    public void GetQuizzesBySemester(int semester)
    {
        QueryQuizzes(Semester, semester,null,null);
    }

}