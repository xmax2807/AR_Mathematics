using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class QuizController
{
    List<QuizModel> quizModel;
    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    public const string Semester = "QuizSemester";
    public const string Chapter = "QuizChapter";
    public const string LessonID = "ID_Lesson";
    private void QueryQuizzes(string field, object fieldVal)
    {
        quizModel = new();
        Query query_Quizzes = db.Collection("quizzes").WhereEqualTo(field, fieldVal);
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
    public void GetQuizzesByLesson(string lessonID)
    {
        QueryQuizzes(LessonID, lessonID);
    }



    public void GetQuizzesByChapter(int chapter)
    {
        QueryQuizzes(Chapter,chapter);
    }

    public void GetQuizzesBySemester(int semester)
    {
        QueryQuizzes(Semester,semester);
    }

}