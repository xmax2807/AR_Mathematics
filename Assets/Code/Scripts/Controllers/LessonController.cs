
using UnityEngine;
using Firebase.Firestore;
using Project.Managers;

[System.Serializable]
public class LessonData
{
    public string LessonTitle;
    public string LessonVideoFolder;
    public int VideoNumbers;
    public int LessonChapter;
    public int LessonSemester;
    public int LessonUnit;

    public bool LessonStatus;
}
public class LessonController
{
    LessonModel lessonModel;

    FirebaseFirestore db => DatabaseManager.FirebaseFireStore;
    Firebase.Storage.FirebaseStorage storage => DatabaseManager.Storage;
    public void UploadLesson(LessonData data)
    {
        lessonModel = new LessonModel()
        {
            LessonTitle = data.LessonTitle,
            LessonUnit = data.LessonUnit,
            LessonVideoFolder = data.LessonVideoFolder,
            VideoNumbers = data.VideoNumbers,
            LessonChapter = data.LessonChapter,
            LessonSemester = data.LessonSemester,
            LessonStatus = data.LessonStatus
        };

        db.Collection("lessons").Document().SetAsync(lessonModel);
    }
    public void GetVideo(int unit, int chapter)
    {
        Query documentQuery = db.Collection("lessons").WhereEqualTo("LessonUnit", unit).WhereEqualTo("LessonChapter", chapter);
        documentQuery.GetSnapshotAsync().ContinueWith(task=>{
           QuerySnapshot querySnapshot = task.Result;
            var model = querySnapshot[0].ConvertTo<LessonModel>();
            Debug.Log(model.GetFileFormat(7));
            Debug.Log(storage.GetReferenceFromUrl(model.GetFileFormat(7)).Path);
        });

    }
}