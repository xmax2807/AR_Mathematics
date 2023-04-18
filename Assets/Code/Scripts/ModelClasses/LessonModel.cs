using Firebase.Firestore;
using Project.Utils.ExtensionMethods;
[FirestoreData]
public class LessonModel{
    [FirestoreProperty] public string LessonTitle {get; set;}
    [FirestoreProperty] public string LessonVideoFolder {get; set;}
    [FirestoreProperty] public int VideoNumbers {get; set;}
    [FirestoreProperty] public int LessonChapter {get; set;}
    [FirestoreProperty] public int LessonUnit {get; set;}
    [FirestoreProperty] public int LessonSemester {get; set;}
    [FirestoreProperty] public bool LessonStatus {get; set;}

    public string GetFileFormat(int index){
        index.EnsureInRange(VideoNumbers);

        return LessonVideoFolder +'/'+ $"chuong{this.LessonChapter}_bai{this.LessonUnit}_{index + 1}.mp4" ; 
    }
}