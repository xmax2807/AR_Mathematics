using Firebase.Firestore;
[FirestoreData]
public class QuizModel
{

    [FirestoreProperty] public string QuizId { get; set; }
    [FirestoreProperty] public int QuizUnit { get; set; }
    [FirestoreProperty] public string QuizTitle { get; set; }
    [FirestoreProperty] public string QuizIMG { get; set; }
    [FirestoreProperty] public string[] QuizAnswer { get; set; }
    [FirestoreProperty] public int QuizCorrectAnswer { get; set; }
    [FirestoreProperty] public int QuizSemester { get; set; }

    [FirestoreProperty] public int QuizChapter { get; set; }
}

[System.Serializable]
public class QuizData
{
    public int QuizUnit;
    public string QuizTitle;
    public string QuizIMG;
    public string[] QuizAnswer;
    public int QuizCorrectAnswer;
    public int QuizSemester;
    public int QuizChapter;
}